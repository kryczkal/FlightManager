namespace projob.DataBaseSQL.CommandBuilders;

public class AddCommandBuilder : SqlCommandBuilder
{
    public override SqlCommand? Build(string command)
    {
        var parts = command.Split(" ").ToList();

        CommandParserUtils.CheckKeyword(parts, "add");
        var objectClass = CommandParserUtils.ParseUntilKeyword(parts, "new");
        CommandParserUtils.ValidateNotEmpty(objectClass);

        CommandParserUtils.CheckKeyword(parts, "new");
        var keyValuePairs = CommandParserUtils.ParseUntilKeyword(parts, CommandParserUtils.ParseUntilEndToken);
        CommandParserUtils.ValidateNotEmpty(keyValuePairs);

        if (parts.Count == 0)
        {
            SqlCommand? baseCommand = CommandParserUtils.GenerateSelects(objectClass);
            if (baseCommand == null) return null;
            baseCommand.Append(
                CommandParser.ParseAdd(keyValuePairs)
                );
            return baseCommand;
        }

        throw new Exception("The case with where keyword is not implemented yet");
    }
}