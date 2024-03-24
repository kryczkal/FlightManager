using System.Diagnostics;
using System.Numerics;
using DataTransformation;
using Mapsui.Projections;
using projob;

namespace Products;

public class Flight : DataBaseObject
{
    private static readonly string _type = "Flight";
    public string Type => _type;
    public ulong ID { get; set; }
    public ulong Origin { get; set; } // As Airport ID
    public ulong Target { get; set; } // As Airport ID
    public DateTime TakeoffTime { get; set; }
    public DateTime LandingTime { get; set; }
    private float _longitude;

    public float Longitude
    {
        get => _longitude;
        set
        {
            PrevLongitude = _longitude;
            _longitude = value;
        }
    }

    private float _latitude;

    public float Latitude
    {
        get => _latitude;
        set
        {
            PrevLatitude = _latitude;
            _latitude = value;
        }
    }

    private WorldPosition _worldPosition;

    public WorldPosition WorldPosition
    {
        get
        {
            _worldPosition.Longitude = Longitude;
            _worldPosition.Latitude = Latitude;
            return _worldPosition;
        }
    }

    private float PrevLongitude { get; set; }
    private float PrevLatitude { get; set; }
    public float AMSL { get; set; }
    public ulong PlaneID { get; set; }
    public ulong[] Crew { get; set; } // As their IDs
    public ulong[] Load { get; set; } // As Cargo IDs

    public float Progress
    {
        get
        {
            var currentTime = Settings.SimulationTime;

            // Calculate the total flight duration in ticks
            var totalFlightDurationTicks = LandingTime.Ticks - TakeoffTime.Ticks;

            // Calculate the elapsed time since takeoff in ticks
            var elapsedTimeTicks = currentTime.Ticks - TakeoffTime.Ticks;

            // Calculate the flight progress as a float between 0 and 1
            var flightProgress = (float)elapsedTimeTicks / totalFlightDurationTicks;

            // Ensure the flight progress is within the range [0, 1]
            flightProgress = Math.Max(0, flightProgress);
            flightProgress = Math.Min(1, flightProgress);

            return flightProgress;
        }
    }

    public double RotationRadians
    {
        get
        {
            // Convert longitude and latitude to x, y coordinates
            var previousPositionXY = SphericalMercator.FromLonLat(PrevLongitude, PrevLatitude);
            var currentPositionXY = SphericalMercator.FromLonLat(Longitude, Latitude);

            // Calculate the difference in x and y coordinates
            var deltaX = currentPositionXY.x - previousPositionXY.x;
            var deltaY = currentPositionXY.y - previousPositionXY.y;

            // Calculate the rotation in radians relative to the vector (0, 1)
            var rotation = Math.Atan2(deltaX, deltaY);
            return rotation;
        }
    }

    /*
     * Methods
     */
    public (float Longitude, float Latitude) CalcCurrentPositionLonLat()
    {
        // Get the origin and target airports from the DataBaseManager
        var originAirport = DataBaseManager.Airports[Origin];
        var targetAirport = DataBaseManager.Airports[Target];

        // Convert the origin and target coordinates to quaternions
        var origin = QuaternionHelper.LonLatToQuaternion(originAirport.Longitude, originAirport.Latitude);
        var target = QuaternionHelper.LonLatToQuaternion(targetAirport.Longitude, targetAirport.Latitude);

        // Calculate the flight progress
        var flightProgress = Progress;

        // Perform spherical interpolation
        var currentPosition = Quaternion.Slerp(origin, target, flightProgress);

        // Quaternion has the property that q and -q represent the same rotation
        // If the dot product of the current position and the target is negative, the current position is the opposite of the target
        if (Quaternion.Dot(currentPosition, target) < 0)
            currentPosition = -currentPosition;
        // Convert the interpolated position back to longitude and latitude
        (var currentLongitude, var currentLatitude) = QuaternionHelper.QuaternionToLonLat(currentPosition);
        return (currentLongitude, currentLatitude);
    }

    public void UpdatePosition()
    {
        (Longitude, Latitude) = CalcCurrentPositionLonLat();
    }

    /*
     * Central database functions
     */
    public override void AddToCentral()
    {
        if (!DataBaseManager.Flights.TryAdd(ID, this))
            throw new InvalidOperationException("Flight with the same ID already exists.");
    }

    /*
     * Format Compliancy : FTR, Binary, JSON
     */
    public override void LoadFromFtrString(string[] data)
    {
        ID = ulong.Parse(data[0]);
        Origin = ulong.Parse(data[1]);
        Target = ulong.Parse(data[2]);

        // The ftr file contains only the hours and minutes of the day so we set the date to today
        TakeoffTime = DateTime.Today.Add(TimeSpan.Parse(data[3]));
        LandingTime = DateTime.Today.Add(TimeSpan.Parse(data[4]));

        Longitude = float.Parse(data[5]);
        Latitude = float.Parse(data[6]);
        AMSL = float.Parse(data[7]);
        PlaneID = ulong.Parse(data[8]);
        Crew = DataTransformation.Ftr.FtrUtils.ParseArray<ulong>(data[9]);
        Load = DataTransformation.Ftr.FtrUtils.ParseArray<ulong>(data[10]);
    }

    public override string[] SaveToFtrString()
    {
        string[] data = new string[12];
        data[0] = Type;
        data[1] = ID.ToString();
        data[2] = Origin.ToString();
        data[3] = Target.ToString();
        data[4] = TakeoffTime.ToString("HH:mm");
        data[5] = LandingTime.ToString("HH:mm");
        data[6] = Longitude.ToString();
        data[7] = Latitude.ToString();
        data[8] = AMSL.ToString();
        data[9] = PlaneID.ToString();
        data[10] = DataTransformation.Ftr.FtrUtils.FormatArray<ulong>(Crew);
        data[11] = DataTransformation.Ftr.FtrUtils.FormatArray<ulong>(Load);
        return data;
    }

    public override byte[] SaveToByteArray()
    {
        throw new NotImplementedException();
    }

    public override void LoadFromByteArray(byte[] data)
    {
        var offset = 0;
        ID = BitConverter.ToUInt64(data, offset);
        offset += sizeof(ulong);
        Origin = BitConverter.ToUInt64(data, offset);
        offset += sizeof(ulong);
        Target = BitConverter.ToUInt64(data, offset);
        offset += sizeof(ulong);

        // TakeoffTime taken as number of ms since Epoch(UTC) and parsed to DateTime
        TakeoffTime = DateTimeOffset.FromUnixTimeMilliseconds(BitConverter.ToInt64(data, offset)).UtcDateTime;
        offset += sizeof(ulong);
        // LandingTime taken as number of ms since Epoch(UTC) and parsed to DateTime
        LandingTime = DateTimeOffset.FromUnixTimeMilliseconds(BitConverter.ToInt64(data, offset)).UtcDateTime;
        offset += sizeof(ulong);

        // Since there is a bug in the NetworkSourceManager, there can be flights where the TakeoffTime is after the LandingTime
        // In this case we add a day to the LandingTime
        if (TakeoffTime > LandingTime) LandingTime = LandingTime.AddDays(1);

        PlaneID = BitConverter.ToUInt64(data, offset);
        offset += sizeof(ulong);
        var CrewLength = BitConverter.ToUInt16(data, offset);
        offset += sizeof(ushort);
        Crew = new ulong[CrewLength];
        for (var i = 0; i < CrewLength; i++)
        {
            Crew[i] = BitConverter.ToUInt64(data, offset);
            offset += sizeof(ulong);
        }

        var LoadLength = BitConverter.ToUInt16(data, offset);
        offset += sizeof(ushort);
        Load = new ulong[LoadLength];
        for (var i = 0; i < LoadLength; i++)
        {
            Load[i] = BitConverter.ToUInt64(data, offset);
            offset += sizeof(ulong);
        }

        // Longitude, Latitude and AMSL are not used in this method
    }
}