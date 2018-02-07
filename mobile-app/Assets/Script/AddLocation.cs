using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class AddLocation : MonoBehaviour {

	public Button cancel;
	public Button submit;
	public Button takeAPhoto;
	public Image examplePic;
	public GameObject addLocationPanel;
	public InputField placename;
	public Dropdown category;
	// Use this for initialization
	void Start () {
		cancel.onClick.AddListener(cancelClicked);
		submit.onClick.AddListener(submitClicked);
		takeAPhoto.onClick.AddListener(()=>{
			Texture2D snap = new Texture2D(PhoneCamera.cameraTexture.height,PhoneCamera.cameraTexture.width);
			Color[] pixels = PhoneCamera.cameraTexture.GetPixels();
			Color[] ret = new Color[snap.width * snap.height];
			
			for (int i = 0; i < snap.width; ++i) {
				for (int j = 0; j < snap.height; ++j) {
					ret[i + (snap.height-1-j) * snap.width] = pixels[j + i * snap.height];
				}
			}
			snap.SetPixels(ret);
			snap.Apply();
			examplePic.sprite = Sprite.Create(snap, new Rect (0, 0, snap.width, snap.height), new Vector2 ());
			// bool fail = false;
            // string bundleId = "com.android.camera"; // your target bundle id
            // AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            // AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
            // AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
     
            // AndroidJavaObject launchIntent = null;
            // try
            // {
                // launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage",bundleId);
            // }
            // catch (System.Exception e)
            // {
                // fail = true;
            // }
 
            // if (fail)
            // { //open app in store
                // Application.OpenURL("https://google.com");
            // }
            // else //open the app
                // ca.Call("startActivity",launchIntent);
 
            // up.Dispose();
            // ca.Dispose();
            // packageManager.Dispose();
            // launchIntent.Dispose();
		});
	}

	// Update is called once per frame
	void Update () {
	}

	void cancelClicked () {
		if (addLocationPanel.activeInHierarchy) {
			addLocationPanel.SetActive (false);
		} else {
			addLocationPanel.SetActive (true);
		}
	}

	void submitClicked () {
		// submit location here
		StartCoroutine(postRequest(Database.API_URL+"addlocation"));
	}

	IEnumerator postRequest(string url)
	{
		float lati = Location.myLatitude;
		float longti = Location.myLongtitude;
		string imgUrl = "https://th2-cdn.pgimgs.com/listing/4203071/UPHO.34272035.V800/MBK-Center-%E0%B8%9B%E0%B8%97%E0%B8%B8%E0%B8%A1%E0%B8%A7%E0%B8%B1%E0%B8%99-Thailand.jpg";
		WWWForm form = new WWWForm();
        form.AddField("Name", placename.text);
		form.AddField("Category", category.captionText.text);
		form.AddField("Latitude", ""+lati);
		form.AddField("Longtitude", ""+longti);
		form.AddField("URLPic", imgUrl);
		UnityWebRequest www = UnityWebRequest.Post(url, form);
        yield return www.SendWebRequest();
 
        if(www.isNetworkError || www.isHttpError) {
            Debug.Log(www.error);
        }
        else {
            Debug.Log("Form upload complete!");
        }
	}
}
