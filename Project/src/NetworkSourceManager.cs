using DataTransformation;
using DataTransformation.Binary;
using NetworkSourceSimulator;
using Products;

namespace projob
{
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
        private void FlushMessageToCentral(object sender, NewDataReadyArgs newDataReadyArgs)
        {
            DataBaseObject? instance = _deserializer!.Deserialize(
                BinaryStringAdapter.BinAsString(_networkSource.GetMessageAt(newDataReadyArgs.MessageIndex).MessageBytes));
            if (instance == null) throw new Exception("Instance is null");
            instance.AddToCentral();
        }

        /// <summary>
        /// Flushes the data from the network source to the ObjectCentral.
        /// </summary>
        public void FlushToObjectCentral()
        {
            for(int i = _sharedIndexTracker.Value; i < _atomicLastMessageIndex.Value; i++)
            {
                DataBaseObject? instance = _deserializer!.Deserialize(
                    BinaryStringAdapter.BinAsString(_networkSource.GetMessageAt(i).MessageBytes));
                if (instance == null) throw new Exception("Instance is null");
                instance.AddToCentral();
            }
            _sharedIndexTracker.Set(_atomicLastMessageIndex.Value);
        }
    }
}