using DataTransformation;
namespace Products;
public class Airport : IDataTransformable
{
    private static readonly string type = "Airport";
    public string Type { get { return type; } }
    public UInt64 ID { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public Single Longitude { get; set; }
    public Single Latitude { get; set; }
    public Single AMSL { get; set; }
    public string ISOCountryCode { get; set; }

    public void LoadFromFtrString(string[] data)
    {
        ID = UInt64.Parse(data[0]);
        Name = data[1];
        Code = data[2];
        Longitude = Single.Parse(data[3]);
        Latitude = Single.Parse(data[4]);
        AMSL = Single.Parse(data[5]);
        ISOCountryCode = data[6];
    }

    public string[] SaveToFtrString()
    {
        string[] data = new string[8];
        data[0] = type;
        data[1] = ID.ToString();
        data[2] = Name;
        data[3] = Code;
        data[4] = Longitude.ToString();
        data[5] = Latitude.ToString();
        data[6] = AMSL.ToString();
        data[7] = ISOCountryCode;
        return data;
    }

    public string Serialize(ISerializer serializer)
    {
        return serializer.Serialize<Airport>(this);
    }

    public byte[] SaveToByteArray()
    {
        throw new NotImplementedException();
    }

    public void LoadFromByteArray(byte[] data)
    {
        int offset = 0;
        ID = BitConverter.ToUInt64(data, offset); offset += sizeof(UInt64);
        UInt16 nameLength = BitConverter.ToUInt16(data, offset); offset += sizeof(UInt16);
        Name = System.Text.Encoding.ASCII.GetString(data, offset, nameLength); offset += nameLength;
        Code = System.Text.Encoding.ASCII.GetString(data, offset, 3); offset += 3;
        Longitude = BitConverter.ToSingle(data, offset); offset += sizeof(Single);
        Latitude = BitConverter.ToSingle(data, offset); offset += sizeof(Single);
        AMSL = BitConverter.ToSingle(data, offset); offset += sizeof(Single);
        ISOCountryCode = System.Text.Encoding.ASCII.GetString(data, offset, 3); offset += 3;
    }
}