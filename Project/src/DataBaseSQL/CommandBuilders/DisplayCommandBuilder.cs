namespace projob.DataBaseSQL.CommandBuilders;

public class DisplayCommandBuilder : SqlCommandBuilder
{
    public override SqlCommand? Build(string command)
    {
        // Split the command into parts by finding keywords: display, from, where
        var parts = command.Split(" ").ToList();
        CommandParserUtils.CheckKeyword(parts,"display");
        var objectFields = CommandParserUtils.ParseUntilKeyword(parts, "from");
        CommandParserUtils.ValidateNotEmpty(objectFields);

        CommandParserUtils.CheckKeyword(parts,"from");
        var objectClass = CommandParserUtils.ParseUntilKeyword(parts, "where");
        CommandParserUtils.ValidateNotEmpty(objectClass);


        // Check if the next keyword is where or the end of the command
        if (parts.Count == 0)
        {
            SqlCommand? baseCommand = CommandParserUtils.GenerateSelects(objectClass);
            if (baseCommand == null) return null;
            baseCommand.Append(
                CommandParser.ParseDisplay(objectFields)
                );
            return baseCommand;
        }

        throw new Exception("The case with where keyword is not implemented yet");

    }
}