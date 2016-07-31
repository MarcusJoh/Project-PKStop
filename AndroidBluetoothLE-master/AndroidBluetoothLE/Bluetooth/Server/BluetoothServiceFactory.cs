using Android.Bluetooth;
using Java.Util;

namespace AndroidBluetoothLE.Bluetooth.Server
{
    public class BluetoothServiceFactory
    {
        public const string ServiceUuid = "AAAAAA00-0000-0000-0000-000000000000";
        public const string WriteCharacteristic = "AAAAAA01-0000-0000-0000-000000000000";

        public BluetoothGattService CreateService()
        {
            var serviceUuid = UUID.FromString(ServiceUuid);
            var service = new BluetoothGattService(serviceUuid, GattServiceType.Primary);

            service.AddCharacteristic(new BluetoothGattCharacteristic(UUID.FromString(WriteCharacteristic),
                GattProperty.WriteNoResponse, GattPermission.Write));

            return service;
        }
    }
}