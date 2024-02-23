using System.Runtime.InteropServices.Marshalling;
using Classes;

namespace Factory
{
    public abstract class AbstractFactory
    {
        public ILoadableFromString? Create(string type)
        {
            switch (type)
            {
                case "Airport":
                    return CreateAirport();
                case "Cargo":
                    return CreateCargo();
                case "CargoPlane":
                    return CreateCargoPlane();
                case "Crew":
                    return CreateCrew();
                case "Passenger":
                    return CreatePassenger();
                case "PassengerPlane":
                    return CreatePassengerPlane();
                default:
                    return null;
            }
        }

        // Create methods for each class
        public abstract ILoadableFromString CreateAirport();
        public abstract ILoadableFromString CreateCargo();
        public abstract ILoadableFromString CreateCargoPlane();
        public abstract ILoadableFromString CreateCrew();
        public abstract ILoadableFromString CreatePassenger();
        public abstract ILoadableFromString CreatePassengerPlane();
    }

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
    }

}