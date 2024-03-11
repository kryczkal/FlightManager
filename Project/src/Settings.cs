namespace projob;

public static class Settings
{
    public static string LoadPath { get; private set; } = "assets/example_data.ftr";
    public static int minSimulationOffset { get; private set; } = 2;
    public static int maxSimulationOffset { get; private set; } = 3;
}