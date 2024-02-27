namespace Factory;
/// <summary>
/// Represents a generic factory that can create instances of a specified type.
/// </summary>
/// <typeparam name="T">The type of objects that the factory can create.</typeparam>
public abstract class Factory<T>
{
    private readonly Dictionary<string, Func<T>> _instances = new Dictionary<string, Func<T>>();

    /// <summary>
    /// Registers a creator function for a specific type.
    /// </summary>
    /// <param name="type">The type to register.</param>
    /// <param name="creator">The function that creates instances of the specified type.</param>
    /// <exception cref="ArgumentException">Thrown if the type is already registered.</exception>
    protected void Register(string type, Func<T> creator)
    {
        if (!_instances.ContainsKey(type))
        {
            _instances[type] = creator;
        }
        else
        {
            throw new ArgumentException($"Type {type} is already registered.");
        }
    }

    /// <summary>
    /// Creates an instance of the specified type.
    /// </summary>
    /// <param name="type">The type of object to create.</param>
    /// <returns>An instance of the specified type, or null if the type is not registered.</returns>
    public T? CreateProduct(string type)
    {
        if (_instances.TryGetValue(type, out var creator))
        {
            return creator();
        }
        else
        {
            return default; // This is null for reference types
        }
    }
}


