using System;
using System.Diagnostics;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Content.PM;

namespace AndroidBluetoothLE.Bluetooth.Client
{
    public class BluetoothClient
    {
        private static BluetoothClient _instance;
        private BluetoothConnectionHandler _connectionHandler;

        public static BluetoothClient Instance { get { return _instance ?? (_instance = new BluetoothClient()); } }

        public BluetoothManager Manager { get; private set; }

        public BluetoothAdapter Adapter { get; private set; }

        public BluetoothDevice SelectedDevice { get; set; }
        
        public bool IsInitialized { get; private set; }

        public BluetoothConnectionHandler ConnectionHandler
        {
            get { return _connectionHandler ?? (_connectionHandler = new BluetoothConnectionHandler(Manager)); }
        }

        public void Initialize()
        {
            if (IsInitialized) return;

            var context = Application.Context;

            if (!context.PackageManager.HasSystemFeature(PackageManager.FeatureBluetoothLe))
            {
                Debug.WriteLine("Bluetooth LE is not supported");
                return;
            }

            IsInitialized = true;

            var manager = (BluetoothManager)context.GetSystemService(Context.BluetoothService);
            
            Manager = manager;
            Adapter = manager.Adapter;
        }

        public void EnableIfNeeded(BluetoothAdapter adapter)
        {
            if (!adapter.IsEnabled)
            {
                adapter.Enable();
            }
        }

        private BluetoothClient()
        {
        }
    }
}