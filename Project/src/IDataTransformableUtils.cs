namespace DataTransformation;

public static class IDataTransformableUtils
{
    public static void SerializeToFile(IDataTransformable obj, string filePath, ISerializer serializer)
    {
        // Serialize the data to the file and append a comma and newline
        string serialized = obj.Serialize(serializer) + ",\n";
        System.IO.File.AppendAllText(filePath, serialized);
    }

}