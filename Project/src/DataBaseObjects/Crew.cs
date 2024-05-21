using DataTransformation;
using projob;
using projob.DataBaseObjects;
using projob.DataBaseSQL;

namespace Products;

public class Crew : DataBaseObject
{
    private static readonly string _type = "Crew";
    public override string Type => _type;

    public string Name { get; set; }
    public ulong Age { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public ushort Practice { get; set; }
    public string Role { get; set; }

    public Crew()
    {
        _sqlAccessors = new Dictionary<string, SqlAccessor>
        {
            {"ID", new SqlAccessor(() => Id.ToString(), (val) => Id = ulong.Parse(val))},
            {"Name", new SqlAccessor(() => Name, (val) => Name = val)},
            {"Age", new SqlAccessor(() => Age.ToString(), (val) => Age = ulong.Parse(val))},
            {"Phone", new SqlAccessor(() => Phone, (val) => Phone = val)},
            {"Email", new SqlAccessor(() => Email, (val) => Email = val)},
            {"Practice", new SqlAccessor(() => Practice.ToString(), (val) => Practice = ushort.Parse(val))},
            {"Role", new SqlAccessor(() => Role, (val) => Role = val)}
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
        if (!DataBaseManager.Crews.TryAdd(Id, this))
            throw new InvalidOperationException("Crew with the same ID already exists.");
    }

    /*
     * Format Compliancy : FTR, Binary, JSON
     */
    public override void LoadFromFtrString(string[] data)
    {
        Id = ulong.Parse(data[0]);
        Name = data[1];
        Age = ulong.Parse(data[2]);
        Phone = data[3];
        Email = data[4];
        Practice = ushort.Parse(data[5]);
        Role = data[6];
    }

    public override string[] SaveToFtrString()
    {
        string[] data = new string[7];
        data[0] = Type;
        data[1] = Id.ToString();
        data[2] = Name;
        data[3] = Age.ToString();
        data[4] = Phone;
        data[5] = Email;
        data[6] = Practice.ToString();
        data[7] = Role;
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
        var NameLength = BitConverter.ToUInt16(data, offset);
        offset += sizeof(short);
        Name = System.Text.Encoding.ASCII.GetString(data, offset, NameLength);
        offset += NameLength;
        Age = BitConverter.ToUInt16(data, offset);
        offset += sizeof(short);
        Phone = System.Text.Encoding.ASCII.GetString(data, offset, 12);
        offset += 12;
        var EmailLength = BitConverter.ToUInt16(data, offset);
        offset += sizeof(ushort);
        Email = System.Text.Encoding.ASCII.GetString(data, offset, EmailLength);
        offset += EmailLength;
        Practice = BitConverter.ToUInt16(data, offset);
        offset += sizeof(ushort);
        Role = System.Text.Encoding.ASCII.GetString(data, offset, 1);
    }
}