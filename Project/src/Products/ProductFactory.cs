using DataTransformation;
using Factory;

namespace Products;
public class ProductFactory : Factory<IDataTransformable>
{
    public ProductFactory()
    {
        Register("C", () => new Crew());
        Register("P", () => new Passenger());
        Register("CA", () => new Cargo());
        Register("CP", () => new CargoPlane());
        Register("PP", () => new PassengerPlane());
        Register("AI", () => new Airport());
        Register("FL", () => new Flight());
    }
}