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

public static class QuaternionHelper
{
    public static Quaternion LonLatToQuaternion(float longitude, float latitude)
    {
        // Convert longitude and latitude to radians
        float lonRad = MathF.PI / 180f * longitude;
        float latRad = MathF.PI / 180f * latitude;

        // Convert spherical coordinates to a quaternion
        float x = MathF.Cos(latRad) * MathF.Cos(lonRad);
        float y = MathF.Cos(latRad) * MathF.Sin(lonRad);
        float z = MathF.Sin(latRad);

        return new Quaternion(x, y, z, 0);
    }

    public static (float Longitude, float Latitude) QuaternionToLonLat(Quaternion position)
    {
        // Normalize the quaternion
        position = Quaternion.Normalize(position);

        // Convert quaternion to spherical coordinates
        float latitude = 180f / MathF.PI * MathF.Asin(position.Z);
        float longitude = 180f / MathF.PI * MathF.Atan2(position.Y, position.X);

        return (longitude, latitude);
    }
}