using DataTransformation;
using DataTransformation.Binary;
using NetworkSourceSimulator;
using Products;
using projob.DataBaseObjects;

namespace projob;

/// <summary>
/// Manages the simulation of network sources. It can run the simulation in a separate thread and flush the data to the ObjectCentral.
/// </summary>
public class NetworkSourceManager
{
    private GeneralUtils.AtomicInt _atomicLastMessageIndex = new();
    private GeneralUtils.AtomicInt _sharedIndexTracker = new();
    private IDeserializer? _deserializer;
    private NetworkSourceSimulator.NetworkSourceSimulator _networkSource;

    private bool _autoFlush;

    /// <summary>
    /// Gets or sets a value indicating whether the manager should automatically flush the data to the Object Central.
    /// </summary>
    /// <value>
    ///   <c>true</c> if the manager should automatically flush the data; otherwise, <c>false</c>.
    /// </value>
    /// <remarks>
    /// When set to <c>true</c>, the manager will automatically flush the data to the Object Central whenever new data is ready.
    /// When set to <c>false</c>, the manager will stop automatically flushing the data.
    /// </remarks>
    public bool AutoFlush
    {
        set
        {
            // If the value is the same as the current setting, no action is taken.
            if (_autoFlush == value) return;

            // Update the _autoFlush field with the new value.
            _autoFlush = value;

            // If _autoFlush is true, subscribe the FlushMessageToCentral method to the OnNewDataReady event.
            // This means that whenever new data is ready, it will be automatically flushed to the Object Central.
            if (_autoFlush) _networkSource.OnNewDataReady += FlushMessageToCentral;

            // If _autoFlush is false, unsubscribe the FlushMessageToCentral method from the OnNewDataReady event.
            // This means that new data will not be automatically flushed to the Object Central.
            else _networkSource.OnNewDataReady -= FlushMessageToCentral;
        }
    }

    /// <summary>
    /// Initializes a new instance of the NetworkSourceManager class.
    /// </summary>
    /// <param name="networkSource">The network source to be managed.</param>
    /// <param name="autoFlush">Indicates whether the manager should automatically flush the data to the Object Central</param>
    public NetworkSourceManager(NetworkSourceSimulator.NetworkSourceSimulator networkSource, bool autoFlush = false)
    {
        _atomicLastMessageIndex.Set(0);
        _sharedIndexTracker.Set(0);

        _networkSource = networkSource;
        AutoFlush = autoFlush;

        // All NetworkSources give data in binary format
        _deserializer = new DeserializerFactory().CreateProduct("bin");
        if (_deserializer == null) throw new Exception("Deserializer not found");
    }

    /// <summary>
    /// Runs the network source simulation in a separate thread. Can be run only once.
    /// </summary>
    public void RunParallel()
    {
        var simulationThread = new Thread(() => SimulateNetworkSource(_networkSource))
        {
            IsBackground = true
        };
        simulationThread.Start();
    }

    /// <summary>
    /// Simulates the network source and sets up event handling for new data.
    /// </summary>
    /// <param name="networkSource">The network source to be simulated.</param>
    private void SimulateNetworkSource(NetworkSourceSimulator.NetworkSourceSimulator networkSource)
    {
        networkSource.OnNewDataReady += SetLastMessageId;
        networkSource.OnIDUpdate += ChangeId;
        networkSource.OnPositionUpdate += UpdatePosition;
        networkSource.OnContactInfoUpdate += UpdateContactInfo;
        networkSource.Run();
    }

    /// <summary>
    /// Event handler for the OnNewDataReady event. Sets the last message ID.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="newDataReadyArgs">The event arguments.</param>
    private void SetLastMessageId(object sender, NewDataReadyArgs newDataReadyArgs)
    {
        GlobalLogger.Log($"New data is ready at {newDataReadyArgs.MessageIndex}", LogLevel.Debug);
        _atomicLastMessageIndex.Set(newDataReadyArgs.MessageIndex);
    }
    /// <summary>
    /// Changes the ID of an object in the database and updates the references.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="idUpdateArgs"></param>
    /// <exception cref="Exception"></exception>
    private void ChangeId(object sender, IDUpdateArgs idUpdateArgs)
    {
        DataBaseManager.UpdateId(idUpdateArgs.ObjectID, idUpdateArgs.NewObjectID);
    }
    /// <summary>
    /// Updates the position of the flight object in the database.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="positionUpdateArgs"></param>
    /// <exception cref="Exception"></exception>
    private void UpdatePosition(object sender, PositionUpdateArgs positionUpdateArgs)
    {
        // Get the flight object from the database
        Flight? flight;
        if (!DataBaseManager.Flights.TryGetValue(positionUpdateArgs.ObjectID, out flight))
        {
            GlobalLogger.Log($"Flight with ID {positionUpdateArgs.ObjectID} does not exist in the database.", LogLevel.Error);
            return;
        }
        // Update the position of the flight
        GlobalLogger.Log($"Updating position of flight {flight.Id} to {positionUpdateArgs.Latitude}, {positionUpdateArgs.Longitude}", LogLevel.Info);

        //flight.UpdatePosition();
    }
    /// <summary>
    /// Get the crew object from the database and update the contact info.
    /// </summary>
    /// <param name="sender"> sender </param>
    /// <param name="contactInfoUpdateArgs"> class containing the object id, new phone number and new email</param>
    /// <exception cref="Exception"></exception>
    private void UpdateContactInfo(object sender, ContactInfoUpdateArgs contactInfoUpdateArgs)
    {
        // Get the flight object from the database
        Crew? crew;
        if (!DataBaseManager.Crews.TryGetValue(contactInfoUpdateArgs.ObjectID, out crew))
        {
            GlobalLogger.Log($"Crew with ID {contactInfoUpdateArgs.ObjectID} does not exist in the database.", LogLevel.Error);
            return;
        }
        // Update the contact info of the crew
        crew.Phone = contactInfoUpdateArgs.PhoneNumber;
        crew.Email = contactInfoUpdateArgs.EmailAddress;

        GlobalLogger.Log($"Updating contact info of crew {crew.Id} to {contactInfoUpdateArgs.PhoneNumber}, {contactInfoUpdateArgs.EmailAddress}", LogLevel.Info);
    }

    private void FlushMessageToCentral(object sender, NewDataReadyArgs newDataReadyArgs)
    {
        var instance = _deserializer!.Deserialize(
            BinaryStringAdapter.BinAsString(_networkSource.GetMessageAt(newDataReadyArgs.MessageIndex).MessageBytes));
        if (instance == null) throw new Exception("Instance is null");
        instance.AcceptAddToCentral();
    }

    /// <summary>
    /// Flushes the data from the network source to the ObjectCentral.
    /// </summary>
    public void FlushToObjectCentral()
    {
        for (var i = _sharedIndexTracker.Value; i < _atomicLastMessageIndex.Value; i++)
        {
            var instance = _deserializer!.Deserialize(
                BinaryStringAdapter.BinAsString(_networkSource.GetMessageAt(i).MessageBytes));
            if (instance == null) throw new Exception("Instance is null");
            instance.AcceptAddToCentral();
        }

        _sharedIndexTracker.Set(_atomicLastMessageIndex.Value);
    }
}