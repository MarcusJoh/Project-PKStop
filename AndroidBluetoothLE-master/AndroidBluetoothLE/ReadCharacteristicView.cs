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
    [Activity(Label = "Read Characteristic")]
    public class ReadCharacteristicView : BaseCharacteristicView
    {
        private BluetoothGattCharacteristic _characteristic;
        private DeviceReadingHandler _readingHandler;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ReadCharacteristicView);

            InitializeView();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            _readingHandler.Dispose();
        }

        private void InitializeView()
        {
            var connectionHandler = BluetoothClient.Instance.ConnectionHandler;
            _readingHandler = new DeviceReadingHandler(connectionHandler.GattValue, GattClientObserver.Instance);
            _characteristic = GetCharacteristic(connectionHandler.GetServiceList());

            var readButton = FindViewById<Button>(Resource.Id.ReadCharacteristicButton);
            readButton.Click += ReadButtonOnClick;
        }

        private BluetoothGattCharacteristic GetCharacteristic(IEnumerable<BluetoothGattService> serviceList)
        {
            var uuid = (UUID)Intent.GetSerializableExtra("Characteristic");

            var service = serviceList.First(s => s.Characteristics.Any(ch => ch.Uuid.Equals(uuid)));
            return service.Characteristics.First(ch => ch.Uuid.Equals(uuid));
        }

        private void ReadButtonOnClick(object sender, EventArgs eventArgs)
        {
            var hexText = FindViewById<TextView>(Resource.Id.HexReadText);
            var stringText = FindViewById<TextView>(Resource.Id.StringReadText);

            ShowDialog("Start reading...");
            _readingHandler.Read(_characteristic, (bytes, status) => RunOnUiThread(() => 
            {
                if (status != GattStatus.Success)
                {
                    ShowDialog("Reading failed with status: " + status);
                    return;
                }

                hexText.SetText(BitConverter.ToString(bytes), TextView.BufferType.Normal);
                stringText.SetText(Encoding.ASCII.GetString(bytes), TextView.BufferType.Normal);
                ShowDialog("Read Success!");
            }));
        }

        private void ShowDialog(string message)
        {
            DialogView.ShowDialog(message, this);
        }
    }
}