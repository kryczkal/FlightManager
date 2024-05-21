namespace projob.DataBaseSQL;

public static class CommandUtils
{
    public static Tuple<string, string>[] KeyValuePairsTupleArray(string keyValuePairs)
    {
        // example of keyValuePairs: "Name=John, Age=25"
        var keyValuePairsArray = keyValuePairs.Split(",");
        // trim each element in array
        for (int i = 0; i < keyValuePairsArray.Length; i++)
        {
            keyValuePairsArray[i] = keyValuePairsArray[i].Trim();
        }
        // convert array to Tuple<string, string>[]
        var keyValuePairsTupleArray = new Tuple<string, string>[keyValuePairsArray.Length];
        for (int i = 0; i < keyValuePairsArray.Length; i++)
        {
            var keyVal = keyValuePairsArray[i].Split("=");
            keyValuePairsTupleArray[i] = new Tuple<string, string>(keyVal[0], keyVal[1]);
        }

        return keyValuePairsTupleArray;
    }

    public static Tuple<string, string>[] KeyValuePairsTupleArray(string[] keyValuePairs)
    {
        Tuple<string, string> [] keyValuePairsTupleArray = new Tuple<string, string>[keyValuePairs.Length];
        for (int i = 0; i < keyValuePairs.Length; i++)
        {
            var keyVal = keyValuePairs[i].Split("=");
            keyValuePairsTupleArray[i] = new Tuple<string, string>(keyVal[0], keyVal[1]);
        }

        return keyValuePairsTupleArray;
    }
}