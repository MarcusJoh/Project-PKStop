using System.Collections.Generic;
using Android.App;
using Android.Bluetooth;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace AndroidBluetoothLE.ServiceList
{
    public class ServiceListAdapter : BaseAdapter<ServiceListItem>
    {
        private readonly Activity _context;
        private readonly IList<ServiceListItem> _itemList;

        public ServiceListAdapter(Activity context, IList<ServiceListItem> itemList)
        {
            _context = context;
            _itemList = itemList;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = _itemList[position].Value;

            if (item is BluetoothGattCharacteristic)
            {
                return GetCharacteristicView((BluetoothGattCharacteristic) item);
            }
            if (item is BluetoothGattService)
            {
                return GetServiceView((BluetoothGattService)item);
            }

            return null;
        }

        private View GetCharacteristicView(BluetoothGattCharacteristic characteristic)
        {
            var itemView = _context.LayoutInflater.Inflate(Android.Resource.Layout.TwoLineListItem, null);

            var itemText1 = itemView.FindViewById<TextView>(Android.Resource.Id.Text1);
            var itemText2 = itemView.FindViewById<TextView>(Android.Resource.Id.Text2);

            itemText1.Text = characteristic.Properties + " Characteristic";
            itemText2.Text = characteristic.Uuid.ToString();

            return itemView;
        }

        private View GetServiceView(BluetoothGattService service)
        {
            var itemView = _context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);

            var itemText = itemView.FindViewById<TextView>(Android.Resource.Id.Text1);
            itemText.Text = "Service " + service.Uuid;
            itemText.SetTextSize(ComplexUnitType.Sp, 15);
            itemText.SetTextColor(Color.Gray);

            return itemView;
        }

        public override int Count
        {
            get { return _itemList.Count; }
        }

        public override ServiceListItem this[int position]
        {
            get { return _itemList[position]; }
        }
    }
}