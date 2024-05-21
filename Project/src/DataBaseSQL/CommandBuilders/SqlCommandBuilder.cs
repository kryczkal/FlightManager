namespace projob.DataBaseSQL;

public abstract class SqlCommandBuilder
{
    public abstract SqlCommand? Build(string command);
}