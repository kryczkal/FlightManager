using projob.DataBaseObjects;

namespace projob.DataBaseSQL;

public abstract class SqlCommand
{
    SqlCommand? _nextCommand;
    SqlCommand? _tailCommand;
    public abstract (IEnumerable<DataBaseObject>, string[]) Execute(IEnumerable<DataBaseObject> dataBaseObjects, params string[] args);

    public SqlCommand SetNext(SqlCommand nextCommand)
    {
        _nextCommand = nextCommand;
        _tailCommand = nextCommand;
        return _nextCommand;
    }

    public void Append(SqlCommand nextCommand)
    {
        if (_tailCommand != null)
        {
            _tailCommand.SetNext(nextCommand);
        }
        if (_nextCommand == null)
        {
            _nextCommand = nextCommand;
        }
        _tailCommand = nextCommand;
    }
    public (IEnumerable<DataBaseObject>, string[]) ExecuteNext(IEnumerable<DataBaseObject> dataBaseObjects, params string[] args)
    {
        if (_nextCommand != null)
        {
            return _nextCommand.Execute(dataBaseObjects, args);
        }
        return (dataBaseObjects, []);
    }

    public void ExecuteChain(IEnumerable<DataBaseObject> dataBaseObjects, params string[] args)
    {
        var (newDataBaseObjects, newArgs) = Execute(dataBaseObjects, args);
        ExecuteNext(newDataBaseObjects, newArgs);
    }

    public void ExecuteChain()
    {
        ExecuteChain(new List<DataBaseObject>());
    }
}
