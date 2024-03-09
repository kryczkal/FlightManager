using System.Text;

namespace projob;

/*
 * This class is used to convert a string to a binary string and vice versa
 * Conversion is done 1:1 with no compression meaning 1 byte = 8 bits = 8 characters in the binary string
 */
public class BinaryStringAdapter()
{
    private readonly string _s_data;
    private readonly byte[] _b_data;

    public BinaryStringAdapter(byte[] data) : this()
    {
        _b_data = data;
    }

    public BinaryStringAdapter(string data) : this()
    {
        _s_data = data;
    }

    public string BinAsString()
    {
        StringBuilder binaryString = new StringBuilder();
        foreach (byte b in _b_data)
        {
            binaryString.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
        }
        return binaryString.ToString();
    }

    public byte[] StringAsBin()
    {
        return Enumerable.Range(0, _s_data.Length / 8)
                                    .Select(i => Convert.ToByte(_s_data.Substring(i * 8, 8), 2))
                                    .ToArray();;
    }
}