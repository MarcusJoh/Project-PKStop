using System;
using Android.Bluetooth;

namespace AndroidBluetoothLE.Bluetooth.Client
{
    public class DeviceReadingHandler : IDisposable
    {
        private readonly BluetoothGatt _gatt;
        private readonly GattClientObserver _gattObserver;
        private Action<byte[], GattStatus> _onRead;


        public DeviceReadingHandler(BluetoothGatt gatt, GattClientObserver gattObserver)
        {
            _gatt = gatt;
            _gattObserver = gattObserver;
            gattObserver.CharacteristicRead += GattObserverOnCharacteristicRead;
        }

        public void Read(BluetoothGattCharacteristic characteristic, Action<byte[], GattStatus> onRead)
        {
            _onRead = onRead;
            _gatt.ReadCharacteristic(characteristic);
        }

        public void Dispose()
        {
            _gattObserver.CharacteristicRead -= GattObserverOnCharacteristicRead;
        }

        private void GattObserverOnCharacteristicRead(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, GattStatus status)
        {
            if (_onRead != null)
            {
                _onRead(characteristic.GetValue(), status);
            }
        }
    }
}