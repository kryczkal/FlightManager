namespace projob;

public static class Settings
{
    /*
     * Database settings
     */
    public static class DataBaseManager
    {
        public static string LoadPath { get; private set; } = "assets/example_data.ftr";
    }
    /*
     * Network Source settings
     */
    public static class NetworkSourceSimulator
    {
        public static string LoadPath { get; private set; } = "assets/example_data.ftre";
        public static int MinSimulationOffset { get; private set; } = 2;
        public static int MaxSimulationOffset { get; private set; } = 3;

    }

    /*
     * Gui Simulation settings
     */
    public static bool IsSimulationRealTime { get; private set; } = false;
    public static int SimulationSpeedMultiplier { get; private set; } = 100;
    public static int GuiUpdateIntervalMs { get; private set; } = 10;

    /*
     * Logger settings
     */
    public static class LoggerSettings
    {
        static LoggerSettings()
        {
            LoggerChain = new ConsoleHandler(LogLevel.Info);
            LoggerChain.SetNext(new FileHandler(LogLevel.Info, LogFilePath));
        }

        public static string LogFormat => "{0:u} {1}: {2}";
        public static string LogFilePath => "log.txt";
        public static ILoggerHandler? LoggerChain { get; }
    }

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