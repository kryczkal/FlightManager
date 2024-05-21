using System.Collections;

namespace projob.DataBaseSQL.CommandBuilders;

public static class CommandParserUtils
{
    public static SqlCommand? GenerateSelects(string[] objectClass)
    {
        if (objectClass.Length == 0)
        {
            throw new ArgumentException("objectClass must have at least one element");
        }
        SqlCommand? baseCommand = new SelectCommand(objectClass[0]);
        for(int i = 1; i < objectClass.Length; i++)
        {
            baseCommand.Append(
                new SelectCommand(objectClass[i])
            );
        }
        return baseCommand;
    }

    /// <summary>
    /// Split the objectClass by comma, trim the whitespace and remove empty strings, and save them in an array
    /// </summary>
    /// <param name="objectClass"></param>
    /// <returns></returns>
    public static string[] CleanupArgumentsStrArray(string[] objectClass)
    {
        // remove '(' and ')' from each element
        for (int i = 0; i < objectClass.Length; i++)
        {
            objectClass[i] = objectClass[i].Trim('(', ')');
        }

        objectClass = objectClass.SelectMany
            (field => field
                .Split(","))
            .Select(field => field.Trim())
            .Where(field => field != "")
            .ToArray();

        return objectClass;
    }

    public static string[] ParseUntilKeyword(List<string> parts, string keyword)
    {
        var result = parts.TakeWhile(part => part != keyword).ToArray();
        parts.RemoveRange(0, result.Length);
        return CleanupArgumentsStrArray(result);
    }

    public static bool CheckKeyword(List<string> parts, string keyword)
    {
        if (parts[0] != keyword)
        {
            throw new Exception($"The command does not contain the '{keyword}' keyword");
        }
        ((IList)parts).RemoveAt(0);
        return true;
    }

    public static bool ValidateNotEmpty(string[] array)
    {
        if (array.Length == 0)
        {
            throw new Exception("The array is empty");
        }
        return true;
    }

    public static string ParseUntilEndToken = "!@#$%^&*()_+{}|:\"<>?[]\\;',./`~";
}