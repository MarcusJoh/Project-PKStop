namespace AndroidBluetoothLE.ServiceList
{
    public class ServiceListItem
    {
        public bool IsService { get; private set; }

        public object Value { get; private set; }

        public ServiceListItem(object value, bool isService)
        {
            Value = value;
            IsService = isService;
        }


    }
}