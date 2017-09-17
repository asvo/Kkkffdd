using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class TryPlugin : MonoBehaviour
{

	public Text t;

	public void ToTry ()
	{
		try {
			t.text = "start android plugins";

			t.text = t.text + "\r\n";
			AndroidJavaClass jc = new AndroidJavaClass ("com.innyo.androidplugin.apTest");
			t.text = t.text + "AndroidJavaClass jc:" + jc.ToString ();

			t.text = t.text + "\r\n";
			t.text = t.text + "tryProp:" + jc.Get<string> ("tryProp");

			t.text = t.text + "\r\n";
			t.text = t.text + "tryStaticProp:" + jc.GetStatic<string> ("tryStaticProp");

			t.text = t.text + "\r\n";
			t.text = t.text + "TryPublic:" + jc.Call<string> ("TryPublic");

			t.text = t.text + "\r\n";
			t.text = t.text + "TryPublic:" + jc.Call<string> ("TryPublic", "unity3d input");

			t.text = t.text + "\r\n";
			t.text = t.text + "TryStaticPublic:" + jc.CallStatic<string> ("TryStaticPublic");

			t.text = t.text + "\r\n";
			t.text = t.text + "TryStaticPublic:" + jc.CallStatic<string> ("TryStaticPublic", "unity3d input");

			t.text = t.text + "\r\n";
			t.text = t.text + "----------------------------------------";

			t.text = t.text + "\r\n";
			AndroidJavaObject jo = new AndroidJavaObject("com.innyo.androidplugin.apTest","unity3d jo build");
			t.text = t.text + "AndroidJavaObject jo:" + jo.ToString ();

			t.text = t.text + "\r\n";
			t.text = t.text + "tryProp:" + jo.Get<string> ("tryProp");

			t.text = t.text + "\r\n";
			t.text = t.text + "tryStaticProp:" + jo.GetStatic<string> ("tryStaticProp");

			t.text = t.text + "\r\n";
			t.text = t.text + "TryPublic:" + jo.Call<string> ("TryPublic");

			t.text = t.text + "\r\n";
			t.text = t.text + "TryPublic:" + jo.Call<string> ("TryPublic", "unity3d input");

			t.text = t.text + "\r\n";
			t.text = t.text + "TryStaticPublic:" + jo.CallStatic<string> ("TryStaticPublic");

			t.text = t.text + "\r\n";
			t.text = t.text + "TryStaticPublic:" + jo.CallStatic<string> ("TryStaticPublic", "unity3d input");

		} catch (Exception ex)
        {
            Debug.Log(ex.ToString());
			t.text = t.text + "\r\n";
			t.text = t.text + ex.Message;
		}
	}
}
