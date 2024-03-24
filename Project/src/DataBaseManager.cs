using System.Collections.Concurrent;
using DataTransformation;
using Products;

namespace projob;

public static class DataBaseManager
{
    public static ConcurrentDictionary<ulong, Airport> Airports = new();
    public static ConcurrentDictionary<ulong, Cargo> Cargos = new();
    public static ConcurrentDictionary<ulong, CargoPlane> CargoPlanes = new();
    public static ConcurrentDictionary<ulong, Crew> Crews = new();
    public static ConcurrentDictionary<ulong, Flight> Flights = new();
    public static ConcurrentDictionary<ulong, Passenger> Passengers = new();
    public static ConcurrentDictionary<ulong, PassengerPlane> PassengerPlanes = new();

    public static void MakeSnapshot(ISerializer serializer)
    {
        Console.WriteLine("Printing data...");
        var filePath = "assets/snapshot_" + DateTime.Now.ToString("HH_mm_ss") + "." + serializer.GetFormat();
        List<IDataTransformable> data;

        // Serialize all objects to the file
        data = Airports.Values.ToList<IDataTransformable>();
        DataTransformationUtils.SerializeObjListToFile(data, filePath, serializer);

        data = Cargos.Values.ToList<IDataTransformable>();
        DataTransformationUtils.SerializeObjListToFile(data, filePath, serializer);

        data = CargoPlanes.Values.ToList<IDataTransformable>();
        DataTransformationUtils.SerializeObjListToFile(data, filePath, serializer);

        data = Crews.Values.ToList<IDataTransformable>();
        DataTransformationUtils.SerializeObjListToFile(data, filePath, serializer);

        data = Flights.Values.ToList<IDataTransformable>();
        DataTransformationUtils.SerializeObjListToFile(data, filePath, serializer);

        data = Passengers.Values.ToList<IDataTransformable>();
        DataTransformationUtils.SerializeObjListToFile(data, filePath, serializer);

        data = PassengerPlanes.Values.ToList<IDataTransformable>();
        DataTransformationUtils.SerializeObjListToFile(data, filePath, serializer);
    }
}

public interface IAddableToCentral
{
    public void AddToCentral();
}