using DataTransformation.FileParser;
using DataTransformation;
using Factory;
using DataTransformation;
using DataTransformation.StringFormatter;
using System.Text.Json;

public class Program
{
    public static void Main(string[] args)
    {
        DeserializeExample();
        SeralizeExample();
    }

    private static void DeserializeExample()
    {
        string path = "assets/example_data.ftr";
        IParser parser = new FTRParser();
        AbstractFactory factory = new BaseFactory();
        Dictionary<string, List<ISerializable>> instances = Deserializer.Deserialize(path, factory, parser);
        foreach (KeyValuePair<string, List<ISerializable>> kvp in instances)
        {
            Console.WriteLine(kvp.Key);
            foreach (ISerializable instance in kvp.Value)
            {
                Console.WriteLine(instance);
            }
        }
    }
    private static void SeralizeExample()
    {
        string path = "assets/example_data.ftr";
        IParser parser = new FTRParser();
        AbstractFactory factory = new BaseFactory();
        Dictionary<string, List<ISerializable>> instances = Deserializer.Deserialize(path, factory, parser);

        foreach (KeyValuePair<string, List<ISerializable>> kvp in instances)
        {
            string file_path = $"assets/serialized/{kvp.Key}";
            IFormatter formatter = new JsonFormatter();
            Serializer.Serialize(file_path, formatter, kvp.Value);
        }
    }
}