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
    [Activity(Label = "Write Characteristic")]
    public class WriteCharacteristicView : BaseCharacteristicView
    {
        private DeviceWritingHandler _writingHandler;
        private BluetoothGattCharacteristic _characteristic;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.WriteCharacteristicView);

            InitalizeView();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _writingHandler.Dispose();
            _writingHandler.ReceivedWriteResponce -= OnReceivedWriteResponce;
        }

        private void InitalizeView()
        {
            var connectionHandler = BluetoothClient.Instance.ConnectionHandler;
            _writingHandler = new DeviceWritingHandler(connectionHandler.GattValue, GattClientObserver.Instance);
            _writingHandler.ReceivedWriteResponce += OnReceivedWriteResponce;
            _characteristic = GetCharacteristic(connectionHandler.GetServiceList());

            var writeButton = FindViewById<Button>(Resource.Id.WriteCharacteristicButton);
            writeButton.Click += WriteButtonOnClick;
        }

        private BluetoothGattCharacteristic GetCharacteristic(IEnumerable<BluetoothGattService> serviceList)
        {
            var uuid = (UUID)Intent.GetSerializableExtra("Characteristic");

            var service = serviceList.First(s => s.Characteristics.Any(ch => ch.Uuid.Equals(uuid)));
            return service.Characteristics.First(ch => ch.Uuid.Equals(uuid));
        }

        private void WriteButtonOnClick(object sender, EventArgs eventArgs)
        {
            var text = FindViewById<EditText>(Resource.Id.WriteTextField).Text;
            if (string.IsNullOrEmpty(text))
            {
                ShowDialog("Enter some text to write!");
                return;
            }

            ShowDialog("Start writing...");
            _writingHandler.Write(Encoding.ASCII.GetBytes(text), _characteristic, true);
        }

        private void OnReceivedWriteResponce(BluetoothGattCharacteristic characteristic, GattStatus status)
        {
            ShowDialog(status != GattStatus.Success ? "Failed to write" : "Write success!");
        }

        private void ShowDialog(string message)
        {
            DialogView.ShowDialog(message, this);
        }
    }
}
