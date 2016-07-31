package androidbluetoothle.bluetooth.client;


public class DevicePairingHandler
	extends android.content.BroadcastReceiver
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onReceive:(Landroid/content/Context;Landroid/content/Intent;)V:GetOnReceive_Landroid_content_Context_Landroid_content_Intent_Handler\n" +
			"";
		mono.android.Runtime.register ("AndroidBluetoothLE.Bluetooth.Client.DevicePairingHandler, AndroidBluetoothLE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", DevicePairingHandler.class, __md_methods);
	}


	public DevicePairingHandler () throws java.lang.Throwable
	{
		super ();
		if (getClass () == DevicePairingHandler.class)
			mono.android.TypeManager.Activate ("AndroidBluetoothLE.Bluetooth.Client.DevicePairingHandler, AndroidBluetoothLE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}

	public DevicePairingHandler (android.app.Activity p0) throws java.lang.Throwable
	{
		super ();
		if (getClass () == DevicePairingHandler.class)
			mono.android.TypeManager.Activate ("AndroidBluetoothLE.Bluetooth.Client.DevicePairingHandler, AndroidBluetoothLE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "Android.App.Activity, Mono.Android, Version=0.0.0.0, Culture=neutral, PublicKeyToken=84e04ff9cfb79065", this, new java.lang.Object[] { p0 });
	}


	public void onReceive (android.content.Context p0, android.content.Intent p1)
	{
		n_onReceive (p0, p1);
	}

	private native void n_onReceive (android.content.Context p0, android.content.Intent p1);

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
