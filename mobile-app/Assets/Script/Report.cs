using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Report : MonoBehaviour {

	public Button cancel;
	public Button submit;
	public GameObject reportPanel;
	public Toggle other;
	public Toggle notExist;
	public InputField field;

	// Use this for initialization
	void Start () {
		cancel.onClick.AddListener(cancelClicked);
		submit.onClick.AddListener(submitClicked);
		field.interactable = false;
	}

	// Update is called once per frame
	void Update () {
		if (other.isOn) {
			field.interactable = true;
		} else {
			field.interactable = false;
		}
	}

	void cancelClicked () {
		if (reportPanel.activeInHierarchy) {
			reportPanel.SetActive (false);
		} else {
			reportPanel.SetActive (true);
		}
	}

	void submitClicked () {
		if (other.enabled) {
			Debug.Log (field.text); // input field text
			// submit text here
			StartCoroutine (postRequest (Database.API_URL+"report"));
		}
	}

	IEnumerator postRequest(string url)
	{
		string username = "name";
		string type = "";
		if(notExist.isOn) type = "Place doesn't exist";
		else type = "Other";
		WWWForm form = new WWWForm();
        form.AddField("Type", type);
		form.AddField("Username", username);
		form.AddField("Detail", field.text);
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
