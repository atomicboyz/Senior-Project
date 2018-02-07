using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TouchScript : MonoBehaviour {

	public GameObject detail;
	public GameObject ratePanel;
	public GameObject reportPanel;
	public GameObject commentPanel;

	public Button back;

	public Button rate;
	public Button report;
	public Button comment;

	public Image image;
	public Text locationName;
	public Text category;
	public Text location;
	public Text rating;
	public Text comments;
	
	public static int currentArrowIndex = 0;
	// Use this for initialization
	void Start () {
		back.onClick.AddListener(backClicked);
		rate.onClick.AddListener(rateClicked);
		report.onClick.AddListener(reportClicked);
		comment.onClick.AddListener(commentClicked);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit hit;
			if(Physics.Raycast(ray, out hit)) {
				//Debug.Log (hit.transform.name);
				string[] hitName = hit.transform.name.Split(' ');
				if (hitName[0] == "Arrow") {
					int arrowIndex = int.Parse(hitName[1]);
					if (!detail.activeInHierarchy) {
						detail.SetActive (true); 
						currentArrowIndex = arrowIndex;
						locationName.text = Database.ListLocations.locations[arrowIndex].Name;
						category.text = "Category : " + Database.ListLocations.locations[arrowIndex].Category;
						location.text = "Location : " + Database.ListLocations.locations[arrowIndex].Latitude + "," + Database.ListLocations.locations[arrowIndex].Longtitude;
						rating.text = "Rating: " + Database.ListLocations.locations[arrowIndex].Rating;
						comments.text = "";
						foreach(string comment in Database.ListLocations.locations[arrowIndex].Comments){
							comments.text += comment+"\n";
						}
						StartCoroutine(LoadImage(Database.ListLocations.locations[arrowIndex].URLPic));
					} else {
						detail.SetActive (false); 
					}
				}

			}
		}
			
	}

	void backClicked()// your listener calls this function
	{
		//your code here 
		if (detail.activeInHierarchy) {
			detail.SetActive (false);
		} else {
			detail.SetActive (true);
		}
		//Debug.Log("Button clicked");
	}

	void rateClicked()// your listener calls this function
	{
		//your code here 
		if (!ratePanel.activeInHierarchy) {
			ratePanel.SetActive (true);
		} else {
			ratePanel.SetActive (false);
		}
		//Debug.Log("Button clicked");
	}

	void reportClicked()// your listener calls this function
	{
		//your code here 
		if (!reportPanel.activeInHierarchy) {
			reportPanel.SetActive (true);
		} else {
			reportPanel.SetActive (false);
		}
		//Debug.Log("Button clicked");
	}

	void commentClicked()// your listener calls this function
	{
		//your code here 
		if (!commentPanel.activeInHierarchy) {
			commentPanel.SetActive (true);
		} else {
			commentPanel.SetActive (false);
		}
		//Debug.Log("Button clicked");
	}
	
	IEnumerator LoadImage(string url){
        using (WWW www = new WWW(url))
        {
            yield return www;
			
			image.sprite = Sprite.Create(www.texture, new Rect (0, 0, 800, 600), new Vector2 ());
        }
   }
}
