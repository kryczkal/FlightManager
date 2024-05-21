using DataTransformation;
using projob.DataBaseObjects;
using projob.DataBaseSQL;
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

        List<DataBaseObject> allObjects = DataBaseManager.GetAllObjects();

        //networkSourceManager.RunParallel();
        //GuiManager.RunParallel();
        ConsoleWork();

        //SqlCommand command1 = new SelectCommand("Airport");
        //command1.Append(
        //    new DisplayCommand(new[] { "ID", "Name", "Code", "Longitude", "Latitude", "AMSL", "ISOCountryCode" }.ToList())
        //    );
        //command1.ExecuteChain();

        //GlobalLogger.Log($"Airport Count {DataBaseManager.Airports.Count()}", LogLevel.Info);
        //GlobalLogger.Log("Deleting all airports.", LogLevel.Info);

        //SqlCommand command2 = new SelectCommand("Airport");
        //command2.Append(new DeleteCommand());
        //command2.ExecuteChain();

        //GlobalLogger.Log($"Airport Count {DataBaseManager.Airports.Count()}", LogLevel.Info);

        //SqlCommand command3 = new SelectCommand("Airport");
        //command3.Append(
        //    new DisplayCommand(new[] { "ID", "Name", "Code", "Longitude", "Latitude", "AMSL", "ISOCountryCode" }.ToList())
        //    );
        //command3.ExecuteChain();

        //GlobalLogger.Log("Adding a new airport.", LogLevel.Info);

        //SqlCommand command4 = new SelectCommand("Airport");

        //command4.Append(
        //    new AddCommand(
        //    [
        //            new("ID", "12"),
        //    ])
        //    );
        //command4.ExecuteChain();
        //GlobalLogger.Log($"{DataBaseManager.Airports.Count()}", LogLevel.Info);

        //SqlCommand command5 = new SelectCommand("Airport");
        //command5.Append(
        //    new DisplayCommand(new[] { "ID", "Name", "Code", "Longitude", "Latitude", "AMSL", "ISOCountryCode" }.ToList())
        //    );
        //command5.ExecuteChain();

        //// Update Airports
        //SqlCommand command6 = new SelectCommand("Airport");
        //command6.Append(
        //    new UpdateCommand(
        //    [
        //            new("Name", "Test Airport Updated"),
        //            new("Code", "TST Updated"),
        //            new("Longitude", "420"),
        //            new("Latitude", "69"),
        //            new("AMSL", "123"),
        //            new("ISOCountryCode", "TST Updated")
        //    ])
        //    );
        //command6.ExecuteChain();

        //SqlCommand command7 = new SelectCommand("Airport");
        //command7.Append(
        //    new DisplayCommand(new[] { "ID", "Name", "Code", "Longitude", "Latitude", "AMSL", "ISOCountryCode" }.ToList())
        //    );
        //command7.ExecuteChain();
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
                default:
                    if (command == null) continue;
                    SqlCommand? sqlCommand = QueryParser.ParseQuery(command);
                    if (sqlCommand != null) sqlCommand.ExecuteChain();
                    break;
            }
        }
    }
}