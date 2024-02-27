using System.ComponentModel;
using Products;

namespace DataTransformation.Ftr;
/// <summary>
/// Represents a contract for objects that are compliant with the Ftr format.
/// </summary>
public interface FtrCompliant : ISerializable
{
    /// <summary>
    /// Converts the object to an array of strings in the Ftr format.
    /// </summary>
    /// <returns>An array of strings representing the object in the Ftr format.</returns>
    string[] SaveToFtrString();

    /// <summary>
    /// Loads the object from an array of strings in the Ftr format.
    /// </summary>
    /// <param name="data">An array of strings representing the object in the Ftr format.</param>
    void LoadFromFtrString(string[] data);
}

/// <summary>
/// Deserializer for FTR format.
/// </summary>
public class FtrDeserializer : IDeserializer
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

        string[] vals = s.Split(",", StringSplitOptions.TrimEntries);
        ProductFactory factory = new ProductFactory();
        IDataTransformable? instance = factory.CreateProduct(vals[0]);
        if (instance != null)
        {
            instance.LoadFromFtrString(vals.Skip(1).ToArray());
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
/// <summary>
/// Utility class for working with FTR format.
/// </summary>
public static class FtrUtils
{
    /// <summary>
    /// Parses a string representation of an array into an array of the specified type.
    /// </summary>
    /// <typeparam name="T">The type of the array elements.</typeparam>
    /// <param name="line">The string representation of the array.</param>
    /// <returns>An array of the specified type.</returns>
    public static T[] ParseArray<T>(string line)
    {
        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
        line = line.Substring(1, line.Length - 2);
        string[] values = line.Split(';');
        return Array.ConvertAll(values, s => (T)converter.ConvertFromString(s)!)!;
    }

    /// <summary>
    /// Formats an array into a string representation.
    /// </summary>
    /// <typeparam name="T">The type of the array elements.</typeparam>
    /// <param name="values">The array to format.</param>
    /// <returns>A string representation of the array.</returns>
    public static string FormatArray<T>(T[] values)
    {
        string?[] data = new string[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            data[i] = values[i]!.ToString();
            if (data[i] == null)
            {
                throw new System.ArgumentNullException();
            }
        }
        return "[" + string.Join(";", data) + "]";
    }
}