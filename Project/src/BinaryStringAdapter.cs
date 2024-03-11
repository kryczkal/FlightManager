using System.Text;

namespace DataTransformation;

/// <summary>
/// This class is used to convert a string to a binary string and vice versa
/// Conversion is done 1:1 with no compression meaning 1 byte = 8 bits = 8 characters in the binary string
/// </summary>
public static class BinaryStringAdapter
{
    public static string BinAsString(byte[] byteArray)
    {
        StringBuilder binaryString = new StringBuilder();
        foreach (byte b in byteArray)
        {
            binaryString.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
        }
        return binaryString.ToString();
    }

    public static byte[] StringAsBin(string str)
    {
        return Enumerable.Range(0, str.Length / 8)
                                    .Select(i => Convert.ToByte(str.Substring(i * 8, 8), 2))
                                    .ToArray();;
    }
}