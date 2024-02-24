using System.Runtime.Versioning;
using System.Text.Json;

namespace DataTransformation
{
    namespace StringFormatter
    {
        public interface IFormatter
        {
            string Format(IConvertableToString obj);
            string GetFileExtension();
        }

        public class JsonFormatter : IFormatter
        {
            public string Format(IConvertableToString obj)
            {
                return JsonSerializer.Serialize(obj); // TODO: This is not working
            }

            public string GetFileExtension()
            {
                return ".json";
            }
        }

        public class FTRFormatter : IFormatter
        {
            public string Format(IConvertableToString obj)
            {
                string[] data = obj.FormatToString();
                return string.Join(",", data);
            }

            public string GetFileExtension()
            {
                return ".ftr";
            }

            public static string FormatArray<T>(T[] values)
            {
                string?[] data = new string[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    data[i] = values[i]!.ToString();
                    if (data[i] == null)
                    {
                        throw new System.ArgumentNullException();
                    }
                }
                return "[" + string.Join(";", data) + "]";
            }
        }
    }
}