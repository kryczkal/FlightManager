using DataTransformation;
using NetworkSourceSimulator;

namespace projob;

public static class SharedIndexTracker
{
    private static int _value = 0;

    public static void Set(int value)
    {
        Interlocked.Exchange(ref _value, value);
    }
    public static int Value
    {
        get { return Interlocked.CompareExchange(ref _value, 0, 0); }
    }
}

public static class Program
{
    public static void Main(string[] args)
    {
        SharedIndexTracker.Set(0);

        var networkSource = new NetworkSourceSimulator.NetworkSourceSimulator(Settings.LoadPath, Settings.minSimulationOffset, Settings.maxSimulationOffset);
        Thread simulationThread = new Thread(new ThreadStart(() => SimulateNetworkSource(networkSource)))
        {
            IsBackground = true
        };
        simulationThread.Start();

        ConsoleWork(networkSource);

    }

    public static void ConsoleWork(NetworkSourceSimulator.NetworkSourceSimulator networkSource)
    {
        IDeserializer? deserializer = new DeserializerFactory().CreateProduct("bin");
        ISerializer? serializer = new SerializerFactory().CreateProduct("json");
        if (deserializer == null) throw new System.Exception("Deserializer not found");
        if (serializer == null) throw new System.Exception("Serializer not found");

        bool running = true;
        while (running)
        {
            Console.WriteLine("Enter a command: ");
            string? command = Console.ReadLine();
            switch (command)
            {
                case "exit":
                    running = false;
                    break;
                case "print":
                    UpdateObjectCentral(networkSource, deserializer);
                    MakeSnapshot(deserializer, serializer);
                    break;
            }
        }
    }

    private static void MakeSnapshot(IDeserializer deserializer,
        ISerializer serializer)
    {
        Console.WriteLine("Printing data...");
        string filePath = "assets/snapshot_" + DateTime.Now.ToString("HH_mm_ss") + "."+ serializer.GetFormat();
        DataTransformation.Utils.SerializeObjListToFile(ObjectCentral.objects, filePath, serializer);
    }

    private static void UpdateObjectCentral(NetworkSourceSimulator.NetworkSourceSimulator networkSource, IDeserializer deserializer)
    {
        for(int i = ObjectCentral.objects.Count(); i < SharedIndexTracker.Value; i++)
        {
            IDataTransformable? instance = deserializer.Deserialize<IDataTransformable>(
                BinaryStringAdapter.BinAsString(networkSource.GetMessageAt(i).MessageBytes));
            if (instance == null) throw new System.Exception("Instance is null");
            ObjectCentral.objects.Add(instance);
        }
    }

    private static void SimulateNetworkSource(NetworkSourceSimulator.NetworkSourceSimulator networkSource)
    {
        networkSource.OnNewDataReady += SetLastMessageId;
        networkSource.Run();
    }

    private static void SetLastMessageId(object sender, NewDataReadyArgs args)
    {
        System.Console.WriteLine($"New data is ready! {args.MessageIndex}");
        SharedIndexTracker.Set(args.MessageIndex);
    }
}