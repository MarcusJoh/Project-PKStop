using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Bluetooth;
using Android.OS;
using Android.Widget;
using AndroidBluetoothLE.Bluetooth.Client;
using Java.Util;

namespace AndroidBluetoothLE
{
    [Activity(Label = "Notify Characteristic")]
    public class NotifyCharacteristicView : BaseCharacteristicView
    {
        private DeviceNotifyingHandler _notifyHandler;
        private BluetoothGattCharacteristic _characteristic;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.NotifyCharacteristicView);

            InitializeView();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _notifyHandler.Dispose();
        }

        private void InitializeView()
        {
            var connectionHandler = BluetoothClient.Instance.ConnectionHandler;
            _notifyHandler = new DeviceNotifyingHandler(connectionHandler.GattValue, GattClientObserver.Instance);
            _characteristic = GetCharacteristic(connectionHandler.GetServiceList());

            _notifyHandler.ValueChanged += NotifyOnValueChanged;
            _notifyHandler.Subscribe(_characteristic,
                result => DialogView.ShowDialog(result ? "Successfuly subscribed" : "Failed to subscribe", this));
        }

        private void NotifyOnValueChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic)
        {
            var hexText = FindViewById<TextView>(Resource.Id.ReceivedNotificationHex);
            var stringText = FindViewById<TextView>(Resource.Id.ReceivedNotificationString);

            var receivedBytes = characteristic.GetValue();

            RunOnUiThread(() =>
            {
                hexText.SetText(BitConverter.ToString(receivedBytes), TextView.BufferType.Normal);
                stringText.SetText(Encoding.ASCII.GetString(receivedBytes), TextView.BufferType.Normal);
            });
            
        }

        private BluetoothGattCharacteristic GetCharacteristic(IEnumerable<BluetoothGattService> serviceList)
        {
            var uuid = (UUID)Intent.GetSerializableExtra("Characteristic");

            var service = serviceList.First(s => s.Characteristics.Any(ch => ch.Uuid.Equals(uuid)));
            return service.Characteristics.First(ch => ch.Uuid.Equals(uuid));
        }
    }
}