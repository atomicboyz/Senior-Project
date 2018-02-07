using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Database : MonoBehaviour {
	public static Locations ListLocations;
	public static string API_URL = "http://161.200.203.190:5000/";
	
	public Toggle restaurant;
	public Toggle toilet;
	public Toggle ticket;
	public Toggle souvenir;
	// Use this for initialization
	
	[Serializable]
	public class Locations
	{
		public List<Location> locations;
	}
	[Serializable]
	public class Location{
		public string id;
		public string Name;
		public string Category;
		public float Latitude;
		public float Longtitude;
		public float Rating;
		public List<string> RatedUser;
		public string URLPic;
		public List<string> Comments;
		public Report Reports;
	}
	[Serializable]
	public class Report{
		public string Type;
		public string Detail;
		public string Username;
	}
	void Start () {
		ListLocations = new Locations();
		StartCoroutine(GetLocations());
	}
	IEnumerator GetLocations() {
		while(true)
        {
			string url = API_URL+"search=0,0";
			if(!(restaurant.isOn && toilet.isOn && ticket.isOn && souvenir.isOn)) {
				url+= "&filter=";
				if(restaurant.isOn) url+= "Restaurant,";
				if(toilet.isOn) url += "Toilet,";
				if(ticket.isOn) url += "TicketSeller,";
				if(souvenir.isOn) url += "SouvenirShop,";
				url = url.Remove(url.Length-1);
			}
			UnityWebRequest www = UnityWebRequest.Get(url);
			yield return www.SendWebRequest();
 
			if(www.isNetworkError || www.isHttpError) {
				Debug.Log(www.error);
			}
			else {
				// Show results as text
				ListLocations = JsonUtility.FromJson<Locations>(www.downloadHandler.text);
			}
			yield return new WaitForSeconds(1);
		}
        
    }
	// Update is called once per frame
	void Update () {
		
	}
}
