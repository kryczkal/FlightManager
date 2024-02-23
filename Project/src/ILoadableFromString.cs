/// <summary>
/// Represents an interface that allows a class to initialize its values from a list of strings representing values.
/// The class implementing this interface should know how to cast each string to its appropriate format.
/// </summary>
public interface ILoadableFromString
{
    /// <summary>
    /// Loads data from a string array.
    /// </summary>
    /// <param name="data">The string array containing the data to be loaded.</param>
    void LoadFromString(string[] data);
}