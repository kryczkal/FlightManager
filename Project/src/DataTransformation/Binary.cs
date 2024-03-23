using System.Text;
using Products;

namespace DataTransformation.Binary;
/// <summary>
/// Represents a contract for objects that are compliant with the Byte format.
/// </summary>
public interface IBinaryCompliant : ISerializable
{
    /// <summary>
    /// Converts the object to an array of Bytes.
    /// </summary>
    /// <returns>An array of Bytes representing the object.</returns>
    byte[] SaveToByteArray();

    /// <summary>
    /// Loads the object from an array of bytes in the predefined format.
    /// </summary>
    /// <param name="data">An array of bytes representing the object in a predefined format.</param>
    void LoadFromByteArray(byte[] data);
}

/// <summary>
/// Deserializer for Binary format.
/// </summary>
public class BinaryDeserializer : IDeserializer
{
    /// <summary>
    /// Deserializes the input string into an instance of type T.
    /// </summary>
    /// <typeparam name="T">The type of the instance to deserialize.</typeparam>
    /// <param name="s">The input string to deserialize.</param>
    /// <returns>The deserialized instance of type T.</returns>
    public T? Deserialize<T>(string s) where T : IDataTransformable, new()
    {
        byte[] byte_data = BinaryStringAdapter.StringAsBin(s);
        string code = System.Text.Encoding.ASCII.GetString(byte_data.Skip(1).Take(2).ToArray());
        byte[] class_vals = byte_data.Skip(7).ToArray();

        T? instance = new T();
        instance.LoadFromByteArray(class_vals);
        return instance;
    }
    public DataBaseObject? Deserialize(string s)
    {
        byte[] byte_data = BinaryStringAdapter.StringAsBin(s);
        string code = System.Text.Encoding.ASCII.GetString(byte_data.Skip(1).Take(2).ToArray());
        byte[] class_vals = byte_data.Skip(7).ToArray();

        DataBaseObjectFactory factory = new DataBaseObjectFactory();
        DataBaseObject? instance = factory.CreateProduct(code);
        if (instance != null)
        {
            instance.LoadFromByteArray(class_vals);
        }
        return instance;
    }

    public string GetObjCode(byte[] byte_data)
    {
        return System.Text.Encoding.ASCII.GetString(byte_data.Skip(1).Take(2).ToArray());
    }
    /// <summary>
    /// Parses a file and returns an enumerable collection of strings representing deserializable classes.
    /// </summary>
    /// <param name="filePath">The path to the file to be parsed.</param>
    /// <returns>An enumerable collection of strings representing deserializable classes.</returns>
    public IEnumerable<string> ParseFile(string filePath)
    {
        using StreamReader reader = new StreamReader(filePath);
        string? line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return line;
        }
    }
}

/// <summary>
/// This class is used to convert a string to a binary string and vice versa
/// Conversion is done 1:1 with no compression meaning 1 byte = 8 bits = 8 characters in the binary string
/// </summary>
public static class BinaryStringAdapter
{
    public static string BinAsString(byte[] byteArray)
    {
        StringBuilder binaryString = new StringBuilder();
        foreach (byte b in byteArray)
        {
            binaryString.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
        }
        return binaryString.ToString();
    }

    public static byte[] StringAsBin(string str)
    {
        return Enumerable.Range(0, str.Length / 8)
                                    .Select(i => Convert.ToByte(str.Substring(i * 8, 8), 2))
                                    .ToArray();;
    }
}
