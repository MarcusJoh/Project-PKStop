using System;
using System.Diagnostics;
using Android.App;
using Android.Bluetooth;
using Android.Content;

namespace AndroidBluetoothLE.Bluetooth.Server
{
    public delegate void ReceivedMessageEventHandler(string text);

    public class BluetoothServer
    {
        private static BluetoothServer _instance;

        public static BluetoothServer Instance
        {
            get { return _instance ?? (_instance = new BluetoothServer()); }
        }

        private readonly GattServerObserver _gattObserver;
        private BluetoothGattServer _gattServer;

        public bool IsOpened { get; private set; }

        public event ReceivedMessageEventHandler ReceivedMessage;

        public void Open()
        {
            var context = Application.Context;

            var manager = (BluetoothManager)context.GetSystemService(Context.BluetoothService);
            _gattServer = manager.OpenGattServer(context, _gattObserver);

            if (_gattServer == null)
            {
                Debug.WriteLine("Couldn't open Gatt Server!");
                return;
            }
            IsOpened = true;

            var serviceFactory = new BluetoothServiceFactory();
            _gattServer.AddService(serviceFactory.CreateService());
        }

        public void Close()
        {
            if (_gattServer != null && IsOpened)
            {
                IsOpened = false;
                _gattServer.Close();
            }
        }

        private void OnCharasteristicWriteRequested(BluetoothDevice device, int requestId,
            BluetoothGattCharacteristic characteristic, bool preparedWrite, bool responseNeeded, int offset, byte[] value)
        {
            Debug.WriteLine("Received Write Request from device");

            var handler = ReceivedMessage;
            if (handler != null)
            {
                ReceivedMessage(BitConverter.ToString(value));
            }
        }

        private BluetoothServer()
        {
            _gattObserver = GattServerObserver.Instance;
            _gattObserver.CharasteristicWriteRequested += OnCharasteristicWriteRequested;
        }
    }
}