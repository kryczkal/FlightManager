using FlightTrackerGUI;
using FlightTrackerData;
using Mapsui.Projections;
using Products;

namespace projob;

public static class GuiManager
{
    private static FlightsGUIData _flightsGUIData = new();

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
            UpdateData();
        }
    }

    public static void UpdateData()
    {
        List<FlightGUI> data = new();
        foreach (Flight flight in DataBaseManager.Flights.Values)
        {
            if(flight.Progress == 0) continue;

            FlightGUI flight_data;
            if (Math.Abs(flight.Progress - 1) < 1e-10)
            {
                // Flight finished

                flight.UpdatePosition();
                flight_data = new FlightGUI(){
                    WorldPosition = new WorldPosition(
                        DataBaseManager.Airports[flight.Target].Latitude,
                        DataBaseManager.Airports[flight.Target].Longitude
                        ),
                    ID = flight.ID,
                    MapCoordRotation = 0,
                };

                // Remove the flight from the database
                DataBaseManager.Flights.TryRemove(flight.ID, out _);
            }
            else
            {
                flight.UpdatePosition();
                flight_data = new FlightGUI()
                {
                    WorldPosition = new WorldPosition(flight.Latitude, flight.Longitude),
                    ID = flight.ID,
                    MapCoordRotation = Math.Abs(flight.Progress - 1) < 1e-9 ? 0 : flight.RotationRadians,
                };

            }
            data.Add(flight_data);
        }
        _flightsGUIData.UpdateFlights(data);
        Runner.UpdateGUI(_flightsGUIData);
    }
}