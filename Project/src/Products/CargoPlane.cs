using DataTransformation;
namespace Products;
public class CargoPlane : IDataTransformable
{
    public static readonly string type = "CargoPlane";
    public string Type { get { return type; } }
    public int ID { get; set; }
    public string Serial { get; set; }
    public string Country { get; set; }
    public string Model { get; set; }
    public Single MaxLoad { get; set; }

    public void LoadFromFtrString(string[] data)
    {
        ID = int.Parse(data[0]);
        Serial = data[1];
        Country = data[2];
        Model = data[3];
        MaxLoad = Single.Parse(data[4]);
    }

    public string[] SaveToFtrString()
    {
        string[] data = new string[6];
        data[0] = Type;
        data[1] = ID.ToString();
        data[2] = Serial;
        data[3] = Country;
        data[4] = Model;
        data[5] = MaxLoad.ToString();
        return data;
    }

    public string Serialize(ISerializer serializer)
    {
        return serializer.Serialize<CargoPlane>(this);
    }
}