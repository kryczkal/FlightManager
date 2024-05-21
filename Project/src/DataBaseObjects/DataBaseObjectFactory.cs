using DataTransformation;
using Factory;
using projob.DataBaseObjects;

namespace Products;

public class DataBaseObjectFactory : Factory<DataBaseObject>
{
    public DataBaseObjectFactory()
    {
        Register("C", () => new Crew());
        Register("CR", () => new Crew());
        Register("Crew", () => new Crew());

        Register("P", () => new Passenger());
        Register("PA", () => new Passenger());
        Register("Passenger", () => new Passenger());

        Register("CA", () => new Cargo());
        Register("Cargo", () => new Cargo());

        Register("CP", () => new CargoPlane());
        Register("CargoPlane", () => new CargoPlane());

        Register("PP", () => new PassengerPlane());
        Register("PassengerPlane", () => new PassengerPlane());

        Register("AI", () => new Airport());
        Register("Airport", () => new Airport());

        Register("FL", () => new Flight());
        Register("Flight", () => new Flight());
    }
}