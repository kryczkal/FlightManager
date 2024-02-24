namespace DataTransformation
{
    public class Cargo : ISerializable
    {
        public int ID { get; set; }
        public Single Weight { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }

        void ILoadableFromString.InitializeFromString(string[] data)
        {
            ID = int.Parse(data[0]);
            Weight = Single.Parse(data[1]);
            Code = data[2];
            Description = data[3];
        }

        string[] IConvertableToString.FormatToString()
        {
            string[] data = new string[4];
            data[0] = ID.ToString();
            data[1] = Weight.ToString();
            data[2] = Code;
            data[3] = Description;
            return data;
        }
    }
}