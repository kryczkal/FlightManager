namespace Classes
{
    public class Crew : ILoadableFromString
    {
        int ID;
        string Name;
        int Age;
        string Phone;
        string Email;
        short Practice;
        string Role;

        void ILoadableFromString.LoadFromString(string[] data)
        {
            ID = int.Parse(data[0]);
            Name = data[1];
            Age = int.Parse(data[2]);
            Phone = data[3];
            Email = data[4];
            Practice = short.Parse(data[5]);
            Role = data[6];
        }
    }
}