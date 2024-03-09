using System.Diagnostics;
using DataTransformation;
using NetworkSourceSimulator;
using System.Threading;
using projob;


public static class SharedInteger
{
    private static int _value = 0;

    public static void Set(int value)
    {
        Interlocked.Exchange(ref _value, value);
    }
    public static void Increment()
    {
        Interlocked.Increment(ref _value);
    }

    public static void Decrement()
    {
        Interlocked.Decrement(ref _value);
    }

    public static int Value
    {
        get { return Interlocked.CompareExchange(ref _value, 0, 0); }
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        SharedInteger.Set(0);

        var networkSource = new NetworkSourceSimulator.NetworkSourceSimulator("assets/example_data.ftr", 500, 1000);
        Thread SimulationThread = new Thread(new ThreadStart(() => SimulateNetworkSource(networkSource)))
            {
                IsBackground = true
            };
        SimulationThread.Start();

        ConsoleWork(networkSource);

        Environment.Exit(0);
    }

    public static void ConsoleWork(NetworkSourceSimulator.NetworkSourceSimulator networkSource)
    {
        IDeserializer deserializer = new DeserializerFactory().CreateProduct("bin")!;
        ISerializer serializer = new SerializerFactory().CreateProduct("json");
        bool running = true;
        while (running)
        {
            Console.WriteLine("Enter a command: ");
            string command = Console.ReadLine();
            switch (command)
            {
                case "exit":
                    running = false;
                    break;
                case "print":
                {
                    Console.WriteLine("Printing data...");
                    BinaryStringAdapter binaryStringAdapter = new BinaryStringAdapter(networkSource.GetMessageAt(SharedInteger.Value).MessageBytes);
                    IDataTransformable instance = deserializer.Deserialize<IDataTransformable>(binaryStringAdapter.BinAsString());

                    Console.WriteLine(instance.Serialize(serializer));

                    string filePath = "assets/snapshot_" + DateTime.Now.ToString("HH_mm_ss") + "."+ serializer.GetFormat();
                    DataTransformation.IDataTransformableUtils.SerializeToFile(instance, filePath, serializer);
                    break;
                }
            }
        }
    }

    private static void SimulateNetworkSource(NetworkSourceSimulator.NetworkSourceSimulator networkSource)
    {
        networkSource.OnNewDataReady += SetLastMessageId;
        networkSource.Run();
    }

    private static void SetLastMessageId(object sender, NewDataReadyArgs args)
    {
        System.Console.WriteLine("New data is ready!");
        SharedInteger.Set(args.MessageIndex);
    }
}