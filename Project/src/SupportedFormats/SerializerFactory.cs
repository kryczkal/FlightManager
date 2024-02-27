namespace DataTransformation;
/// <summary>
/// Represents a factory for creating serializers.
/// </summary>
public class SerializerFactory : Factory.Factory<ISerializer>
{
    public SerializerFactory()
    {
        Register("json", () => new DataTransformation.Json.JsonSerializer());
    }
}