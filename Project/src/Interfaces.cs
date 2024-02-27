/// <summary>
/// Represents a namespace for data transformation related functionality. Mainly used for serialization and deserialization of objects.
/// </summary>
namespace DataTransformation
{
    public interface IDataTransformable : ISerializable, Ftr.FtrCompliant
    {
    }
    /// <summary>
    /// Represents an interface for objects that can be serialized.
    /// </summary>
    public interface ISerializable
    {
        /// <summary>
        /// Serializes the object using the specified serializer.
        /// </summary>
        /// <param name="serializer">The serializer to use for serialization.</param>
        /// <returns>A string representation of the serialized object.</returns>
        public string Serialize(ISerializer serializer);
    }
    /// <summary>
    /// Represents a serializer that converts objects to their string representation.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Serializes the specified object into a string.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>A string representation of the serialized object.</returns>
        string Serialize<T>(T obj);
    };
    /// <summary>
    /// Represents an interface for deserializing data and parsing files.
    /// </summary>
    public interface IDeserializer
    {
        /// <summary>
        /// Deserializes a string into an object of type T that implements IDataTransformable.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize.</typeparam>
        /// <param name="s">The string to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public IDataTransformable? Deserialize<T>(string s) where T : IDataTransformable;

        /// <summary>
        /// Parses a file and returns a collection of strings.
        /// </summary>
        /// <param name="filePath">The path of the file to parse.</param>
        /// <returns>A collection of strings, each representing a single deserializable class.</returns>
        public IEnumerable<string> ParseFile(string filePath);
    }
}