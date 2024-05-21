namespace projob.DataBaseSQL.CommandBuilders;

public class DeleteCommandBuilder : SqlCommandBuilder
{
    public override SqlCommand? Build(string command)
    {
        var parts = command.Split(" ").ToList();

        CommandParserUtils.CheckKeyword(parts, "delete");
        var objectClass = CommandParserUtils.ParseUntilKeyword(parts, "where");
        CommandParserUtils.ValidateNotEmpty(objectClass);

        if (parts.Count == 0)
        {
            SqlCommand? baseCommand = CommandParserUtils.GenerateSelects(objectClass);
            if (baseCommand == null) return null;
            baseCommand.Append(
                CommandParser.ParseDelete("")
                );
            return baseCommand;
        }

        throw new Exception("The case with where keyword is not implemented yet");

        CommandParserUtils.CheckKeyword(parts, "where");
        var conditions = CommandParserUtils.ParseUntilKeyword(parts, CommandParserUtils.ParseUntilEndToken);
        CommandParserUtils.ValidateNotEmpty(conditions);
    }
}