using DataTransformation;
namespace Products;
public class PassengerPlane : IDataTransformable
{
    public static readonly string type = "PassengerPlane";
    public string Type { get { return type; } }
    public int ID { get; set; }
    public string Serial { get; set; }
    public string Country { get; set; }
    public string Model { get; set; }
    public short FirstClassSize { get; set; }
    public short BusinessClassSize { get; set; }
    public short EconomyClassSize { get; set; }
    public void LoadFromFtrString(string[] data)
    {
        ID = int.Parse(data[0]);
        Serial = data[1];
        Country = data[2];
        Model = data[3];
        FirstClassSize = short.Parse(data[4]);
        BusinessClassSize = short.Parse(data[5]);
        EconomyClassSize = short.Parse(data[6]);
    }

    public string[] SaveToFtrString()
    {
        string[] data = new string[8];
        data[0] = Type;
        data[1] = ID.ToString();
        data[2] = Serial;
        data[3] = Country;
        data[4] = Model;
        data[5] = FirstClassSize.ToString();
        data[6] = BusinessClassSize.ToString();
        data[7] = EconomyClassSize.ToString();
        return data;
    }
    public string Serialize(ISerializer serializer)
    {
        return serializer.Serialize<PassengerPlane>(this);
    }
}