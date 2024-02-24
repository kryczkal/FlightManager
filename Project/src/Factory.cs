using System.Runtime.InteropServices.Marshalling;
using DataTransformation;

namespace Factory
{
    /// <summary>
    /// Represents an abstract factory that creates instances of objects implementing the <see cref="ISerializable"/> interface.
    /// </summary>
    public abstract class AbstractFactory
    {
        private Dictionary<string, Func<ISerializable>> _instances;
        protected AbstractFactory()
        {
            _instances = new Dictionary<string, Func<ISerializable>>()
            {
                {"C", CreateCrew},
                {"P", CreatePassenger},
                {"CA", CreateCargo},
                {"CP", CreateCargoPlane},
                {"PP", CreatePassengerPlane},
                {"AI", CreateAirport},
                {"FL", CreateFlight}
            };
        }
        public ISerializable Create(string type)
        {
            if (!_instances.ContainsKey(type)) throw new ArgumentException("Invalid type");
            return _instances[type]();
        }

        // Create methods for each class
        public abstract ISerializable CreateAirport();
        public abstract ISerializable CreateCargo();
        public abstract ISerializable CreateCargoPlane();
        public abstract ISerializable CreateCrew();
        public abstract ISerializable CreatePassenger();
        public abstract ISerializable CreatePassengerPlane();
        public abstract ISerializable CreateFlight();
    }

    /// <summary>
    /// Represents a base factory that implements the abstract factory pattern.
    /// </summary>
    public class BaseFactory : AbstractFactory
    {
        public override ISerializable CreateAirport()
        {
            return new Airport();
        }
        public override ISerializable CreateCargo()
        {
            return new Cargo();
        }
        public override ISerializable CreateCargoPlane()
        {
            return new CargoPlane();
        }
        public override ISerializable CreateCrew()
        {
            return new Crew();
        }
        public override ISerializable CreatePassenger()
        {
            return new Passenger();
        }
        public override ISerializable CreatePassengerPlane()
        {
            return new PassengerPlane();
        }
        public override ISerializable CreateFlight()
        {
            return new Flight();
        }
    }

}