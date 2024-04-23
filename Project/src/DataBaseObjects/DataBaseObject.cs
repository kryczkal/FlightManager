using DataTransformation;

namespace projob.DataBaseObjects;

public abstract class DataBaseObject : Serializable, IDataTransformable, IAddableToCentral
{
    /*
     * Properties
     */
    /// <summary>
    /// A string that represents the type of the object
    /// </summary>
    public virtual string Type { get; }
    /// <summary>
    /// ID of the object
    /// </summary>
    public ulong Id { get; set; }

    /*
     * Consolidation functions
     */

    public delegate void OnIdChangeHandler(DataBaseObject sender, ulong oldId, ulong newId);
    public event OnIdChangeHandler OnIdChange = delegate { };
    /// <summary>
    /// Change the ID of the object and notify the subscribers
    /// </summary>
    /// <param name="newId"></param>
    public void ChangeId(ulong newId)
    {
        ulong oldId = Id;
        Id = newId;
        OnIdChange(this, oldId, newId);
    }
    /// <summary>
    ///  Update the references of the object when the ID is changed
    /// </summary>
    /// <param name="sender"> sender </param>
    /// <param name="oldId"> the old ID </param>
    /// <param name="newId"> the new ID </param>
    public abstract void UpdateObjReferences(DataBaseObject sender, ulong oldId, ulong newId);

    /// <summary>
    /// Returns a list of IDs that are referenced by this object.
    /// </summary>
    /// <returns></returns>
    public abstract List<ulong>? GetReferencedIds();

    /*
     * Central database functions
     */
    /// <summary>
    /// Add the object to the central database
    /// </summary>
    public abstract void AcceptAddToCentral();

    /*
     * Format compliancy functions
     */
    public abstract string[] SaveToFtrString();
    public abstract void LoadFromFtrString(string[] data);
    public abstract byte[] SaveToByteArray();
    public abstract void LoadFromByteArray(byte[] data);
}