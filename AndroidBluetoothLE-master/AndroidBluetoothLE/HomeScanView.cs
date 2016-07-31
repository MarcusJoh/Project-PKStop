using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Views;
using Android.OS;
using Android.Widget;
using AndroidBluetoothLE.Bluetooth.Client;
using AndroidBluetoothLE.Bluetooth.Server;

namespace AndroidBluetoothLE
{
    [Activity(Label = "BluetoothLE", MainLauncher = true)]
    public class HomeScanView : Activity
    {
        private readonly List<BluetoothDevice> _deviceList;
        private ArrayAdapter<string> _adapter;
        private BluetoothClient _bluetoothClient;
        private BluetoothDeviceScanner _scanner;
        private BluetoothServer _bluetoothServer;

        public HomeScanView()
        {
            _deviceList = new List<BluetoothDevice>();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.ScanView);
        }

        protected override void OnStart()
        {
            base.OnStart();
            InitializeListView();
            InvalidateOptionsMenu();

            InitializeBluetoothClient();
            InitializeBluetoothServer();
        }

        private void InitializeBluetoothClient()
        {
            _bluetoothClient = BluetoothClient.Instance;
            _bluetoothClient.Initialize();
            if (_bluetoothClient.IsInitialized)
            {
                _scanner = new BluetoothDeviceScanner(_bluetoothClient.Adapter, OnDiscoveredPeripheral);
            }
        }

        private void InitializeBluetoothServer()
        {
            _bluetoothServer = BluetoothServer.Instance;
            GattServerObserver.Instance.ServiceAdded +=
                (status, service) =>
                    DialogView.ShowDialog("Server service uuid: " + BluetoothServiceFactory.ServiceUuid, this);
        }

        protected override void OnStop()
        {
            base.OnStop();
            _deviceList.Clear();
        }

        private void InitializeListView()
        {
            var listView = FindViewById<ListView>(Resource.Id.ScanList);
            listView.ItemClick += ListViewOnItemClick;
            
            var adapter = new ArrayAdapter<string>(this, Resource.Layout.ScanRow, Resource.Id.label);
            listView.Adapter = adapter;
            _adapter = adapter;
        }

        private void ListViewOnItemClick(object sender, AdapterView.ItemClickEventArgs args)
        {
            var name = _adapter.GetItem(args.Position);

            var device = _deviceList.First(d => d.Name.Equals(name));
            _bluetoothClient.SelectedDevice = device;
            _scanner.StopScan();

            var intent = new Intent(this, typeof (DeviceView));
            StartActivity(intent);
        }

        private void OnDiscoveredPeripheral(BluetoothDevice device)
        {
            if (_deviceList.All(d => !d.Address.Equals(device.Address, StringComparison.OrdinalIgnoreCase)))
            {
                _deviceList.Add(device);
                RunOnUiThread(() =>
                {
                    _adapter.Add(device.Name);
                    _adapter.NotifyDataSetChanged();
                });
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Layout.ScanMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnMenuItemSelected(int featureId, IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.ScanAction:
                    item.SetTitle(_scanner.IsScanning ? Resource.String.StartScan : Resource.String.StopScan);
                    if (_scanner.IsScanning)
                    {
                        _scanner.StopScan();
                        break;
                    }
                    _scanner.StartScan();
                    break;

                case Resource.Id.OpenServerAction:
                    item.SetTitle(_bluetoothServer.IsOpened ?  Resource.String.OpenServer : Resource.String.CloseServer);
                    if (_bluetoothServer.IsOpened)
                    {
                        _bluetoothServer.Close();
                    }
                    else
                    {
                        _bluetoothServer.Open();
                    }
                    break;
            }
            return true;
        }
    }
}

