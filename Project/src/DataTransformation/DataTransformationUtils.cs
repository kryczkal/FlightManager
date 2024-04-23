using System.Collections;
using projob.DataBaseObjects;

namespace DataTransformation;

public static class DataTransformationUtils
{
    public static void SerializeObjToFile(IDataTransformable obj, string filePath, ISerializer serializer)
    {
        // Serialize the data to the file and append a comma and newline
        var serialized = obj.Serialize(serializer) + ",\n";
        File.AppendAllText(filePath, serialized);
    }

    public static void SerializeObjListToFile(List<IDataTransformable> objs, string filePath, ISerializer serializer)
    {
        foreach (var obj in objs) SerializeObjToFile(obj, filePath, serializer);
    }

    public static void SerializeObjToFile<T>(T obj, string filePath, ISerializer serializer)
        where T : IDataTransformable
    {
        // Serialize the data to the file and append a comma and newline
        var serialized = obj.Serialize(serializer) + ",\n";
        File.AppendAllText(filePath, serialized);
    }

    public static void SerializeObjListToFile<T>(List<T> objs, string filePath, ISerializer serializer)
        where T : IDataTransformable
    {
        foreach (var obj in objs) SerializeObjToFile<T>(obj, filePath, serializer);
    }

    public static IEnumerable<DataBaseObject?> DeserializeFileToObjList(string filePath, IDeserializer deserializer)
    {
        // Read the file and split the data into an array
        string[] data = File.ReadAllText(filePath).Split("\r\n", StringSplitOptions.RemoveEmptyEntries);
        foreach (var obj in data)
        {
            // Deserialize the data and yield return it
            yield return deserializer.Deserialize(obj);
        }
    }
}
