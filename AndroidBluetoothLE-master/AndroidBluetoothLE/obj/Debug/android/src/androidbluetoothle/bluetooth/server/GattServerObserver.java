package androidbluetoothle.bluetooth.server;


public class GattServerObserver
	extends android.bluetooth.BluetoothGattServerCallback
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCharacteristicWriteRequest:(Landroid/bluetooth/BluetoothDevice;ILandroid/bluetooth/BluetoothGattCharacteristic;ZZI[B)V:GetOnCharacteristicWriteRequest_Landroid_bluetooth_BluetoothDevice_ILandroid_bluetooth_BluetoothGattCharacteristic_ZZIarrayBHandler\n" +
			"n_onConnectionStateChange:(Landroid/bluetooth/BluetoothDevice;II)V:GetOnConnectionStateChange_Landroid_bluetooth_BluetoothDevice_IIHandler\n" +
			"n_onServiceAdded:(ILandroid/bluetooth/BluetoothGattService;)V:GetOnServiceAdded_ILandroid_bluetooth_BluetoothGattService_Handler\n" +
			"";
		mono.android.Runtime.register ("AndroidBluetoothLE.Bluetooth.Server.GattServerObserver, AndroidBluetoothLE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", GattServerObserver.class, __md_methods);
	}


	public GattServerObserver () throws java.lang.Throwable
	{
		super ();
		if (getClass () == GattServerObserver.class)
			mono.android.TypeManager.Activate ("AndroidBluetoothLE.Bluetooth.Server.GattServerObserver, AndroidBluetoothLE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCharacteristicWriteRequest (android.bluetooth.BluetoothDevice p0, int p1, android.bluetooth.BluetoothGattCharacteristic p2, boolean p3, boolean p4, int p5, byte[] p6)
	{
		n_onCharacteristicWriteRequest (p0, p1, p2, p3, p4, p5, p6);
	}

	private native void n_onCharacteristicWriteRequest (android.bluetooth.BluetoothDevice p0, int p1, android.bluetooth.BluetoothGattCharacteristic p2, boolean p3, boolean p4, int p5, byte[] p6);


	public void onConnectionStateChange (android.bluetooth.BluetoothDevice p0, int p1, int p2)
	{
		n_onConnectionStateChange (p0, p1, p2);
	}

	private native void n_onConnectionStateChange (android.bluetooth.BluetoothDevice p0, int p1, int p2);


	public void onServiceAdded (int p0, android.bluetooth.BluetoothGattService p1)
	{
		n_onServiceAdded (p0, p1);
	}

	private native void n_onServiceAdded (int p0, android.bluetooth.BluetoothGattService p1);

	java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
