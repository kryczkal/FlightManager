using DataTransformation;
using projob;
using projob.media;

namespace Products;

public abstract class DataBaseObject : Serializable, IDataTransformable, IAddableToCentral
{
    /*
     * Properties
     */
    public virtual string Type { get; }

    /*
     * Central database functions
     */
    public abstract void AddToCentral();

    /*
     * Format compliancy functions
     */
    public abstract string[] SaveToFtrString();
    public abstract void LoadFromFtrString(string[] data);
    public abstract byte[] SaveToByteArray();
    public abstract void LoadFromByteArray(byte[] data);
}