using System;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidBluetoothLE.Bluetooth.Client;
using AndroidBluetoothLE.Bluetooth.Server;

namespace AndroidBluetoothLE
{
    [Activity(Label = "DeviceView")]
    public class DeviceView : Activity
    {
        private BluetoothConnectionHandler _connectionHandler;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.DeviceView);
            _connectionHandler = BluetoothClient.Instance.ConnectionHandler;

            FindViewById<Button>(Resource.Id.DiscoverServicesButton).Click += DiscoverServicesButtonClick;
            FindViewById<Button>(Resource.Id.ReconnectButton).Click += OnReconnect;

            var serverVisibility = BluetoothServer.Instance.IsOpened ? ViewStates.Visible : ViewStates.Invisible;
            FindViewById<TextView>(Resource.Id.ServerRequestCaption).Visibility = serverVisibility;
            FindViewById<TextView>(Resource.Id.ServerRequestText).Visibility = serverVisibility;
        }

        private async void OnReconnect(object sender, EventArgs eventArgs)
        {
            if (_connectionHandler.IsConnected)
            {
                ShowDialog("Disconnecting...");
                await _connectionHandler.DisconnectAsync();
            }

            ConnectDevice();
        }

        private void DiscoverServicesButtonClick(object sender, EventArgs eventArgs)
        {
            var intent = new Intent(this, typeof(ServiceListView));
            StartActivity(intent);
        }

        protected override void OnStart()
        {
            base.OnStart();
            if (!_connectionHandler.IsConnected)
            {
                ConnectDevice();
            }
        }

        private void ConnectDevice()
        {
            var client = BluetoothClient.Instance;
            ShowDialog("Connecting Device...");
            _connectionHandler.Connect(client.SelectedDevice, OnConnectionChanged);
        }

        private void OnConnectionChanged(ProfileState profileState)
        {
            DialogView.CloseDialog(this);
            if (profileState == ProfileState.Disconnected)
            {
                FinishActivity(123);
            }
        }
        
        private void ShowDialog(string message)
        {
            DialogView.ShowDialog(message, this);
        }
    }
}