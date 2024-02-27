using DataTransformation;
namespace Products;
public class Airport : IDataTransformable
{
    private static readonly string type = "Airport";
    public string Type { get { return type; } }
    public int ID { get; set; }
    public string Name { get; set; }
    public string Code { get; set; }
    public Single Longitude { get; set; }
    public Single Latitude { get; set; }
    public Single AMSL { get; set; }
    public string Country { get; set; }

    public void LoadFromFtrString(string[] data)
    {
        ID = int.Parse(data[0]);
        Name = data[1];
        Code = data[2];
        Longitude = Single.Parse(data[3]);
        Latitude = Single.Parse(data[4]);
        AMSL = Single.Parse(data[5]);
        Country = data[6];
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
        data[7] = Country;
        return data;
    }

    public string Serialize(ISerializer serializer)
    {
        return serializer.Serialize<Airport>(this);
    }

}