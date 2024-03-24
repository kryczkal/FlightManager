

using Products;

namespace DataTransformation
{
    /// <summary>
    /// This addon is used to make the objects serializable out of the box if they implement the IDataTransformable interface.
    /// </summary>
    public class Serializable : ISerializable
    {
        public string Serialize(ISerializer serializer)
        {
            return serializer.Serialize((dynamic)this);
        }
    }
    public interface IDataTransformable : ISerializable, Ftr.IFtrCompliant, Binary.IBinaryCompliant
    {
        public string Type { get; }
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
        string GetFormat();
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
        public T? Deserialize<T>(string s) where T : IDataTransformable, new();

        /// <summary>
        /// This is a specialisation of the Deserialize method that returns a DataBaseObject.
        /// It is used when the exact type of the object is not known, but it is known that it is derived from a DataBaseObject.
        /// </summary>
        /// <param name="s"> The string to deserialize</param>
        /// <returns>The deserialized object</returns>
        public DataBaseObject? Deserialize(string s);

        /// <summary>
        /// Parses a file and returns a collection of strings.
        /// </summary>
        /// <param name="filePath">The path of the file to parse.</param>
        /// <returns>A collection of strings, each representing a single deserializable class.</returns>
        public IEnumerable<string> ParseFile(string filePath);
    }
}