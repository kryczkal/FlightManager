using DataTransformation;
using projob;
using projob.media;

namespace Products;

public class Airport : DataBaseObject, IReportable
{
    private static readonly string _type = "Airport";
    public override string Type => _type;
    public ulong ID { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public float Longitude { get; set; }
    public float Latitude { get; set; }
    public float AMSL { get; set; }
    public string ISOCountryCode { get; set; }

    /*
     * Central database functions
     */
    public override void AddToCentral()
    {
        if (!DataBaseManager.Airports.TryAdd(ID, this))
            throw new InvalidOperationException("Airport with the same ID already exists.");
    }

    /*
     * Format Compliancy : FTR, Binary, JSON
     */
    public override void LoadFromFtrString(string[] data)
    {
        ID = ulong.Parse(data[0]);
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
        data[1] = ID.ToString();
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
        ID = BitConverter.ToUInt64(data, offset);
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