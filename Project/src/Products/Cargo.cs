using DataTransformation;
namespace Products;

public class Cargo : IDataTransformable
{
    public static readonly string type = "Cargo";
    public string Type { get { return type; } }

    public int ID { get; set; }
    public Single Weight { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }

    public void LoadFromFtrString(string[] data)
    {
        ID = int.Parse(data[0]);
        Weight = Single.Parse(data[1]);
        Code = data[2];
        Description = data[3];
    }

    public string[] SaveToFtrString()
    {
        string[] data = new string[5];
        data[0] = Type;
        data[1] = ID.ToString();
        data[2] = Weight.ToString();
        data[3] = Code;
        data[4] = Description;
        return data;
    }
    public string Serialize(ISerializer serializer)
    {
        return serializer.Serialize<Cargo>(this);
    }
}