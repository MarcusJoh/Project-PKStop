package androidbluetoothle;


public class BaseCharacteristicView
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
	static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"n_onStop:()V:GetOnStopHandler\n" +
			"";
		mono.android.Runtime.register ("AndroidBluetoothLE.BaseCharacteristicView, AndroidBluetoothLE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", BaseCharacteristicView.class, __md_methods);
	}


	public BaseCharacteristicView () throws java.lang.Throwable
	{
		super ();
		if (getClass () == BaseCharacteristicView.class)
			mono.android.TypeManager.Activate ("AndroidBluetoothLE.BaseCharacteristicView, AndroidBluetoothLE, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);


	public void onStop ()
	{
		n_onStop ();
	}

	private native void n_onStop ();

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
