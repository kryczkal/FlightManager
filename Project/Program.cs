using System.IO;
using FileParser;
using Factory;
using Classes;

public class Program
{
    public static void Main(string[] args)
    {
        string path = "assets/example_data.ftr";
        IParser parser = new FTRParser();
        AbstractFactory factory = new BaseFactory();
        Dictionary<string, List<ILoadableFromString>> instances = Deserializer.Deserialize(path, factory, parser);
        foreach (KeyValuePair<string, List<ILoadableFromString>> kvp in instances)
        {
            Console.WriteLine(kvp.Key);
            foreach (ILoadableFromString instance in kvp.Value)
            {
                Console.WriteLine(instance);
            }
        }
    }
}