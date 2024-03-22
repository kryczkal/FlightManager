using Factory;

namespace DataTransformation;
/// <summary>
/// Represents a factory for creating deserializers.
/// </summary>
public class DeserializerFactory : Factory.Factory<IDeserializer>
{
    public DeserializerFactory()
    {
        Register("ftr", () => new DataTransformation.Ftr.FtrDeserializer());
        Register("json", () => new DataTransformation.Json.JsonDeserializer());
        Register("bin", () => new DataTransformation.Binary.BinaryDeserializer());
    }
}