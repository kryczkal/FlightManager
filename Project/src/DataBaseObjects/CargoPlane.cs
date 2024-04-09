using DataTransformation;
using projob;
using projob.media;

namespace Products;

public class CargoPlane : DataBaseObject, IReportable
{
    private static readonly string _type = "CargoPlane";
    public override string Type => _type;
    public ulong ID { get; set; }
    public string Serial { get; set; }
    public string ISOCountryCode { get; set; }
    public string Name { get; set; }
    public float MaxLoad { get; set; }

    /*
     * Central database functions
     */
    public override void AddToCentral()
    {
        if (!DataBaseManager.CargoPlanes.TryAdd(ID, this))
            throw new InvalidOperationException("CargoPlane with the same ID already exists.");
    }

    /*
     * Format Compliancy : FTR, Binary, JSON
     */
    public override void LoadFromFtrString(string[] data)
    {
        ID = ulong.Parse(data[0]);
        Serial = data[1];
        ISOCountryCode = data[2];
        Name = data[3];
        MaxLoad = float.Parse(data[4]);
    }

    public override string[] SaveToFtrString()
    {
        string[] data = new string[6];
        data[0] = Type;
        data[1] = ID.ToString();
        data[2] = Serial;
        data[3] = ISOCountryCode;
        data[4] = Name;
        data[5] = MaxLoad.ToString();
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
        Serial = System.Text.Encoding.ASCII.GetString(data, offset, 10);
        offset += 10;
        ISOCountryCode = System.Text.Encoding.ASCII.GetString(data, offset, 3);
        offset += 3;
        var ModelLength = BitConverter.ToUInt16(data, offset);
        offset += sizeof(ushort);
        Name = System.Text.Encoding.ASCII.GetString(data, offset, ModelLength);
        offset += ModelLength;
        MaxLoad = BitConverter.ToSingle(data, offset);
        offset += sizeof(float);
    }

    /*
     * IReportable interface implementation
     */
    public string AcceptMediaReport(Media media)
    {
        return media.ReportCargoPlane(this);
    }
}