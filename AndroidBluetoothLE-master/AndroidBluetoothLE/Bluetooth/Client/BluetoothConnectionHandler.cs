using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;

namespace AndroidBluetoothLE.Bluetooth.Client
{
    public class BluetoothConnectionHandler
    {
        private readonly BluetoothManager _manager;
        private TaskCompletionSource<object> _disconnectTaskSource; 
        private Action<ProfileState> _onConnection;
        private Action<GattStatus> _onServiceDiscovery;
        private BluetoothDevice _lastDevice;

        public BluetoothGatt GattValue { get; private set; }
        
        public bool IsConnected
        {
            get
            {
                return _lastDevice != null && GattValue != null &&
                       _manager.GetConnectedDevices(ProfileType.Gatt).Contains(_lastDevice);
            }
        }

        public BluetoothConnectionHandler(BluetoothManager manager)
        {
            _manager = manager;
            var gattClientObserver = GattClientObserver.Instance;
            gattClientObserver.ConnectionStateChanged += OnConnectionStateChanged;
            gattClientObserver.ServicesDiscovered += OnServicesDiscovered;
        }

        public void Connect(BluetoothDevice device, Action<ProfileState> onConnectionChanged)
        {
            _lastDevice = device;
            GattValue = device.ConnectGatt(Application.Context, false, GattClientObserver.Instance);
            _onConnection = onConnectionChanged;
        }

        public Task DisconnectAsync()
        {
            _disconnectTaskSource = new TaskCompletionSource<object>();
            GattValue.Disconnect();
            return _disconnectTaskSource.Task;
        }

        public IList<BluetoothGattService> GetServiceList()
        {
            if (GattValue == null) return new List<BluetoothGattService>();

            return GattValue.Services;
        }

        public void DiscoverServices(Action<GattStatus> onServiceDiscovery)
        {
            _onServiceDiscovery = onServiceDiscovery;
            if (GattValue != null)
            {
                GattValue.DiscoverServices();
            }
            else
            {
                onServiceDiscovery(GattStatus.Failure);
            }
        }

        private void OnConnectionStateChanged(BluetoothGatt gatt, GattStatus status, ProfileState newState)
        {
            switch (newState)
            {
                case ProfileState.Connected:
                    Debug.WriteLine("Connected peripheral: " + gatt.Device.Name);
                    _onConnection(ProfileState.Connected);
                    break;
                case ProfileState.Disconnected:
                    Debug.WriteLine("Disconnected peripheral: " + gatt.Device.Name);
                    OnDisconnection();
                    break;
                case ProfileState.Connecting:
                    Debug.WriteLine("Connecting peripheral: " + gatt.Device.Name);
                    break;
                case ProfileState.Disconnecting:
                    Debug.WriteLine("Disconnecting peripheral: " + gatt.Device.Name);
                    break;
            }
        }

        private void OnDisconnection()
        {
            GattValue.Close();
            GattValue = null;
            if (_disconnectTaskSource != null)
            {
                _disconnectTaskSource.TrySetResult(null);
            }
        }

        private void OnServicesDiscovered(BluetoothGatt gatt, GattStatus status)
        {
            Debug.WriteLine(status != GattStatus.Success
                ? "Failed to discover device services"
                : "Successfully discovered device services");
            _onServiceDiscovery(status);

        }
    }
}