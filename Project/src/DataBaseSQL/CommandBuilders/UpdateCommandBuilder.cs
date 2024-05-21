namespace projob.DataBaseSQL.CommandBuilders;

public class UpdateCommandBuilder : SqlCommandBuilder
{
    public override SqlCommand? Build(string command)
    {
        var parts = command.Split(" ").ToList();

        CommandParserUtils.CheckKeyword(parts, "update");
        var objectClass = CommandParserUtils.ParseUntilKeyword(parts, "set");
        CommandParserUtils.ValidateNotEmpty(objectClass);

        CommandParserUtils.CheckKeyword(parts, "set");
        var keyValuePairs = CommandParserUtils.ParseUntilKeyword(parts, "where");
        CommandParserUtils.ValidateNotEmpty(keyValuePairs);

        if (parts.Count == 0)
        {
            SqlCommand? baseCommand = CommandParserUtils.GenerateSelects(objectClass);
            if (baseCommand == null) return null;
            baseCommand.Append(
                CommandParser.ParseUpdate(keyValuePairs)
                );
            return baseCommand;
        }

        throw new Exception("The case with where keyword is not implemented yet");
    }
}