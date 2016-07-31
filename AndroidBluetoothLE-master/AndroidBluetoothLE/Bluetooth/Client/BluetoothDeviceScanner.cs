using System;
using Android.Bluetooth;

namespace AndroidBluetoothLE.Bluetooth.Client
{
    public class BluetoothDeviceScanner : Java.Lang.Object, BluetoothAdapter.ILeScanCallback
    {
        private readonly Action<BluetoothDevice> _onDiscoveredPeripheral;
        private readonly BluetoothAdapter _adapter;

        public bool IsScanning { get; private set; }

        public BluetoothDeviceScanner(BluetoothAdapter adapter, Action<BluetoothDevice> onDiscoveredPeripheral)
        {
            _adapter = adapter;
            _onDiscoveredPeripheral = onDiscoveredPeripheral;
        }

        public void StartScan()
        {
            IsScanning = true;
            _adapter.StartLeScan(this);
        }

        public void StopScan()
        {
            IsScanning = false;
            _adapter.StopLeScan(this);
        }

        public void OnLeScan(BluetoothDevice device, int rssi, byte[] scanRecord)
        {
            if (_onDiscoveredPeripheral == null) return;
            _onDiscoveredPeripheral(device);
        }
    }
}