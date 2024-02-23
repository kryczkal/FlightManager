using DataTransformation.FileParser;
using DataTransformation;
using Factory;
using Classes;

public class Program
{
    public static void Main(string[] args)
    {
        DeserializeExample();
    }

    private static void DeserializeExample()
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