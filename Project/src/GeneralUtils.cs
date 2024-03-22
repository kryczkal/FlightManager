namespace projob;

public class GeneralUtils
{
    public class AtomicInt
    {
        private int _value = 0;

        public void Set(int value)
        {
            Interlocked.Exchange(ref _value, value);
        }
        public int Value
        {
            get { return Interlocked.CompareExchange(ref _value, 0, 0); }
        }
    }

    public class ConcurrentList<T>
    {
        private List<T> _list = new();
        public void Add(T item)
        {
            lock(_list)
            {
                _list.Add(item);
            }
        }
        public void Remove(T item)
        {
            lock(_list)
            {
                _list.Remove(item);
            }
        }
        public List<T> GetList()
        {
            lock(_list)
            {
                return _list;
            }
        }
    }
}