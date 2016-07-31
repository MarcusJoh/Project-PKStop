using System;
using System.Linq;
using Android.Bluetooth;
using Java.Util;

namespace AndroidBluetoothLE.Bluetooth.Client
{
    public delegate void NotifyValueChangedEventHandler(
        BluetoothGatt gatt, BluetoothGattCharacteristic characteristic);

    public class DeviceNotifyingHandler : IDisposable
    {
        private readonly BluetoothGatt _gatt;
        private Action<bool> _onSubscribed;
        private GattClientObserver _gattObserver;

        public event NotifyValueChangedEventHandler ValueChanged;

        public DeviceNotifyingHandler(BluetoothGatt gatt, GattClientObserver gattObserver)
        {
            _gatt = gatt;
            _gattObserver = gattObserver;
            gattObserver.CharacteristicValueChanged += OnCharacteristicValueChanged;
            gattObserver.DescriptorWritten += OnDescriptorWritten;
        }

        private void OnDescriptorWritten(BluetoothGatt gatt, BluetoothGattDescriptor descriptor, GattStatus status)
        {
            if (status == GattStatus.Success && _onSubscribed != null)
            {
                _onSubscribed(true);
            }
        }

        public void Subscribe(BluetoothGattCharacteristic characteristic, Action<bool> onSubscribed)
        {
            _onSubscribed = onSubscribed;

            SubscribeCharacteristic(characteristic);
        }

        public void Unsubscribe(BluetoothGattCharacteristic characteristic)
        {
            _gatt.SetCharacteristicNotification(characteristic, false);
        }

        private void OnCharacteristicValueChanged(BluetoothGatt gatt, BluetoothGattCharacteristic characteristic)
        {
            var handler = ValueChanged;
            if (handler != null) handler(gatt, characteristic);
        }

        private void SubscribeCharacteristic(BluetoothGattCharacteristic characteristic)
        {
            _gatt.SetCharacteristicNotification(characteristic, true);

            var descriptor = characteristic.GetDescriptor(UUID.FromString("00002902-0000-1000-8000-00805f9b34fb"));
            descriptor.SetValue(BluetoothGattDescriptor.EnableNotificationValue.ToArray());
            _gatt.WriteDescriptor(descriptor);
        }

        public void Dispose()
        {
            _gattObserver.CharacteristicValueChanged -= OnCharacteristicValueChanged;
            _gattObserver.DescriptorWritten -= OnDescriptorWritten;
        }
    }
}