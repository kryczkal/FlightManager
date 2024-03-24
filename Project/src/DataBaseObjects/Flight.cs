using DataTransformation;
using Mapsui.Projections;
using projob;

namespace Products;
public class Flight : DataBaseObject
{
    private static readonly string _type = "Flight";
    public string Type => _type;
    public UInt64 ID { get; set; }
    public UInt64 Origin { get; set; } // As Airport ID
    public UInt64 Target { get; set; } // As Airport ID
    public DateTime TakeoffTime { get; set; }
    public DateTime LandingTime { get; set; }

    private Single _longitude;

    public Single Longitude
    {
        get => _longitude;
        set
        {
            PrevLongitude = _longitude;
            _longitude = value;
        }
    }

    private Single _latitude;

    public Single Latitude
    {
        get => _latitude;
        set
        {
            PrevLatitude = _latitude;
            _latitude = value;
        }
    }

    private Single PrevLongitude { get; set; }
    private Single PrevLatitude { get; set; }
    public Single AMSL { get; set; }
    public UInt64 PlaneID { get; set; }
    public UInt64[] Crew { get; set; } // As their IDs
    public UInt64[] Load { get; set; } // As Cargo IDs
    public (float Longitude, float Latitude) CurrentPositionLonLat
    {
        get
        {
            // Get the origin and target airports from the DataBaseManager
            Airport originAirport = DataBaseManager.Airports[Origin];
            Airport targetAirport = DataBaseManager.Airports[Target];

            // Calculate the flight progress

            // Interpolate the current latitude and longitude
            float currentLatitude = originAirport.Latitude + Progress * (targetAirport.Latitude - originAirport.Latitude);
            float currentLongitude = originAirport.Longitude + Progress * (targetAirport.Longitude - originAirport.Longitude);

            return (currentLongitude, currentLatitude);
        }
    }
    public float Progress
    {
        get
        {
            DateTime currentTime = DateTime.UtcNow;

            // Calculate the total flight duration in ticks
            long totalFlightDurationTicks = LandingTime.Ticks - TakeoffTime.Ticks;

            // Calculate the elapsed time since takeoff in ticks
            long elapsedTimeTicks = currentTime.Ticks - TakeoffTime.Ticks;

            // Calculate the flight progress as a float between 0 and 1
            float flightProgress = (float)elapsedTimeTicks / totalFlightDurationTicks;

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
            double deltaX = currentPositionXY.x - previousPositionXY.x;
            double deltaY = currentPositionXY.y - previousPositionXY.y;

            // Calculate the rotation in radians relative to the vector (0, 1)
            double rotation = Math.Atan2(deltaX, deltaY);

            return rotation;
        }
    }

    /*
     * Methods
     */
    public void UpdatePosition()
    {
        (Longitude, Latitude) = CurrentPositionLonLat;
    }

    /*
     * Central database functions
     */
    public override void AddToCentral()
    {
        if (!DataBaseManager.Flights.TryAdd(ID, this)) throw new InvalidOperationException("Flight with the same ID already exists.");
    }

    /*
     * Format Compliancy : FTR, Binary, JSON
     */
    public override void LoadFromFtrString(string[] data)
    {
        ID = UInt64.Parse(data[0]);
        Origin = UInt64.Parse(data[1]);
        Target = UInt64.Parse(data[2]);

        // The ftr file contains only the hours and minutes of the day so we set the date to today
        TakeoffTime = DateTime.Today.Add(TimeSpan.Parse(data[3]));
        LandingTime = DateTime.Today.Add(TimeSpan.Parse(data[4]));

        Longitude = Single.Parse(data[5]);
        Latitude = Single.Parse(data[6]);
        AMSL = Single.Parse(data[7]);
        PlaneID = UInt64.Parse(data[8]);
        Crew = DataTransformation.Ftr.FtrUtils.ParseArray<UInt64>(data[9]);
        Load = DataTransformation.Ftr.FtrUtils.ParseArray<UInt64>(data[10]);
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
        data[10] = DataTransformation.Ftr.FtrUtils.FormatArray<UInt64>(Crew);
        data[11] = DataTransformation.Ftr.FtrUtils.FormatArray<UInt64>(Load);
        return data;
    }

    public override byte[] SaveToByteArray()
    {
        throw new NotImplementedException();
    }

    public override void LoadFromByteArray(byte[] data)
    {
        int offset = 0;
        ID = BitConverter.ToUInt64(data, offset); offset += sizeof(UInt64);
        Origin = BitConverter.ToUInt64(data, offset); offset += sizeof(UInt64);
        Target = BitConverter.ToUInt64(data, offset); offset += sizeof(UInt64);

        // TakeoffTime taken as number of ms since Epoch(UTC) and parsed to DateTime
        TakeoffTime = DateTimeOffset.FromUnixTimeMilliseconds(BitConverter.ToInt64(data, offset)).UtcDateTime; offset += sizeof(UInt64);
        // LandingTime taken as number of ms since Epoch(UTC) and parsed to DateTime
        LandingTime = DateTimeOffset.FromUnixTimeMilliseconds(BitConverter.ToInt64(data, offset)).UtcDateTime; offset += sizeof(UInt64);

        // Since there is a bug in the NetworkSourceManager, there can be flights where the TakeoffTime is after the LandingTime
        // In this case we add a day to the LandingTime
        if (TakeoffTime > LandingTime) LandingTime = LandingTime.AddDays(1);

        PlaneID = BitConverter.ToUInt64(data, offset); offset += sizeof(UInt64);
        UInt16 CrewLength = BitConverter.ToUInt16(data, offset); offset += sizeof(UInt16);
        Crew = new UInt64[CrewLength];
        for (int i = 0; i < CrewLength; i++)
        {
            Crew[i] = BitConverter.ToUInt64(data, offset); offset += sizeof(UInt64);
        }
        UInt16 LoadLength = BitConverter.ToUInt16(data, offset); offset += sizeof(UInt16);
        Load = new UInt64[LoadLength];
        for (int i = 0; i < LoadLength; i++)
        {
            Load[i] = BitConverter.ToUInt64(data, offset); offset += sizeof(UInt64);
        }

        // Longitude, Latitude and AMSL are not used in this method
    }
}