using System.ComponentModel;

namespace FileParser
{
    public abstract class AbstractParser
    {
        public abstract IEnumerable<string[]> ParseFile(string path);
    }

    public class FTRParser : AbstractParser
    {
        public override IEnumerable<string[]> ParseFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException();
            }
            using StreamReader reader = new StreamReader(filePath);
            string? line;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line.Split(',');
            }
        }

        /// <summary>
        /// Parses a string representation of an array in the form [value1;value2;value3;...;value_n] and converts it to an array of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the elements in the array.</typeparam>
        /// <param name="line">The string representation of the array.</param>
        /// <returns>An array of the specified type.</returns>
        public static T[] ParseArray<T>(string line)
        {
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
            line = line.Substring(1, line.Length - 2);
            string[] values = line.Split(';');
            return Array.ConvertAll(values, s => (T)converter.ConvertFromString(s)!)!;
        }
    }
}