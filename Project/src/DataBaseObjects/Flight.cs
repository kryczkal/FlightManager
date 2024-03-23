using DataTransformation;
namespace Products;
public class Flight : DataBaseObject
{
    private static readonly string _type = "Flight";
    public string Type => _type;
    public UInt64 ID { get; set; }
    public UInt64 Origin { get; set; } // As Airport ID
    public UInt64 Target { get; set; } // As Airport ID
    public string TakeoffTime { get; set; }
    public string LandingTime { get; set; }
    public Single Longitude { get; set; }
    public Single Latitude { get; set; }
    public Single AMSL { get; set; }
    public UInt64 PlaneID { get; set; }
    public UInt64[] Crew { get; set; } // As their IDs
    public UInt64[] Load { get; set; } // As Cargo IDs

    /*
     * Format Compliancy : FTR, Binary, JSON
     */
    public override void LoadFromFtrString(string[] data)
    {
        ID = UInt64.Parse(data[0]);
        Origin = UInt64.Parse(data[1]);
        Target = UInt64.Parse(data[2]);
        TakeoffTime = data[3];
        LandingTime = data[4];
        Longitude = Single.Parse(data[5]);
        Latitude = Single.Parse(data[6]);
        AMSL = Single.Parse(data[7]);
        PlaneID = UInt64.Parse(data[8]);
        Crew = DataTransformation.Ftr.FtrUtils.ParseArray<UInt64>(data[9]);
        Load = DataTransformation.Ftr.FtrUtils.ParseArray<UInt64>(data[10]);
    }

    public override string[] SaveToFtrString()
    {
        string[] data = new string[12];
        data[0] = Type;
        data[1] = ID.ToString();
        data[2] = Origin.ToString();
        data[3] = Target.ToString();
        data[4] = TakeoffTime;
        data[5] = LandingTime;
        data[6] = Longitude.ToString();
        data[7] = Latitude.ToString();
        data[8] = AMSL.ToString();
        data[9] = PlaneID.ToString();
        data[10] = DataTransformation.Ftr.FtrUtils.FormatArray<UInt64>(Crew);
        data[11] = DataTransformation.Ftr.FtrUtils.FormatArray<UInt64>(Load);
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
        Origin = BitConverter.ToUInt64(data, offset); offset += sizeof(UInt64);
        Target = BitConverter.ToUInt64(data, offset); offset += sizeof(UInt64);
        // TakeoffTime as number of ms since Epoch(UTC)
        TakeoffTime = BitConverter.ToInt64(data, offset).ToString(); offset += sizeof(UInt64);
        // LandingTime as number of ms since Epoch(UTC)
        LandingTime = BitConverter.ToInt64(data, offset).ToString(); offset += sizeof(UInt64);
        PlaneID = BitConverter.ToUInt64(data, offset); offset += sizeof(UInt64);
        UInt16 CrewLength = BitConverter.ToUInt16(data, offset); offset += sizeof(UInt16);
        Crew = new UInt64[CrewLength];
        for (int i = 0; i < CrewLength; i++)
        {
            Crew[i] = BitConverter.ToUInt64(data, offset); offset += sizeof(UInt64);
        }
        UInt16 LoadLength = BitConverter.ToUInt16(data, offset); offset += sizeof(UInt16);
        Load = new UInt64[LoadLength];
        for (int i = 0; i < LoadLength; i++)
        {
            Load[i] = BitConverter.ToUInt64(data, offset); offset += sizeof(UInt64);
        }

        // Longitude, Latitude and AMSL are not used in this method
    }
}