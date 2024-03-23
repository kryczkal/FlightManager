using DataTransformation;
using projob;

namespace Products;

public class DataBaseObject : Serializable, IDataTransformable, IAddableToCentral
{
    /*
     * Properties
     */
    public virtual string Type { get; }

    /*
     * Central database functions
     */
    public virtual void AddToCentral()
    {
        throw new InvalidOperationException("This method should be overriden in the derived class.");
    }

    /*
     * Format compliancy functions
     */
    public virtual string[] SaveToFtrString()
    {
        throw new InvalidOperationException("This method should be overriden in the derived class.");
    }

    public virtual void LoadFromFtrString(string[] data)
    {
        throw new InvalidOperationException("This method should be overriden in the derived class.");
    }

    public virtual byte[] SaveToByteArray()
    {
        throw new InvalidOperationException("This method should be overriden in the derived class.");
    }

    public virtual void LoadFromByteArray(byte[] data)
    {
        throw new InvalidOperationException("This method should be overriden in the derived class.");
    }
}