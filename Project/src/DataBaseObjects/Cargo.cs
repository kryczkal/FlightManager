using DataTransformation;
using projob;

namespace Products;

public class Cargo : DataBaseObject
{
    private static readonly string _type = "Cargo";
    public virtual string Type => _type;
    public ulong ID { get; set; }
    public float Weight { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }

    /*
     * Central database functions
     */
    public override void AddToCentral()
    {
        if (!DataBaseManager.Cargos.TryAdd(ID, this))
            throw new InvalidOperationException("Cargo with the same ID already exists.");
    }

    /*
     * Format Compliancy : FTR, Binary, JSON
     */
    public override void LoadFromFtrString(string[] data)
    {
        ID = ulong.Parse(data[0]);
        Weight = float.Parse(data[1]);
        Code = data[2];
        Description = data[3];
    }

    public override string[] SaveToFtrString()
    {
        string[] data = new string[5];
        data[0] = Type;
        data[1] = ID.ToString();
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
        ID = BitConverter.ToUInt64(data, offset);
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