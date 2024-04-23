using FlightTrackerGUI;
using FlightTrackerData;
using Mapsui.Projections;
using Products;
using projob.DataBaseObjects;

namespace projob;

public static class GuiManager
{
    private static DatabaseFlightGUIData _flightsGuiData = new();
    private static List<Flight> _flightsToKill = new();

    public static void RunParallel()
    {
        var simulationThread = new Thread(() => RunGui())
        {
            IsBackground = true
        };
        var updateThread = new Thread(() => PeriodicUpdate())
        {
            IsBackground = true
        };
        simulationThread.Start();
        updateThread.Start();
    }

    private static void RunGui()
    {
        Runner.Run(); // Run the GUI from the FlightTrackerGUI namespace
    }

    private static void PeriodicUpdate()
    {
        while (true)
        {
            Thread.Sleep(Settings.GuiUpdateIntervalMs);
            UpdateData();
        }
    }

    public static void UpdateData()
    {
        foreach (var flight in DataBaseManager.Flights.Values) flight.RecalcPosition();

        List<Flight> flights = DataBaseManager.Flights.Values.ToList();
        flights.AddRange(_flightsToKill);
        _flightsGuiData.UpdateFlights(flights);
        _flightsToKill.Clear();
        Runner.UpdateGUI(_flightsGuiData);
    }

    public static void KillFlight(ulong id)
    {
        _flightsToKill.Add(new Flight()
        {
            Id = id,
            Latitude = 0,
            Longitude = 0,
        });
    }

    // An adapter class to provide the data to the GUI
    public class DatabaseFlightGUIData : FlightsGUIData
    {
        private List<Flight> DataBaseFlights = new();

        public DatabaseFlightGUIData(List<Flight> _flights)
        {
            DataBaseFlights = _flights;
        }

        public DatabaseFlightGUIData()
        {
        }

        public void UpdateFlights(List<Flight> _flights)
        {
            DataBaseFlights = _flights;
        }

        public override int GetFlightsCount()
        {
            return DataBaseFlights.Count;
        }

        public override ulong GetID(int index)
        {
            return DataBaseFlights[index].Id;
        }

        public override WorldPosition GetPosition(int index)
        {
            return DataBaseFlights[index].WorldPosition;
        }

        public override double GetRotation(int index)
        {
            return DataBaseFlights[index].RotationRadians;
        }
    }
}