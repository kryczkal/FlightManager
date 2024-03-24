namespace projob;

public static class Settings
{
    public static string LoadPath { get; private set; } = "assets/example_data.ftr";
    public static int minSimulationOffset { get; private set; } = 2;
    public static int maxSimulationOffset { get; private set; } = 3;

    public static bool IsSimulationRealTime { get; private set; } = false;
    public static int SimulationSpeed { get; private set; } = 400;

    public static DateTime SimulationStartTime { get; private set; } = DateTime.Now;
    private static DateTime _simulationTime = SimulationStartTime;

    public static DateTime SimulationTime
    {
        get
        {
            if (IsSimulationRealTime) return DateTime.Now;

            var realTimeElapsed = DateTime.Now - SimulationStartTime;
            var simulatedTimeElapsed = TimeSpan.FromMilliseconds(realTimeElapsed.TotalMilliseconds * SimulationSpeed);

            return SimulationStartTime + simulatedTimeElapsed;
        }
    }
}