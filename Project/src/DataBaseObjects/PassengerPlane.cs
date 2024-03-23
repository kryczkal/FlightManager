using DataTransformation;
using projob;

namespace Products;
public class PassengerPlane : DataBaseObject
{
    private static readonly string _type = "PassengerPlane";
    public string Type => _type;
    public UInt64 ID { get; set; }
    public string Serial { get; set; }
    public string ISOCountryCode { get; set; }
    public string Model { get; set; }
    public UInt16 FirstClassSize { get; set; }
    public UInt16 BusinessClassSize { get; set; }
    public UInt16 EconomyClassSize { get; set; }

    /*
     * Central database functions
     */
    public override void AddToCentral()
    {
        if (!ObjectCentral.PassengerPlanes.TryAdd(ID, this)) throw new InvalidOperationException("PassengerPlane with the same ID already exists.");
    }

    /*
     * Format Compliancy : FTR, Binary, JSON
     */
    public override void LoadFromFtrString(string[] data)
    {
        ID = UInt64.Parse(data[0]);
        Serial = data[1];
        ISOCountryCode = data[2];
        Model = data[3];
        FirstClassSize = UInt16.Parse(data[4]);
        BusinessClassSize = UInt16.Parse(data[5]);
        EconomyClassSize = UInt16.Parse(data[6]);
    }

    public override string[] SaveToFtrString()
    {
        string[] data = new string[8];
        data[0] = Type;
        data[1] = ID.ToString();
        data[2] = Serial;
        data[3] = ISOCountryCode;
        data[4] = Model;
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
        int offset = 0;
        ID = BitConverter.ToUInt64(data, offset); offset += sizeof(UInt64);
        Serial = System.Text.Encoding.ASCII.GetString(data, offset, 10); offset += 10;
        ISOCountryCode = System.Text.Encoding.ASCII.GetString(data, offset, 3); offset += 3;
        UInt16 ModelLength = BitConverter.ToUInt16(data, offset); offset += sizeof(UInt16);
        Model = System.Text.Encoding.ASCII.GetString(data, offset, ModelLength); offset += ModelLength;
        FirstClassSize = BitConverter.ToUInt16(data, offset); offset += sizeof(UInt16);
        BusinessClassSize = BitConverter.ToUInt16(data, offset); offset += sizeof(UInt16);
        EconomyClassSize = BitConverter.ToUInt16(data, offset); offset += sizeof(UInt16);
    }
}