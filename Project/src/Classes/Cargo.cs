namespace Classes
{
    public class Cargo : ILoadableFromString
    {
        int ID;
        Single Weight;
        string Code;
        string Description;

        void ILoadableFromString.LoadFromString(string[] data)
        {
            ID = int.Parse(data[0]);
            Weight = Single.Parse(data[1]);
            Code = data[2];
            Description = data[3];
        }
    }
}