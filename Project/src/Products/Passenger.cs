using DataTransformation;
namespace Products;

public class Passenger : IDataTransformable
{
    public static readonly string type = "Passenger";
    public string Type { get { return type; } }
    public UInt64 ID { get; set; }
    public string Name { get; set; }
    public UInt64 Age { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Class { get; set; }
    public UInt64 Miles { get; set; }

    public void LoadFromFtrString(string[] data)
    {
        ID = UInt64.Parse(data[0]);
        Name = data[1];
        Age = UInt64.Parse(data[2]);
        Phone = data[3];
        Email = data[4];
        Class = data[5];
        Miles = UInt64.Parse(data[6]);
    }

    public string[] SaveToFtrString()
    {
        string[] data = new string[7];
        data[0] = Type;
        data[1] = ID.ToString();
        data[2] = Name;
        data[3] = Age.ToString();
        data[4] = Phone;
        data[5] = Email;
        data[6] = Class;
        data[7] = Miles.ToString();
        return data;
    }

    public string Serialize(ISerializer serializer)
    {
        return serializer.Serialize<Passenger>(this);
    }

    public byte[] SaveToByteArray()
    {
        throw new NotImplementedException();
    }

    public void LoadFromByteArray(byte[] data)
    {
        int offset = 0;
        ID = BitConverter.ToUInt64(data, offset); offset += sizeof(UInt64);
        UInt16 NameLength = BitConverter.ToUInt16(data, offset); offset += sizeof(Int16);
        Name = System.Text.Encoding.ASCII.GetString(data, offset, NameLength); offset += NameLength;
        Age = BitConverter.ToUInt16(data, offset); offset += sizeof(Int16);
        Phone = System.Text.Encoding.ASCII.GetString(data, offset, 12); offset += 12;
        UInt16 EmailLength = BitConverter.ToUInt16(data, offset); offset += sizeof(ushort);
        Email = System.Text.Encoding.ASCII.GetString(data, offset, EmailLength); offset += EmailLength;
        Class = System.Text.Encoding.ASCII.GetString(data, offset, 12); offset += 12;
        Miles = BitConverter.ToUInt64(data, offset); offset += sizeof(UInt64);
    }
}
