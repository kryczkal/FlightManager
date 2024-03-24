using Factory;

namespace DataTransformation;

/// <summary>
/// Represents a factory for creating deserializers.
/// </summary>
public class DeserializerFactory : Factory<IDeserializer>
{
    public DeserializerFactory()
    {
        Register("ftr", () => new Ftr.FtrDeserializer());
        Register("json", () => new Json.JsonDeserializer());
        Register("bin", () => new Binary.BinaryDeserializer());
    }
}