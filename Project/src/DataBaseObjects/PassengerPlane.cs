using DataTransformation;
using projob;
using projob.DataBaseObjects;
using projob.DataBaseSQL;
using projob.media;

namespace Products;

public class PassengerPlane : DataBaseObject, IReportable
{
    private static readonly string _type = "PassengerPlane";
    public override string Type => _type;

    public string Serial { get; set; }
    public string ISOCountryCode { get; set; }
    public string Name { get; set; }
    public ushort FirstClassSize { get; set; }
    public ushort BusinessClassSize { get; set; }
    public ushort EconomyClassSize { get; set; }

    public PassengerPlane()
    {
        _sqlAccessors = new Dictionary<string, SqlAccessor>
        {
            {"ID", new SqlAccessor(() => Id.ToString(), (val) => Id = ulong.Parse(val))},
            {"Serial", new SqlAccessor(() => Serial, (val) => Serial = val)},
            {"ISOCountryCode", new SqlAccessor(() => ISOCountryCode, (val) => ISOCountryCode = val)},
            {"Name", new SqlAccessor(() => Name, (val) => Name = val)},
            {"FirstClassSize", new SqlAccessor(() => FirstClassSize.ToString(), (val) => FirstClassSize = ushort.Parse(val))},
            {"BusinessClassSize", new SqlAccessor(() => BusinessClassSize.ToString(), (val) => BusinessClassSize = ushort.Parse(val))},
            {"EconomyClassSize", new SqlAccessor(() => EconomyClassSize.ToString(), (val) => EconomyClassSize = ushort.Parse(val))}
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
        if (!DataBaseManager.PassengerPlanes.TryAdd(Id, this))
            throw new InvalidOperationException("PassengerPlane with the same ID already exists.");
    }

    /*
     * Format Compliancy : FTR, Binary, JSON
     */
    public override void LoadFromFtrString(string[] data)
    {
        Id = ulong.Parse(data[0]);
        Serial = data[1];
        ISOCountryCode = data[2];
        Name = data[3];
        FirstClassSize = ushort.Parse(data[4]);
        BusinessClassSize = ushort.Parse(data[5]);
        EconomyClassSize = ushort.Parse(data[6]);
    }

    public override string[] SaveToFtrString()
    {
        string[] data = new string[8];
        data[0] = Type;
        data[1] = Id.ToString();
        data[2] = Serial;
        data[3] = ISOCountryCode;
        data[4] = Name;
        data[5] = FirstClassSize.ToString();
        data[6] = BusinessClassSize.ToString();
        data[7] = EconomyClassSize.ToString();
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
        Serial = System.Text.Encoding.ASCII.GetString(data, offset, 10);
        offset += 10;
        ISOCountryCode = System.Text.Encoding.ASCII.GetString(data, offset, 3);
        offset += 3;
        var ModelLength = BitConverter.ToUInt16(data, offset);
        offset += sizeof(ushort);
        Name = System.Text.Encoding.ASCII.GetString(data, offset, ModelLength);
        offset += ModelLength;
        FirstClassSize = BitConverter.ToUInt16(data, offset);
        offset += sizeof(ushort);
        BusinessClassSize = BitConverter.ToUInt16(data, offset);
        offset += sizeof(ushort);
        EconomyClassSize = BitConverter.ToUInt16(data, offset);
        offset += sizeof(ushort);
    }

    /*
     * IReportable implementation
     */
    public string AcceptMediaReport(Media media)
    {
        return media.ReportPassengerPlane(this);
    }
}