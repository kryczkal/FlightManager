namespace projob.DataBaseSQL;

public static class CommandParser
{
    public static UpdateCommand ParseUpdate(string[] keyValuePairs)
    {
        var keyValuePairsTupleArray = CommandUtils.KeyValuePairsTupleArray(keyValuePairs);
        return new UpdateCommand(keyValuePairsTupleArray);
    }

    public static UpdateCommand ParseUpdate(string command)
    {
        var keyValuePairsTupleArray = CommandUtils.KeyValuePairsTupleArray(command);
        return new UpdateCommand(keyValuePairsTupleArray);
    }
    
    public static SelectCommand ParseSelect(string objectClass)
    {
        return new SelectCommand(objectClass);
    }
    

    public static FilterCommand ParseFilter(string command)
    {
        throw new NotImplementedException();
    }
    public static DisplayCommand ParseDisplay(string objectFields)
    {
        return new DisplayCommand(objectFields.Split(',').ToList());
    }
    public static DisplayCommand ParseDisplay(string[] objectFields)
    {
        return new DisplayCommand(objectFields.ToList());
    }
    public static DeleteCommand ParseDelete(string command)
    {
        return new DeleteCommand();
    }
    public static AddCommand ParseAdd(string keyValuePairs)
    {
        var keyValuePairsTupleArray = CommandUtils.KeyValuePairsTupleArray(keyValuePairs);
        return new AddCommand(keyValuePairsTupleArray);
    }
    public static AddCommand ParseAdd(string[] keyValuePairs)
    {
        var keyValuePairsTupleArray = CommandUtils.KeyValuePairsTupleArray(keyValuePairs);
        return new AddCommand(keyValuePairsTupleArray);
    }
}