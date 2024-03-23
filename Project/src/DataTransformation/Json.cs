
using Products;

namespace DataTransformation.Json;

public class JsonDeserializer : IDeserializer
{
    // This is a setup for the future implementation of the Deserialize method
    public T? Deserialize<T>(string s) where T : IDataTransformable, new()
    {
        throw new NotImplementedException();
    }

    public DataBaseObject? Deserialize(string s)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> ParseFile(string filePath)
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            string? line = reader.ReadLine();
            if (line != null)
            {
                yield return line;
            }
            else
            {
                yield break;
            }
        }
    }
}

public class JsonSerializer : ISerializer
{
    public string Serialize<T>(T obj)
    {
        return System.Text.Json.JsonSerializer.Serialize<T>(obj);
    }

    public string GetFormat()
    {
        return "json";
    }
}