using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuClicked : MonoBehaviour {

	public GameObject category;
	public GameObject addLocationPanel;
	public Button menu;
	public Button back;
	public Button addLocation;

	// Use this for initialization
	void Start () {
		menu.onClick.AddListener(menuClicked);//adds a listener for when you click the button
		back.onClick.AddListener(backClicked);
		addLocation.onClick.AddListener(addLocationClicked);
	}
	
	// Update is called once per frame
	void Update () {
		

	}

	void menuClicked()// your listener calls this function
	{
		//your code here 
		if (!category.activeInHierarchy) {
			category.SetActive (true);
		} else {
			category.SetActive (false);
		}
		//Debug.Log("Button clicked");
	}

	void backClicked()// your listener calls this function
	{
		//your code here 
		if (category.activeInHierarchy) {
			category.SetActive (false);
		} else {
			category.SetActive (true);
		}
		//Debug.Log("Button clicked");
	}

	void addLocationClicked() {
		//your code here 
		if (!addLocationPanel.activeInHierarchy) {
			addLocationPanel.SetActive (true);
		} else {
			addLocationPanel.SetActive (false);
		}
		//Debug.Log("Button clicked");
	}
}
