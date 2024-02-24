namespace DataTransformation
{
    public class PassengerPlane : ISerializable
    {
        public int ID { get; set; }
        public string Serial { get; set; }
        public string Country { get; set; }
        public string Model { get; set; }
        public short FirstClassSize { get; set; }
        public short BusinessClassSize { get; set; }
        public short EconomyClassSize { get; set; }

        void ILoadableFromString.InitializeFromString(string[] data)
        {
            ID = int.Parse(data[0]);
            Serial = data[1];
            Country = data[2];
            Model = data[3];
            FirstClassSize = short.Parse(data[4]);
            BusinessClassSize = short.Parse(data[5]);
            EconomyClassSize = short.Parse(data[6]);
        }

        string[] IConvertableToString.FormatToString()
        {
            string[] data = new string[7];
            data[0] = ID.ToString();
            data[1] = Serial;
            data[2] = Country;
            data[3] = Model;
            data[4] = FirstClassSize.ToString();
            data[5] = BusinessClassSize.ToString();
            data[6] = EconomyClassSize.ToString();
            return data;
        }
    }
}