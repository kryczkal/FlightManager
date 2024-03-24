namespace projob;

public static class Settings
{
    /*
     * Network Source settings
     */
    public static string LoadPath { get; private set; } = "assets/example_data.ftr";
    public static int minSimulationOffset { get; private set; } = 2;
    public static int maxSimulationOffset { get; private set; } = 3;

    /*
     * Gui Simulation settings
     */
    public static bool IsSimulationRealTime { get; private set; } = false;
    public static int SimulationSpeedMultiplier { get; private set; } = 1000;
    public static int GuiUpdateIntervalMs { get; private set; } = 10;

    /*
     * Non editable members
     */
    public static DateTime SimulationStartTime { get; } = DateTime.Now;
    public static DateTime SimulationTime
    {
        get
        {
            if (IsSimulationRealTime) return DateTime.Now;

            var realTimeElapsed = DateTime.Now - SimulationStartTime;
            var simulatedTimeElapsed = TimeSpan.FromMilliseconds(realTimeElapsed.TotalMilliseconds * SimulationSpeedMultiplier);

            return SimulationStartTime + simulatedTimeElapsed;
        }
    }
}