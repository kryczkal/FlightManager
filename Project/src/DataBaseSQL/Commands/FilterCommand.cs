using projob.DataBaseObjects;

namespace projob.DataBaseSQL;

public class FilterCommand : SqlCommand
{
    private readonly Func<DataBaseObject, bool> _filter;
    FilterCommand(Func<DataBaseObject, bool> filter)
    {
        _filter = filter;
    }

    public override (IEnumerable<DataBaseObject>, string[]) Execute(IEnumerable<DataBaseObject> dataBaseObjects,
        params string[] args)
    {
        return (dataBaseObjects.Where(_filter), args);
    }

}

public class ComparasionEntity
{
    public int X { get; set; }
    public int Y { get; set; }
}