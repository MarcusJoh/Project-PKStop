using System;
using System.Diagnostics;
using Android.App;
using Android.Bluetooth;
using Android.Content;

namespace AndroidBluetoothLE.Bluetooth.Client
{
    public class DevicePairingHandler : BroadcastReceiver
    {
        private readonly Activity _currentActivity;

        public DevicePairingHandler(Activity activity)
        {
            _currentActivity = activity;
            Application.Context.RegisterReceiver(this, new IntentFilter(BluetoothDevice.ActionBondStateChanged));
        }

        public override void OnReceive(Context context, Intent intent)
        {
            var device = (BluetoothDevice)intent.GetParcelableExtra(BluetoothDevice.ExtraDevice);

            Debug.WriteLine("Result of trying to pair with pager: " + device.BondState);

            var message = string.Empty;
            switch (device.BondState)
            {
                case Bond.Bonding:
                    message = "Bonding with device...";
                    break;
                case Bond.Bonded:
                    message = "Successfuly Bonded";
                    break;
                case Bond.None:
                    message = "Failed To Bond";
                    break;
            }
            DialogView.ShowDialog(message, _currentActivity);
        }


        public void Unregister()
        {
            Application.Context.UnregisterReceiver(this);
        }
    }
}