namespace projob.DataBaseSQL;

public class SqlAccessor
{
    private readonly Func<string> _getter;
    private readonly Action<string> _setter;
    private readonly Func<string, string, bool> _comparer;
    public SqlAccessor(
        Func<string> getter,
        Action<string> setter)
    {
        _getter = getter;
        _setter = setter;
    }

    public string Get()
    {
        try { return _getter() != null ? _getter() : "Null"; }
        catch { return "Error"; }
    }

    public void Set(string val)
    {
        try
        {
            _setter(val);
        }
        catch
        {
            throw new Exception("Error setting value.");
        }
    }

    public bool Compare(string val1, string val2) => _comparer(val1, val2);
}