using System;
using Android.Bluetooth;

namespace AndroidBluetoothLE.Bluetooth.Client
{
    public delegate void ReceivedWriteResponceEventHandler(
        BluetoothGattCharacteristic characteristic, GattStatus status);

    public class DeviceWritingHandler : IDisposable
    {
        private readonly BluetoothGatt _gatt;
        private readonly GattClientObserver _gattObserver;

        public event ReceivedWriteResponceEventHandler ReceivedWriteResponce;

        public DeviceWritingHandler(BluetoothGatt gatt, GattClientObserver gattObserver)
        {
            _gatt = gatt;
            _gattObserver = gattObserver;
            gattObserver.CharacteristicWritten += OnCharacteristicWritten;
        }

        public void Dispose()
        {
            _gattObserver.CharacteristicWritten -= OnCharacteristicWritten;
        }

        public void Write(byte[] buffer, BluetoothGattCharacteristic characteristic, bool withResponce = false)
        {
            WriteValueInternal(buffer, characteristic, withResponce);
        }

        private void OnCharacteristicWritten(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic, GattStatus status)
        {
            var handler = ReceivedWriteResponce;
            if (handler != null) handler(characteristic, status);
        }

        private void WriteValueInternal(byte[] buffer, BluetoothGattCharacteristic characteristic, bool withResponce)
        {
            characteristic.SetValue(buffer);
            characteristic.WriteType = withResponce ? GattWriteType.Default : GattWriteType.NoResponse;
            _gatt.WriteCharacteristic(characteristic);
            
        }
    }
}