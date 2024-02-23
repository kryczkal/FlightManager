using System.ComponentModel;

namespace FileParser
{
    /// <summary>
    /// Represents an interface for parsing files.
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Parses the specified file and returns the parsed data as an enumerable of string arrays.
        /// </summary>
        /// <param name="path">The path of the file to parse.</param>
        /// <returns>An enumerable of string arrays representing the parsed data.</returns>
        IEnumerable<string[]> ParseFile(string path);
    }

    public class FTRParser : IParser
    {
        /// <summary>
        /// Parses a file and returns an enumerable collection of string arrays.
        /// Each string array represents a line in the file, with each element being a comma-separated value.
        /// </summary>
        /// <param name="filePath">The path of the file to parse.</param>
        /// <returns>An enumerable collection of string arrays representing the lines in the file.</returns>
        public IEnumerable<string[]> ParseFile(string filePath)
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