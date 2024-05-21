namespace projob.DataBaseSQL;

public interface ISqlAccessable
{
    public Dictionary<string, SqlAccessor> Accessors { get; }
}