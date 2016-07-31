using Android.App;
using Android.OS;
using AndroidBluetoothLE.Bluetooth.Client;

namespace AndroidBluetoothLE
{
    public class BaseCharacteristicView : Activity
    {
        private DevicePairingHandler _pairingHandler;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            _pairingHandler = new DevicePairingHandler(this);
        }

        protected override void OnStop()
        {
            base.OnStop();
            _pairingHandler.Unregister();
        }
    }
}