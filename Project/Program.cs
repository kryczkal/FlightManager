using DataTransformation;
using projob.DataBaseObjects;
using projob.media;

namespace projob;

public static class Program
{
    public static void Main(string[] args)
    {
        DataBaseManager.LoadFromFtrFile(Settings.DataBaseManager.LoadPath);
        DataBaseManager.CreateObjReferences();

        var networkSourceManager = new NetworkSourceManager(
            new NetworkSourceSimulator.NetworkSourceSimulator(
                Settings.NetworkSourceSimulator.LoadPath,
                Settings.NetworkSourceSimulator.MinSimulationOffset,
                Settings.NetworkSourceSimulator.MaxSimulationOffset),
            true
        );

        //List<DataBaseObject> allObjects = DataBaseManager.GetAllObjects();

        networkSourceManager.RunParallel();
        GuiManager.RunParallel();
        ConsoleWork();
    }

    public static void ConsoleWork()
    {
        // Setting up snapshot functionality
        var serializer = new SerializerFactory().CreateProduct("json");
        if (serializer == null) throw new Exception("Serializer not found");

        // Setting up news generator
        List<Media> mediaList = new List<Media>
        {
            new Newspaper("CNN"),
            new Radio("BBC Radio"),
            new Television()
        };

        var running = true;
        while (running)
        {
            Console.Write("Enter a command: ");
            var command = Console.ReadLine();
            switch (command)
            {
                case "exit":
                    running = false;
                    break;
                case "print":
                    DataBaseManager.MakeSnapshot(serializer);
                    break;
                case "report":
                    var newsGenerator = new NewsGenerator(mediaList, DataBaseManager.GetReportableObjects());
                    foreach (var news in newsGenerator.GenerateNextNews())
                    {
                        Console.WriteLine(news);
                    }
                    break;
                case "clear":
                    Console.Clear();
                    break;
            }
        }
    }
}