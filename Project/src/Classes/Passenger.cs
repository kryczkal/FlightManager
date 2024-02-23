namespace Classes
{
    public class Passenger : ILoadableFromString
    {
        int ID;
        string Name;
        int Age;
        string Phone;
        string Email;
        string Class;
        int Miles;

        void ILoadableFromString.LoadFromString(string[] data)
        {
            ID = int.Parse(data[0]);
            Name = data[1];
            Age = int.Parse(data[2]);
            Phone = data[3];
            Email = data[4];
            Class = data[5];
            Miles = int.Parse(data[6]);
        }
    }
}