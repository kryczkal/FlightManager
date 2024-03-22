using DataTransformation;
using NetworkSourceSimulator;

namespace projob
{
    /// <summary>
    /// Manages the simulation of network sources.
    /// </summary>
    public class NetworkSourceManager
    {
        private GeneralUtils.AtomicInt _atomicLastMessageIndex = new();
        private GeneralUtils.AtomicInt _sharedIndexTracker = new();
        private IDeserializer? _deserializer;
        private NetworkSourceSimulator.NetworkSourceSimulator _networkSource;

        /// <summary>
        /// Initializes a new instance of the NetworkSourceManager class.
        /// </summary>
        /// <param name="networkSource">The network source to be managed.</param>
        public NetworkSourceManager(NetworkSourceSimulator.NetworkSourceSimulator networkSource)
        {
            _atomicLastMessageIndex.Set(0);
            _sharedIndexTracker.Set(0);

            _networkSource = networkSource;

            // All NetworkSources give data in binary format
            _deserializer = new DeserializerFactory().CreateProduct("bin");
            if(_deserializer == null) throw new System.Exception("Deserializer not found");
        }

        /// <summary>
        /// Runs the network source simulation in a separate thread. Can be run only once.
        /// </summary>
        public void RunParallel()
        {
            Thread simulationThread = new Thread(() => SimulateNetworkSource(_networkSource))
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
            networkSource.Run();
        }

        /// <summary>
        /// Event handler for the OnNewDataReady event. Sets the last message ID.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="newDataReadyArgs">The event arguments.</param>
        private void SetLastMessageId(object sender, NewDataReadyArgs newDataReadyArgs)
        {
            Console.WriteLine($"New data is ready! {newDataReadyArgs.MessageIndex}");
            _atomicLastMessageIndex.Set(newDataReadyArgs.MessageIndex);
        }

        /// <summary>
        /// Flushes the data from the network source to the ObjectCentral.
        /// </summary>
        public void FlushToObjectCentral()
        {
            for(int i = _sharedIndexTracker.Value; i < _atomicLastMessageIndex.Value; i++)
            {
                IDataTransformable? instance = _deserializer!.Deserialize<IDataTransformable>(
                    BinaryStringAdapter.BinAsString(_networkSource.GetMessageAt(i).MessageBytes));
                if (instance == null) throw new Exception("Instance is null");
                ObjectCentral.Objects.Add(instance);
            }
            _sharedIndexTracker.Set(_atomicLastMessageIndex.Value);
        }
    }
}