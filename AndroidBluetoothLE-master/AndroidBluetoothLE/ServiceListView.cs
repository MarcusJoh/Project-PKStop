using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidBluetoothLE.Bluetooth.Client;
using AndroidBluetoothLE.ServiceList;
using Java.Util;

namespace AndroidBluetoothLE
{
    [Activity(Label = "Service View")]
    public class ServiceListView : Activity
    {
        private ServiceListAdapter _adapter;
        private readonly List<GattProperty> _allowedProperties = new List<GattProperty>
        {
            GattProperty.Write, GattProperty.WriteNoResponse, GattProperty.SignedWrite, GattProperty.Notify, GattProperty.Read
        };

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.ServiceListView);

            DialogView.ShowDialog("Discovering services...", this);
            BluetoothClient.Instance.ConnectionHandler.DiscoverServices(status =>
            {
                if (status == GattStatus.Success)
                {
                    DialogView.CloseDialog(this);
                    RunOnUiThread(() => ShowServicesAndCharacteristics(BluetoothClient.Instance.ConnectionHandler));
                }
                else
                {
                    DialogView.ShowDialog("Failed to discover. Return and try again", this);
                }
            });
            
        }

        private void ShowServicesAndCharacteristics(BluetoothConnectionHandler connectionHandler)
        {
            var listView = FindViewById<ListView>(Resource.Id.ServiceList);

            var filterUuid = new [] { UUID.FromString("00001800-0000-1000-8000-00805F9B34FB"), 
                UUID.FromString("00001801-0000-1000-8000-00805F9B34FB"), UUID.FromString("7905F431-B5CE-4E99-A40F-4B1E122D00D0") };
            
            var serviceList = connectionHandler.GetServiceList().Where(s => filterUuid.All(uuid => !uuid.Equals(s.Uuid)));
            listView.Adapter = _adapter = CreateServiceListAdapter(serviceList);
            listView.ItemClick += OnItemClick;
        }

        private ServiceListAdapter CreateServiceListAdapter(IEnumerable<BluetoothGattService> services)
        {
            var serviceItemList = new List<ServiceListItem>();
            foreach (var service in services)
            {
                serviceItemList.Add(new ServiceListItem(service, true));
                serviceItemList.AddRange(
                    service.Characteristics.Select(characteristic => new ServiceListItem(characteristic, false)));
            }
            return new ServiceListAdapter(this, serviceItemList);
        }

        private void OnItemClick(object sender, AdapterView.ItemClickEventArgs args)
        {
            var serviceItem = _adapter[args.Position];
            if (IsClickableItem(serviceItem))
            {
                ShowCharacteristicView((BluetoothGattCharacteristic)serviceItem.Value);
            }
        }

        private void ShowCharacteristicView(BluetoothGattCharacteristic characteristic)
        {
            var extraString = "Characteristic";
            Intent intent = null;

            var property = _allowedProperties.FirstOrDefault(p => characteristic.Properties.HasFlag(p));

            switch (property)
            {
                case GattProperty.Read:
                    intent = new Intent(this, typeof(ReadCharacteristicView)).PutExtra(extraString, characteristic.Uuid);
                    break;
                case GattProperty.Notify:
                    intent = new Intent(this, typeof(NotifyCharacteristicView)).PutExtra(extraString, characteristic.Uuid);
                    break;
                case GattProperty.Write:
                case GattProperty.WriteNoResponse:
                case GattProperty.SignedWrite:
                    intent = new Intent(this, typeof(WriteCharacteristicView)).PutExtra(extraString, characteristic.Uuid);
                    break;
            }

            if (intent != null)
            {
                StartActivity(intent);
            }
        }

        private bool IsClickableItem(ServiceListItem serviceItem)
        {
            if (serviceItem.IsService || !(serviceItem.Value is BluetoothGattCharacteristic)) return false;
            
            var properties = ((BluetoothGattCharacteristic) serviceItem.Value).Properties;
            return _allowedProperties.Any(p => properties.HasFlag(p));
        }
    }
}