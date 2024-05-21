using projob.DataBaseObjects;

namespace projob.DataBaseSQL;

public class SelectCommand : SqlCommand
{
    private IEnumerable<DataBaseObject> _selectedObjects;
    private string _selectedObjectString = "";
    public SelectCommand(string objectClass)
    {
        switch (objectClass)
        {
            case "Airport":
                _selectedObjects = DataBaseManager.Airports.Values;
                _selectedObjectString = "Airport";
                break;
            case "Cargo":
                _selectedObjects = DataBaseManager.Cargos.Values;
                _selectedObjectString = "Cargo";
                break;
            case "CargoPlane":
                _selectedObjects = DataBaseManager.CargoPlanes.Values;
                _selectedObjectString = "CargoPlane";
                break;
            case "Crew":
                _selectedObjects = DataBaseManager.Crews.Values;
                _selectedObjectString = "Crew";
                break;
            case "Flight":
                _selectedObjects = DataBaseManager.Flights.Values;
                _selectedObjectString = "Flight";
                break;
            case "Passenger":
                _selectedObjects = DataBaseManager.Passengers.Values;
                _selectedObjectString = "Passenger";
                break;
            case "PassengerPlane":
                _selectedObjects = DataBaseManager.PassengerPlanes.Values;
                _selectedObjectString = "PassengerPlane";
                break;
            default:
                throw new Exception($"Unknown object class '{objectClass}'");
                break;
        }
    }

    public override (IEnumerable<DataBaseObject>, string[]) Execute(IEnumerable<DataBaseObject> dataBaseObjects,
        params string[] args)
    {
        return (dataBaseObjects.Concat(_selectedObjects), args.Append(_selectedObjectString).ToArray());
    }

}
