using DataTransformation.StringFormatter;

namespace DataTransformation
{
    public static class Serializer
    {
        public static void Serialize(string path, IFormatter formatter, IEnumerable<IConvertableToString> obj)
        {
            string file_extension = formatter.GetFileExtension();

            // Create the directory if it doesn't exist
            Directory.CreateDirectory(Directory.GetParent(path)!.FullName); // TODO: Throws if the directory is null

            // Save the data to the file
            string data_to_save = "";
            foreach (IConvertableToString instance in obj)
            {
                data_to_save += formatter.Format(instance) + "\n";
            }
            File.WriteAllText(path + file_extension, data_to_save);
        }
    }

}