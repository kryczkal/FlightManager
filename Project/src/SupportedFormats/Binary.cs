using Products;
using projob;

namespace DataTransformation.Binary;
/// <summary>
/// Represents a contract for objects that are compliant with the Byte format.
/// </summary>
public interface BinaryCompliant : ISerializable
{
    /// <summary>
    /// Converts the object to an array of strings in the Ftr format.
    /// </summary>
    /// <returns>An array of strings representing the object in the Ftr format.</returns>
    byte[] SaveToByteArray();

    /// <summary>
    /// Loads the object from an array of bytes in the predefined format.
    /// </summary>
    /// <param name="data">An array of bytes representing the object in a predefined format.</param>
    void LoadFromByteArray(byte[] data);
}

/// <summary>
/// Deserializer for FTR format.
/// </summary>
public class BinaryDeserializer : IDeserializer
{
    /// <summary>
    /// Deserializes the input string into an instance of type T.
    /// </summary>
    /// <typeparam name="T">The type of the instance to deserialize.</typeparam>
    /// <param name="s">The input string to deserialize.</param>
    /// <returns>The deserialized instance of type T.</returns>
    public IDataTransformable? Deserialize<T>(string s) where T : IDataTransformable
    {
        // IDataTransformable is used in order to avoid casting instance made from factory to specific type
        // This can be modified to return a specific type

        BinaryStringAdapter binaryStringAdapter = new BinaryStringAdapter(s);
        byte[] byte_data = binaryStringAdapter.StringAsBin();
        string code = System.Text.Encoding.ASCII.GetString(byte_data.Skip(1).Take(2).ToArray());
        byte[] class_vals = byte_data.Skip(7).ToArray();

        ProductFactory factory = new ProductFactory();
        IDataTransformable? instance = factory.CreateProduct(code);
        if (instance != null)
        {
            instance.LoadFromByteArray(class_vals);
        }
        return instance;
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