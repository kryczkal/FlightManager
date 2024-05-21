using System.Globalization;
using DataTransformation;
using projob;
using projob.DataBaseObjects;
using projob.DataBaseSQL;
using projob.media;

namespace Products;

public class Airport : DataBaseObject, IReportable
{
    private static readonly string _type = "Airport";
    public override string Type => _type;
    public string Name { get; set; }
    public string Code { get; set; }
    public float Longitude { get; set; }
    public float Latitude { get; set; }
    public float AMSL { get; set; }
    public string ISOCountryCode { get; set; }

    public Airport()
    {
        _sqlAccessors = new Dictionary<string, SqlAccessor>
        {
            {"ID", new SqlAccessor(() => Id.ToString(), (val) => Id = ulong.Parse(val))},
            {"Name", new SqlAccessor(() => Name, (val) => Name = val)},
            {"Code", new SqlAccessor(() => Code, (val) => Code = val)},
            {"Longitude", new SqlAccessor(() => Longitude.ToString(CultureInfo.CurrentCulture), (val) => Longitude = float.Parse(val))},
            {"Latitude", new SqlAccessor(() => Latitude.ToString(CultureInfo.CurrentCulture), (val) => Latitude = float.Parse(val))},
            {"AMSL", new SqlAccessor(() => AMSL.ToString(CultureInfo.CurrentCulture), (val) => AMSL = float.Parse(val))},
            {"ISOCountryCode", new SqlAccessor(() => ISOCountryCode, (val) => ISOCountryCode = val)}
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
        if (!DataBaseManager.Airports.TryAdd(Id, this))
            throw new InvalidOperationException("Airport with the same ID already exists.");
    }

    /*
     * Format Compliancy : FTR, Binary, JSON
     */
    public override void LoadFromFtrString(string[] data)
    {
        Id = ulong.Parse(data[0]);
        Name = data[1];
        Code = data[2];
        Longitude = float.Parse(data[3]);
        Latitude = float.Parse(data[4]);
        AMSL = float.Parse(data[5]);
        ISOCountryCode = data[6];
    }

    public override string[] SaveToFtrString()
    {
        string[] data = new string[8];
        data[0] = _type;
        data[1] = Id.ToString();
        data[2] = Name;
        data[3] = Code;
        data[4] = Longitude.ToString();
        data[5] = Latitude.ToString();
        data[6] = AMSL.ToString();
        data[7] = ISOCountryCode;
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
        var nameLength = BitConverter.ToUInt16(data, offset);
        offset += sizeof(ushort);
        Name = System.Text.Encoding.ASCII.GetString(data, offset, nameLength);
        offset += nameLength;
        Code = System.Text.Encoding.ASCII.GetString(data, offset, 3);
        offset += 3;
        Longitude = BitConverter.ToSingle(data, offset);
        offset += sizeof(float);
        Latitude = BitConverter.ToSingle(data, offset);
        offset += sizeof(float);
        AMSL = BitConverter.ToSingle(data, offset);
        offset += sizeof(float);
        ISOCountryCode = System.Text.Encoding.ASCII.GetString(data, offset, 3);
        offset += 3;
    }


    /*
     * IReportable implementation
     */
    public string AcceptMediaReport(Media media)
    {
        return media.ReportAirport(this);
    }
}