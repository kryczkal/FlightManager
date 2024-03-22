namespace DataTransformation;

public static class DataTransformationUtils
{
    public static void SerializeObjToFile(IDataTransformable obj, string filePath, ISerializer serializer)
    {
        // Serialize the data to the file and append a comma and newline
        string serialized = obj.Serialize(serializer) + ",\n";
        System.IO.File.AppendAllText(filePath, serialized);
    }
    public static void SerializeObjListToFile(List<IDataTransformable> objs, string filePath, ISerializer serializer)
    {
        foreach(IDataTransformable obj in objs)
        {
            SerializeObjToFile(obj, filePath, serializer);
        }
    }
    

}