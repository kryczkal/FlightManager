/// <summary>
/// Represents an interface that allows a class to initialize its values from a list of strings representing values.
/// The class implementing this interface should know how to cast each string to its appropriate format.
/// </summary>
namespace DataTransformation
{
    public interface ISerializable : ILoadableFromString, IConvertableToString
    {
    }

    public interface ILoadableFromString
    {
        /// <summary>
        /// Loads data from a string array.
        /// </summary>
        /// <param name="data">The string array containing the data to be loaded.</param>
        void InitializeFromString(string[] data);
    }
    public interface IConvertableToString
    {
        /// <summary>
        /// Formats data to a string array.
        /// </summary>
        /// <returns>The string array containing the formatted data.</returns>
        string[] FormatToString();
    }
}