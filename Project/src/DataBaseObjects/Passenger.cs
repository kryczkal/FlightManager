using DataTransformation;
using projob;

namespace Products;

public class Passenger : DataBaseObject
{
    private static readonly string _type = "Passenger";
    public string Type => _type;
    public ulong ID { get; set; }
    public string Name { get; set; }
    public ulong Age { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Class { get; set; }
    public ulong Miles { get; set; }

    /*
     * Central database functions
     */
    public override void AddToCentral()
    {
        if (!DataBaseManager.Passengers.TryAdd(ID, this))
            throw new InvalidOperationException("Passenger with the same ID already exists.");
    }

    /*
     * Format Compliancy : FTR, Binary, JSON
     */
    public override void LoadFromFtrString(string[] data)
    {
        ID = ulong.Parse(data[0]);
        Name = data[1];
        Age = ulong.Parse(data[2]);
        Phone = data[3];
        Email = data[4];
        Class = data[5];
        Miles = ulong.Parse(data[6]);
    }

    public override string[] SaveToFtrString()
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

    public override byte[] SaveToByteArray()
    {
        throw new NotImplementedException();
    }

    public override void LoadFromByteArray(byte[] data)
    {
        var offset = 0;
        ID = BitConverter.ToUInt64(data, offset);
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
        Class = System.Text.Encoding.ASCII.GetString(data, offset, 1);
        offset += 1;
        Miles = BitConverter.ToUInt64(data, offset);
        offset += sizeof(ulong);
    }
}