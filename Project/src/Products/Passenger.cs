using DataTransformation;
namespace Products;

public class Passenger : IDataTransformable
{
    public static readonly string type = "Passenger";
    public string Type { get { return type; } }
    public int ID { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public string Class { get; set; }
    public int Miles { get; set; }

    public void LoadFromFtrString(string[] data)
    {
        ID = int.Parse(data[0]);
        Name = data[1];
        Age = int.Parse(data[2]);
        Phone = data[3];
        Email = data[4];
        Class = data[5];
        Miles = int.Parse(data[6]);
    }

    public string[] SaveToFtrString()
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

    public string Serialize(ISerializer serializer)
    {
        return serializer.Serialize<Passenger>(this);
    }
}
