using projob.DataBaseSQL.CommandBuilders;

namespace projob.DataBaseSQL;

public class QueryParser
{
    private static Dictionary<string, SqlCommandBuilder> _commandBuilders = new()
    {
        {"display", new DisplayCommandBuilder()},
        {"update", new UpdateCommandBuilder()},
        {"add", new AddCommandBuilder()},
        {"delete", new DeleteCommandBuilder()},
    };

    public static SqlCommand? ParseQuery(string query)
    {
        if (query.Length <= 0)
        {
            Console.WriteLine("Query is empty", LogLevel.Error);
        }

        // Check the command type (first word in the query)
        var parts = query.Split(" ").ToList();
        var commandType = parts[0];

        if (!_commandBuilders.TryGetValue(commandType, out var commandBuilder))
        {
            Console.WriteLine($"Command type {commandType} is not supported", LogLevel.Error);
            return null;
        }

        try
        {
            return commandBuilder.Build(query);
        } catch (Exception e)
        {
            Console.WriteLine($"Failed to parse query: {e.Message}", LogLevel.Error);
            return null;
        }
    }
}