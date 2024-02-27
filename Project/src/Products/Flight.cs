using DataTransformation;
namespace Products;
public class Flight : IDataTransformable
{
    public static readonly string type = "Flight";
    public string Type { get { return type; } }
    public int ID { get; set; }
    public int Origin { get; set; } // As Airport ID
    public int Target { get; set; } // As Airport ID
    public string TakeoffTime { get; set; }
    public string LandingTime { get; set; }
    public Single Longitude { get; set; }
    public Single Latitude { get; set; }
    public Single AMSL { get; set; }

    public int PlaneID { get; set; }
    public int[] Crew { get; set; } // As their IDs
    public int[] Load { get; set; } // As Cargo IDs

    public void LoadFromFtrString(string[] data)
    {
        ID = int.Parse(data[0]);
        Origin = int.Parse(data[1]);
        Target = int.Parse(data[2]);
        TakeoffTime = data[3];
        LandingTime = data[4];
        Longitude = Single.Parse(data[5]);
        Latitude = Single.Parse(data[6]);
        AMSL = Single.Parse(data[7]);
        PlaneID = int.Parse(data[8]);
        Crew = DataTransformation.Ftr.FtrUtils.ParseArray<int>(data[9]);
        Load = DataTransformation.Ftr.FtrUtils.ParseArray<int>(data[10]);
    }

    public string[] SaveToFtrString()
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
        data[10] = DataTransformation.Ftr.FtrUtils.FormatArray<int>(Crew);
        data[11] = DataTransformation.Ftr.FtrUtils.FormatArray<int>(Load);
        return data;
    }

    public string Serialize(ISerializer serializer)
    {
        return serializer.Serialize<Flight>(this);
    }
}