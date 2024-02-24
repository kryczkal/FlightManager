namespace DataTransformation
{
    public class Airport : ISerializable
    {
        public int ID { get; set;}
        public string Name { get; set;}
        public string Code { get; set;}
        public Single Longitude { get; set; }
        public Single Latitude { get; set; }
        public Single AMSL { get; set; }
        public string Country { get; set; }

        void ILoadableFromString.InitializeFromString(string[] data)
        {
            ID = int.Parse(data[0]);
            Name = data[1];
            Code = data[2];
            Longitude = Single.Parse(data[3]);
            Latitude = Single.Parse(data[4]);
            AMSL = Single.Parse(data[5]);
            Country = data[6];
        }

        string[] IConvertableToString.FormatToString()
        {
            string[] data = new string[7];
            data[0] = ID.ToString();
            data[1] = Name;
            data[2] = Code;
            data[3] = Longitude.ToString();
            data[4] = Latitude.ToString();
            data[5] = AMSL.ToString();
            data[6] = Country;
            return data;
        }
    }
}