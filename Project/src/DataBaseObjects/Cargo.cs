using DataTransformation;
using projob;
using projob.DataBaseObjects;
using projob.DataBaseSQL;

namespace Products;

public class Cargo : DataBaseObject
{
    private static readonly string _type = "Cargo";
    public override string Type => _type;

    public float Weight { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }

    public Cargo()
    {
        _sqlAccessors = new Dictionary<string, SqlAccessor>
        {
            {"ID", new SqlAccessor(() => Id.ToString(), (val) => Id = ulong.Parse(val))},
            {"Weight", new SqlAccessor(() => Weight.ToString(), (val) => Weight = float.Parse(val))},
            {"Code", new SqlAccessor(() => Code, (val) => Code = val)},
            {"Description", new SqlAccessor(() => Description, (val) => Description = val)}
        };
    }

    /*
     * SQL Accessors
     */
    private Dictionary<string, SqlAccessor> _sqlAccessors;
    public override Dictionary<string, SqlAccessor> Accessors => _sqlAccessors;

    /*
     * Central database functions
     */
    public override void UpdateObjReferences(DataBaseObject sender, ulong oldId, ulong newId)
    {
    }

    public override List<ulong>? GetReferencedIds()
    {
        return null;
    }

    public override void AcceptAddToCentral()
    {
        if (!DataBaseManager.Cargos.TryAdd(Id, this))
            throw new InvalidOperationException("Cargo with the same ID already exists.");
    }

    /*
     * Format Compliancy : FTR, Binary, JSON
     */
    public override void LoadFromFtrString(string[] data)
    {
        Id = ulong.Parse(data[0]);
        Weight = float.Parse(data[1]);
        Code = data[2];
        Description = data[3];
    }

    public override string[] SaveToFtrString()
    {
        string[] data = new string[5];
        data[0] = Type;
        data[1] = Id.ToString();
        data[2] = Weight.ToString();
        data[3] = Code;
        data[4] = Description;
        return data;
    }

    public override byte[] SaveToByteArray()
    {
        throw new NotImplementedException();
    }

    public override void LoadFromByteArray(byte[] data)
    {
        var offset = 0;
        Id = BitConverter.ToUInt64(data, offset);
        offset += sizeof(ulong);
        Weight = BitConverter.ToSingle(data, offset);
        offset += sizeof(float);
        Code = System.Text.Encoding.ASCII.GetString(data, offset, 6);
        offset += 6;
        var DescriptionLength = BitConverter.ToUInt16(data, offset);
        offset += sizeof(ushort);
        Description = System.Text.Encoding.ASCII.GetString(data, offset, DescriptionLength);
        offset += DescriptionLength;
    }
}