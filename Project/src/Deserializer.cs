using DataTransformation;
using Factory;
using DataTransformation.FileParser;

/// <summary>
/// Provides methods for deserializing data from a file using a specified factory and parser.
/// </summary>
namespace DataTransformation
{
    public static class Deserializer
    {
        /// <summary>
        /// Deserializes data from a file and returns a dictionary of instances.
        /// </summary>
        /// <param name="path">The path to the file.</param>
        /// <param name="factory">The factory used to create instances.</param>
        /// <param name="parser">The parser used to parse the file.</param>
        /// <returns>A dictionary of instances, where the key is the type (as string) and the value is a list of instances of that type.</returns>
        public static Dictionary<string, List<ISerializable>> Deserialize(string path, AbstractFactory factory, IParser parser)
        {
            Dictionary<string, List<ISerializable>> instances = new Dictionary<string, List<ISerializable>>();

            IEnumerable<string[]> parsed_lines = parser.ParseFile(path);

            foreach (string[] parsed_line in parsed_lines)
            {
                string type = parsed_line[0];
                ISerializable instance = factory.Create(type);

                string[] class_data = parsed_line.Skip(1).ToArray();
                instance.InitializeFromString(class_data);

                if (instances.ContainsKey(parsed_line[0]))
                    instances[parsed_line[0]].Add(instance);
                else
                    instances.Add(parsed_line[0], new List<ISerializable> { instance });
            }
            return instances;
        }
    }

}