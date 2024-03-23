using DataTransformation;
using Factory;

namespace Products;
public class DataBaseObjectFactory : Factory<DataBaseObject>
{
    public DataBaseObjectFactory()
    {
        Register("C", () => new Crew());
        Register("CR", () => new Crew());
        Register("P", () => new Passenger());
        Register("PA", () => new Passenger());
        Register("CA", () => new Cargo());
        Register("CP", () => new CargoPlane());
        Register("PP", () => new PassengerPlane());
        Register("AI", () => new Airport());
        Register("FL", () => new Flight());
    }
}