namespace Classes
{
    public class Flight : ILoadableFromString
    {
        int ID;
        int Origin; // As Airport ID
        int Target; // As Airport ID
        string TakeoffTime;
        string LandingTime;
        Single Longitude;
        Single Latitude;
        Single AMSL;

        int PlaneID;
        int[] Crew; // As their IDs
        int[] Load; // As Cargo IDs

        void ILoadableFromString.LoadFromString(string[] data)
        {
            ID = int.Parse(data[0]);
            Origin = int.Parse(data[1]);
            Target = int.Parse(data[2]);
            TakeoffTime = data[3];
            LandingTime = data[4];
            Longitude = Single.Parse(data[5]);
            Latitude = Single.Parse(data[6]);
            AMSL = Single.Parse(data[7]);
            PlaneID = int.Parse(data[8]);
            Crew = FileParser.FTRParser.ParseArray<int>(data[9]);
            Load = FileParser.FTRParser.ParseArray<int>(data[10]);
        }
    }
}