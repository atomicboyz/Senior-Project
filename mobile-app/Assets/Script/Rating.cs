using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Rating : MonoBehaviour {

	public Text value;
	public Slider slider;
	public Button cancel;
	public Button submit;
	public GameObject ratePanel;

	// Use this for initialization
	void Start () {
		value.text = "0";
		cancel.onClick.AddListener(cancelClicked);
		submit.onClick.AddListener(submitClicked);
	}
	
	// Update is called once per frame
	void Update () {
		value.text = slider.value.ToString();
	}

	void cancelClicked () {
		if (ratePanel.activeInHierarchy) {
			ratePanel.SetActive (false);
		} else {
			ratePanel.SetActive (true);
		}
	}

	void submitClicked () {
		Debug.Log (value.text); // rating value
		// submit rating here
		StartCoroutine(postRequest(Database.API_URL+"rate"));
	}

	IEnumerator postRequest(string url)
	{
		string username = "name";
		WWWForm form = new WWWForm();
        form.AddField("Rating", ""+slider.value);
		form.AddField("Username", username);
		form.AddField("locationId", Database.ListLocations.locations[TouchScript.currentArrowIndex].id);
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
