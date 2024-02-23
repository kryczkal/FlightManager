using Classes;
using Factory;
using FileParser;

public static class Deserializer
{
    public static Dictionary<string, List<ILoadableFromString>> Deserialize(string path, AbstractFactory factory, IParser parser)
    {
        Dictionary<string, List<ILoadableFromString>> instances = new Dictionary<string, List<ILoadableFromString>>();

        IEnumerable<string[]> parsed_lines = parser.ParseFile(path);

        foreach (string[] parsed_line in parsed_lines)
        {
            string type = parsed_line[0];
            ILoadableFromString instance = factory.Create(type);

            string[] class_data = parsed_line.Skip(1).ToArray();
            instance.LoadFromString(class_data);

            if (instances.ContainsKey(parsed_line[0]))
                instances[parsed_line[0]].Add(instance);
            else
                instances.Add(parsed_line[0], new List<ILoadableFromString> { instance });
        }
        return instances;
    }
}