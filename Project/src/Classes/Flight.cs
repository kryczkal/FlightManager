using DataTransformation.FileParser;
using DataTransformation.StringFormatter;
namespace DataTransformation
{
    public class Flight : ISerializable
    {
        public int ID { get; set; }
        public int Origin { get; set; } // As Airport ID
        public int Target { get; set; } // As Airport ID
        public string TakeoffTime { get; set; }
        public string LandingTime { get; set; }
        public Single Longitude { get; set; }
        public Single Latitude { get; set; }
        public Single AMSL { get; set; }

        public int PlaneID { get; set; }
        public int[] Crew { get; set; } // As their IDs
        public int[] Load { get; set; } // As Cargo IDs

        void ILoadableFromString.InitializeFromString(string[] data)
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
            Crew = FTRParser.ParseArray<int>(data[9]);
            Load = FTRParser.ParseArray<int>(data[10]);
        }

        string[] IConvertableToString.FormatToString()
        {
            string[] data = new string[11];
            data[0] = ID.ToString();
            data[1] = Origin.ToString();
            data[2] = Target.ToString();
            data[3] = TakeoffTime;
            data[4] = LandingTime;
            data[5] = Longitude.ToString();
            data[6] = Latitude.ToString();
            data[7] = AMSL.ToString();
            data[8] = PlaneID.ToString();
            data[9] = FTRFormatter.FormatArray<int>(Crew);
            data[10] = FTRFormatter.FormatArray<int>(Load);
            return data;
        }
    }
}