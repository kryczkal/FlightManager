using FlightTrackerGUI;
using FlightTrackerData;
using Mapsui.Projections;
using Products;

namespace projob;

public static class GuiManager
{
    private static FlightsGUIData _flightsGUIData = new();
    private static long current_time_ms = 0;

    public static void RunParallel()
    {
        Thread simulationThread = new Thread(() => RunGui())
        {
            IsBackground = true
        };
        Thread updateThread = new Thread(() => PeriodicUpdate())
        {
            IsBackground = true
        };
        simulationThread.Start();
        updateThread.Start();
    }
    private static void RunGui()
    {
        //networkSource.OnNewDataReady += SetLastMessageId;
        Runner.Run(); // Run the GUI from the FlightTrackerGUI namespace
    }
    private static void PeriodicUpdate()
    {
        while (true)
        {
            Thread.Sleep(100);
            current_time_ms += 1000;
            UpdateData();
        }
    }

    public static void UpdateData()
    {
        List<FlightGUI> data = new();
        foreach (Flight flight in DataBaseManager.Flights.Values)
        {
            var previousPosition = (flight.Latitude, flight.Longitude);
            var currentPosition = GetPlaneCurrentPosition(flight);
            var rotation = GetPlaneRotation(
    (DataBaseManager.Airports[flight.Origin].Latitude,
                    DataBaseManager.Airports[flight.Origin].Longitude),
    (DataBaseManager.Airports[flight.Target].Latitude,
                    DataBaseManager.Airports[flight.Target].Longitude)
                );

            flight.Latitude = currentPosition.Latitude;
            flight.Longitude = currentPosition.Longitude;

            var flight_data = new FlightGUI()
            {
                WorldPosition = new WorldPosition(currentPosition.Latitude, currentPosition.Longitude),
                ID = flight.ID,
                MapCoordRotation = rotation,
            };
            data.Add(flight_data);
        }
        _flightsGUIData.UpdateFlights(data);
        Runner.UpdateGUI(_flightsGUIData);
    }

    public static float GetFlightProgress(string LandingTime, string TakeoffTime){
    // Convert the takeoff and landing times from string to long (milliseconds since EPOCH)
    long takeoffTimeMs = long.Parse(TakeoffTime);
    long landingTimeMs = long.Parse(LandingTime);

    if (landingTimeMs < takeoffTimeMs)
    {
        landingTimeMs += 24 * 60 * 60 * 1000;
    }

    // Get the current time in milliseconds since EPOCH
    long currentTimeMs = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() + 60 * 60 * 1000;

    // Calculate the total flight duration
    long totalFlightDurationMs = landingTimeMs - takeoffTimeMs;

    // Calculate the elapsed time since takeoff
    long elapsedTimeMs = currentTimeMs - takeoffTimeMs;

    // Calculate the flight progress as a float between 0 and 1
    float flightProgress = (float)elapsedTimeMs / totalFlightDurationMs;

    // Ensure the flight progress is within the range [0, 1]
    flightProgress = Math.Max(0, flightProgress);
    flightProgress = Math.Min(1, flightProgress);

    return flightProgress;
    }

    public static (float Latitude, float Longitude) GetPlaneCurrentPosition(Flight flight)
    {
    // Get the origin and target airports from the DataBaseManager
    Airport originAirport = DataBaseManager.Airports[flight.Origin];
    Airport targetAirport = DataBaseManager.Airports[flight.Target];

    // Calculate the flight progress
    float flightProgress = GetFlightProgress(flight.LandingTime, flight.TakeoffTime);


    // Interpolate the current latitude and longitude
    float currentLatitude = originAirport.Latitude + flightProgress * (targetAirport.Latitude - originAirport.Latitude);
    float currentLongitude = originAirport.Longitude + flightProgress * (targetAirport.Longitude - originAirport.Longitude);

    return (currentLatitude, currentLongitude);
    }

    public static double GetPlaneRotation((float Latitude, float Longitude) previousPosition, (float Latitude, float Longitude) currentPosition)
    {
    // Convert longitude and latitude to x, y coordinates
    var previousPositionXY = SphericalMercator.FromLonLat(previousPosition.Longitude, previousPosition.Latitude);
    var currentPositionXY = SphericalMercator.FromLonLat(currentPosition.Longitude, currentPosition.Latitude);

    // Calculate the difference in x and y coordinates
    double deltaX = currentPositionXY.x - previousPositionXY.x;
    double deltaY = currentPositionXY.y - previousPositionXY.y;

    // Calculate the rotation in radians relative to the vector (0, 1)
    double rotation = Math.Atan2(deltaY, deltaX);

    return rotation;
    }
}