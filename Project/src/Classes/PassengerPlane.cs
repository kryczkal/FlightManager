namespace Classes
{
    public class PassengerPlane : ILoadableFromString
    {
        int ID;
        string Serial;
        string Country;
        string Model;
        short FirstClassSize;
        short BusinessClassSize;
        short EconomyClassSize;

        void ILoadableFromString.LoadFromString(string[] data)
        {
            ID = int.Parse(data[0]);
            Serial = data[1];
            Country = data[2];
            Model = data[3];
            FirstClassSize = short.Parse(data[4]);
            BusinessClassSize = short.Parse(data[5]);
            EconomyClassSize = short.Parse(data[6]);
        }
    }
}