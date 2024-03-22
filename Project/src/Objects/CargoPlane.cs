using DataTransformation;
namespace Products;
public class CargoPlane : IDataTransformable
{
    public static readonly string type = "CargoPlane";
    public string Type { get { return type; } }
    public UInt64 ID { get; set; }
    public string Serial { get; set; }
    public string ISOCountryCode { get; set; }
    public string Model { get; set; }
    public Single MaxLoad { get; set; }

    public void LoadFromFtrString(string[] data)
    {
        ID = UInt64.Parse(data[0]);
        Serial = data[1];
        ISOCountryCode = data[2];
        Model = data[3];
        MaxLoad = Single.Parse(data[4]);
    }

    public string[] SaveToFtrString()
    {
        string[] data = new string[6];
        data[0] = Type;
        data[1] = ID.ToString();
        data[2] = Serial;
        data[3] = ISOCountryCode;
        data[4] = Model;
        data[5] = MaxLoad.ToString();
        return data;
    }

    public string Serialize(ISerializer serializer)
    {
        return serializer.Serialize<CargoPlane>(this);
    }

    public byte[] SaveToByteArray()
    {
        throw new NotImplementedException();
    }

    public void LoadFromByteArray(byte[] data)
    {
        int offset = 0;
        ID = BitConverter.ToUInt64(data, offset); offset += sizeof(UInt64);
        Serial = System.Text.Encoding.ASCII.GetString(data, offset, 10); offset += 10;
        ISOCountryCode = System.Text.Encoding.ASCII.GetString(data, offset, 3); offset += 3;
        UInt16 ModelLength = BitConverter.ToUInt16(data, offset); offset += sizeof(UInt16);
        Model = System.Text.Encoding.ASCII.GetString(data, offset, ModelLength); offset += ModelLength;
        MaxLoad = BitConverter.ToSingle(data, offset); offset += sizeof(Single);
    }
}