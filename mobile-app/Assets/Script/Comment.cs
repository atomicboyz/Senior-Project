using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Comment : MonoBehaviour {

	public Button cancel;
	public Button submit;
	public GameObject commentPanel;
	public InputField field;

	// Use this for initialization
	void Start () {
		cancel.onClick.AddListener(cancelClicked);
		submit.onClick.AddListener(submitClicked);
	}

	// Update is called once per frame
	void Update () {
	}

	void cancelClicked () {
		if (commentPanel.activeInHierarchy) {
			commentPanel.SetActive (false);
		} else {
			commentPanel.SetActive (true);
		}
	}

	void submitClicked () {
		Debug.Log (field.text); // comment text
		// submit comment here
		StartCoroutine(postRequest(Database.API_URL+"comment"));
	}

	IEnumerator postRequest(string url)
	{
		WWWForm form = new WWWForm();
        form.AddField("Comment", field.text);
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
