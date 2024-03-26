using System.Numerics;
using DataTransformation;
using Products;

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

        public int Value => Interlocked.CompareExchange(ref _value, 0, 0);
    }

    public class ConcurrentList<T>
    {
        private List<T> _list = new();

        public void Add(T item)
        {
            lock (_list)
            {
                _list.Add(item);
            }
        }

        public void Remove(T item)
        {
            lock (_list)
            {
                _list.Remove(item);
            }
        }

        public List<T> GetList()
        {
            lock (_list)
            {
                return _list;
            }
        }
    }
}

public static class QuaternionHelper
{
    public static QuaternionDouble LonLatToQuaternion(double longitude, double latitude)
    {
        // Convert longitude and latitude to radians
        var lonRad = Math.PI / 180f * longitude;
        var latRad = Math.PI / 180f * latitude;

        // Convert spherical coordinates to a quaternion
        double x = Math.Cos(latRad) * Math.Cos(lonRad);
        double y = Math.Cos(latRad) * Math.Sin(lonRad);
        double z = Math.Sin(latRad);

        return new QuaternionDouble(x, y, z, 0);
    }

    public static (double Longitude, double Latitude) QuaternionToLonLat(QuaternionDouble position)
    {
        // Normalize the quaternion
        position = QuaternionDouble.Normalize(position);

        // Convert quaternion to spherical coordinates
        var latitude = 180f / Math.PI * Math.Asin(position.Z);
        var longitude = 180f / Math.PI * Math.Atan2(position.Y, position.X);

        return (longitude, latitude);
    }
}