package crc643bde8c398b6cd8f7;


public class ActividadPrincipal
	extends android.app.Activity
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onCreate:(Landroid/os/Bundle;)V:GetOnCreate_Landroid_os_Bundle_Handler\n" +
			"";
		mono.android.Runtime.register ("Ejercicio2.ActividadPrincipal, Ejercicio2", ActividadPrincipal.class, __md_methods);
	}


	public ActividadPrincipal ()
	{
		super ();
		if (getClass () == ActividadPrincipal.class)
			mono.android.TypeManager.Activate ("Ejercicio2.ActividadPrincipal, Ejercicio2", "", this, new java.lang.Object[] {  });
	}


	public void onCreate (android.os.Bundle p0)
	{
		n_onCreate (p0);
	}

	private native void n_onCreate (android.os.Bundle p0);

	private java.util.ArrayList refList;
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
