using FlightTrackerGUI;
using FlightTrackerData;
using Mapsui.Projections;
using Products;

namespace projob;

public static class GuiManager
{
    private static DatabaseFlightGUIData _flightsGuiData = new();

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
        foreach (var flight in DataBaseManager.Flights.Values) flight.UpdatePosition();

        _flightsGuiData.UpdateFlights(DataBaseManager.Flights.Values.ToList());
        Runner.UpdateGUI(_flightsGuiData);
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
            return DataBaseFlights[index].ID;
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