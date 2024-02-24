namespace DataTransformation
{
    public class Passenger : ISerializable
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Class { get; set; }
        public int Miles { get; set; }

        void ILoadableFromString.InitializeFromString(string[] data)
        {
            ID = int.Parse(data[0]);
            Name = data[1];
            Age = int.Parse(data[2]);
            Phone = data[3];
            Email = data[4];
            Class = data[5];
            Miles = int.Parse(data[6]);
        }

        string[] IConvertableToString.FormatToString()
        {
            string[] data = new string[7];
            data[0] = ID.ToString();
            data[1] = Name;
            data[2] = Age.ToString();
            data[3] = Phone;
            data[4] = Email;
            data[5] = Class;
            data[6] = Miles.ToString();
            return data;
        }
    }
}