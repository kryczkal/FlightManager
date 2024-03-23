using DataTransformation;

namespace Products;

public class DataBaseObject : Serializable, IDataTransformable
{
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

    public virtual string Type { get; }
}