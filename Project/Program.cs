using DataTransformation;

public class Program
{
    public static void Main(string[] args)
    {
        List<IDataTransformable> data;
        data = DeserializeExample();
        SeralizeExample(data);
    }

    private static List<IDataTransformable> DeserializeExample()
    {
        // The data is stored in a file with the .ftr format
        // Then we use the DeserializerFactory to create a deserializer for the .ftr format
        // We use the deserializer to parse the file and deserialize the data
        // The deserialized data is then stored in a list of IDataTransformable
        // The list is then returned

        // For simplicity, each class is deserialized to IDataTransformable


        string path = "assets/example_data.ftr";
        List<IDataTransformable> data = new List<IDataTransformable>();

        DeserializerFactory deserializerFactory = new DeserializerFactory();
        IDeserializer? deserializer = deserializerFactory.CreateProduct("ftr");
        if (deserializer == null)
        {
            throw new System.ArgumentNullException();
        }

        foreach (string parsed_line in deserializer.ParseFile(path))
        {
            IDataTransformable? instance = deserializer.Deserialize<IDataTransformable>(parsed_line);
            if (instance != null)
            {
                data.Add(instance);
            }
        }
        return data;
    }
    private static void SeralizeExample(List<IDataTransformable> data)
    {
        // The data is serialized to a file with the .json format
        // We use the JsonSerializer to serialize the data

        string path = "assets/example_data.json";
        ISerializer serializer = new DataTransformation.Json.JsonSerializer();

        // Create an empty file with the .json format
        File.WriteAllText(path, string.Empty);
        foreach (IDataTransformable instance in data)
        {
            string serialized = instance.Serialize(serializer) + ",\n";
            System.IO.File.AppendAllText(path, serialized);
        }

    }
}