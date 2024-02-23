using System.Runtime.InteropServices.Marshalling;
using Classes;

namespace Factory
{
    /// <summary>
    /// Represents an abstract factory that creates instances of objects implementing the <see cref="ILoadableFromString"/> interface.
    /// </summary>
    public abstract class AbstractFactory
    {
        private Dictionary<string, Func<ILoadableFromString>> _instances;
        protected AbstractFactory()
        {
            _instances = new Dictionary<string, Func<ILoadableFromString>>()
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
        public ILoadableFromString Create(string type)
        {
            if (!_instances.ContainsKey(type)) throw new ArgumentException("Invalid type");
            return _instances[type]();
        }

        // Create methods for each class
        public abstract ILoadableFromString CreateAirport();
        public abstract ILoadableFromString CreateCargo();
        public abstract ILoadableFromString CreateCargoPlane();
        public abstract ILoadableFromString CreateCrew();
        public abstract ILoadableFromString CreatePassenger();
        public abstract ILoadableFromString CreatePassengerPlane();
        public abstract ILoadableFromString CreateFlight();
    }

    /// <summary>
    /// Represents a base factory that implements the abstract factory pattern.
    /// </summary>
    public class BaseFactory : AbstractFactory
    {
        public override ILoadableFromString CreateAirport()
        {
            return new Airport();
        }
        public override ILoadableFromString CreateCargo()
        {
            return new Cargo();
        }
        public override ILoadableFromString CreateCargoPlane()
        {
            return new CargoPlane();
        }
        public override ILoadableFromString CreateCrew()
        {
            return new Crew();
        }
        public override ILoadableFromString CreatePassenger()
        {
            return new Passenger();
        }
        public override ILoadableFromString CreatePassengerPlane()
        {
            return new PassengerPlane();
        }
        public override ILoadableFromString CreateFlight()
        {
            return new Flight();
        }
    }

}