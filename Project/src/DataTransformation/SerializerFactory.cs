using Factory;

namespace DataTransformation;

/// <summary>
/// Represents a factory for creating serializers.
/// </summary>
public class SerializerFactory : Factory<ISerializer>
{
    public SerializerFactory()
    {
        Register("json", () => new Json.JsonSerializer());
    }
}