namespace Classes
{
    public class CargoPlane : ILoadableFromString
    {
        int ID;
        string Serial;
        string Country;
        string Model;
        Single MaxLoad;

        void ILoadableFromString.LoadFromString(string[] data)
        {
            ID = int.Parse(data[0]);
            Serial = data[1];
            Country = data[2];
            Model = data[3];
            MaxLoad = Single.Parse(data[4]);
        }
    }
}