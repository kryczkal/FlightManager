namespace Classes
{
    public class Airport : ILoadableFromString
    {
        int ID;
        string Name;
        string Code;
        Single Longitude;
        Single Latitude;
        Single AMSL;
        string Country;

        void ILoadableFromString.LoadFromString(string[] data)
        {
            ID = int.Parse(data[0]);
            Name = data[1];
            Code = data[2];
            Longitude = Single.Parse(data[3]);
            Latitude = Single.Parse(data[4]);
            AMSL = Single.Parse(data[5]);
            Country = data[6];
        }
    }
}