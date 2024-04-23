using System.Collections.Concurrent;
using DataTransformation;
using Products;
using projob.DataBaseObjects;
using projob.media;

namespace projob;

public static class DataBaseManager
{
    /*
     * Central database
     */
    public static ConcurrentDictionary<ulong, Airport> Airports = new();
    public static ConcurrentDictionary<ulong, Cargo> Cargos = new();
    public static ConcurrentDictionary<ulong, CargoPlane> CargoPlanes = new();
    public static ConcurrentDictionary<ulong, Crew> Crews = new();
    public static ConcurrentDictionary<ulong, Flight> Flights = new();
    public static ConcurrentDictionary<ulong, Passenger> Passengers = new();
    public static ConcurrentDictionary<ulong, PassengerPlane> PassengerPlanes = new();
    // Reference list
    public static ConcurrentDictionary<ulong, DataBaseConsolidatedRelation> References = new();

    /*
     * Accessors
     */
    public static List<DataBaseObject> GetAllObjects()
    {
        var allObjects = new List<DataBaseObject>();
        allObjects.AddRange(Airports.Values);
        allObjects.AddRange(Cargos.Values);
        allObjects.AddRange(CargoPlanes.Values);
        allObjects.AddRange(Crews.Values);
        allObjects.AddRange(Flights.Values);
        allObjects.AddRange(Passengers.Values);
        allObjects.AddRange(PassengerPlanes.Values);
        return allObjects;
    }
    public static List<IReportable> GetReportableObjects()
    {
        var reportableObjects = new List<IReportable>();
        reportableObjects.AddRange(Airports.Values);
        reportableObjects.AddRange(CargoPlanes.Values);
        reportableObjects.AddRange(PassengerPlanes.Values);
        return reportableObjects;
    }
    public static DataBaseObject? GetById(ulong id)
    {
        if (Airports.ContainsKey(id))
            return Airports[id];
        if (Cargos.ContainsKey(id))
            return Cargos[id];
        if (CargoPlanes.ContainsKey(id))
            return CargoPlanes[id];
        if (Crews.ContainsKey(id))
            return Crews[id];
        if (Flights.ContainsKey(id))
            return Flights[id];
        if (Passengers.ContainsKey(id))
            return Passengers[id];
        if (PassengerPlanes.ContainsKey(id))
            return PassengerPlanes[id];
        GlobalLogger.Log($"Object with ID {id} does not exist in the database.", LogLevel.Error);
        return null;
    }
    public static void UpdateId(ulong oldId, ulong newId)
    {
        if (References.TryGetValue(oldId, out DataBaseConsolidatedRelation? dataBaseConsolidatedRelation))
        {
            dataBaseConsolidatedRelation.BaseObject.ChangeId(newId);
            References.TryRemove(oldId, out _);
            References.TryAdd(newId, dataBaseConsolidatedRelation);
        }
        else
        {
            GlobalLogger.Log($"Object with ID {oldId} does not exist in the database.", LogLevel.Error);
            return;
        }

        if (Airports.TryGetValue(oldId, out Airport? airport))
        {
            Airports.TryRemove(oldId, out _);
            Airports.TryAdd(newId, airport);
            GlobalLogger.Log($"Updated ID of {airport.Type} {oldId} to ID {newId}", LogLevel.Info);
        }else if (Cargos.TryGetValue(oldId, out Cargo? cargo))
        {
            Cargos.TryRemove(oldId, out _);
            Cargos.TryAdd(newId, cargo);
            GlobalLogger.Log($"Updated ID of {cargo.Type} {oldId} to ID {newId}", LogLevel.Info);
        }else if (CargoPlanes.TryGetValue(oldId, out CargoPlane? cargoPlane))
        {
            CargoPlanes.TryRemove(oldId, out _);
            CargoPlanes.TryAdd(newId, cargoPlane);
            GlobalLogger.Log($"Updated ID of {cargoPlane.Type} {oldId} to ID {newId}", LogLevel.Info);
        }else if (Crews.TryGetValue(oldId, out Crew? crew))
        {
            Crews.TryRemove(oldId, out _);
            Crews.TryAdd(newId, crew);
            GlobalLogger.Log($"Updated ID of {crew.Type} {oldId} to ID {newId}", LogLevel.Info);
        }else if (Flights.TryGetValue(oldId, out Flight? flight))
        {
            Flights.TryRemove(oldId, out _);
            Flights.TryAdd(newId, flight);
            GlobalLogger.Log($"Updated ID of {flight.Type} {oldId} to ID {newId}", LogLevel.Info);
        }else if (Passengers.TryGetValue(oldId, out Passenger? passenger))
        {
            Passengers.TryRemove(oldId, out _);
            Passengers.TryAdd(newId, passenger);
            GlobalLogger.Log($"Updated ID of {passenger.Type} {oldId} to ID {newId}", LogLevel.Info);
        }else if (PassengerPlanes.TryGetValue(oldId, out PassengerPlane? passengerPlane))
        {
            PassengerPlanes.TryRemove(oldId, out _);
            PassengerPlanes.TryAdd(newId, passengerPlane);
            GlobalLogger.Log($"Updated ID of {passengerPlane.Type} {oldId} to ID {newId}", LogLevel.Info);
        }
    }

    /*
     * Reference maintenance
     */
    public static void CreateObjReferences()
    {
        // Create references
        foreach (var obj in GetAllObjects())
        {
            if (!References.TryAdd(obj.Id, new DataBaseConsolidatedRelation(obj)))
            {
                throw new InvalidOperationException($"Object with ID {obj.Id} already exists.");
            }
        }
        // Set objects to auto-update references
        foreach (DataBaseConsolidatedRelation dataBaseConsolidatedRelation in References.Values)
        {
            foreach (DataBaseObject dependency in dataBaseConsolidatedRelation.References)
            {
                dependency.OnIdChange += dataBaseConsolidatedRelation.BaseObject.UpdateObjReferences;
            }
        }
    }

    /*
     * Snapshot functions
     */
    public static void MakeSnapshot(ISerializer serializer)
    {
        Console.WriteLine("Printing data...");
        var filePath = "assets/snapshot_" + DateTime.Now.ToString("HH_mm_ss") + "." + serializer.GetFormat();
        DataTransformationUtils.SerializeObjListToFile(GetAllObjects(), filePath, serializer);
    }

    public static void LoadFromFtrFile(string filePath)
    {
        GlobalLogger.Log("Loading data from FTR file...", LogLevel.Debug);
        DeserializerFactory deserializerFactory = new();
        var data = DataTransformationUtils.DeserializeFileToObjList(filePath, deserializerFactory.CreateProduct("ftr")!);
        foreach (var obj in data)
        {
            if (obj == null)
                throw new Exception("Failed to deserialize object from file.");

            obj.AcceptAddToCentral();
            GlobalLogger.Log($"Loaded {obj.Type} with ID {obj.Id}", LogLevel.Debug);
        }
    }
}

public class DataBaseConsolidatedRelation
{
    public DataBaseObject BaseObject;
    public List<DataBaseObject> References;

    public DataBaseConsolidatedRelation(DataBaseObject baseObject)
    {
        BaseObject = baseObject;
        References = new List<DataBaseObject>();
        List<ulong>? referencedIds = BaseObject.GetReferencedIds();
        if (referencedIds != null)
        {
            // Add referenced objects to the list
            foreach (ulong id in referencedIds)
            {
                DataBaseObject? obj = DataBaseManager.GetById(id);
                if (obj != null)
                {
                    References.Add(obj);
                }
                else
                    throw new Exception($"{BaseObject.Type} with ID {BaseObject.Id} references an object with ID {id} that does not exist in the database.");
            }
        }
    }

}

public interface IAddableToCentral
{
    public void AcceptAddToCentral();
}