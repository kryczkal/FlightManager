namespace DataTransformation
{
    public class CargoPlane : ISerializable
    {
        public int ID { get; set; }
        public string Serial { get; set; }
        public string Country { get; set; }
        public string Model { get; set; }
        public Single MaxLoad { get; set; }

        void ILoadableFromString.InitializeFromString(string[] data)
        {
            ID = int.Parse(data[0]);
            Serial = data[1];
            Country = data[2];
            Model = data[3];
            MaxLoad = Single.Parse(data[4]);
        }

        string[] IConvertableToString.FormatToString()
        {
            string[] data = new string[5];
            data[0] = ID.ToString();
            data[1] = Serial;
            data[2] = Country;
            data[3] = Model;
            data[4] = MaxLoad.ToString();
            return data;
        }
    }
}