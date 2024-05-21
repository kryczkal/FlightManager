using projob.DataBaseObjects;

namespace projob.DataBaseSQL;

public abstract class FunctionCommand : SqlCommand
{
    protected Func<IEnumerable<DataBaseObject>, string[], IEnumerable<DataBaseObject>>? Function;

    protected FunctionCommand()
    {
    }
    protected FunctionCommand(Func<IEnumerable<DataBaseObject>, string[], IEnumerable<DataBaseObject>> function)
    {
        Function = function;
    }

    public override (IEnumerable<DataBaseObject>, string[]) Execute(IEnumerable<DataBaseObject> dataBaseObjects,
        params string[] args)
    {
        if (Function == null)
        {
            return (dataBaseObjects, args);
        }


        Function(dataBaseObjects, args);
        return (dataBaseObjects, args);
    }
}
