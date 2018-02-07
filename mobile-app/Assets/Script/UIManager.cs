using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;

public class UIManager : MonoBehaviour {
	public GameObject[] arrows;
	
	// Use this for initialization
	void Start () {
		Input.gyro.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
		foreach(GameObject arrow in arrows){
			arrow.SetActive(false);
		}
		for(int i=0;i<Database.ListLocations.locations.Count;i++){
			GameObject arrow = arrows[i];
			arrow.SetActive(true);
			Location.myLatitude = 0.001f;
			Location.myLongtitude = 0;
			
			if(Mathf.Abs(Database.ListLocations.locations[i].Longtitude-Location.myLongtitude)<=180){
				var angle = -Vector2.SignedAngle(new Vector2(-10,0),new Vector2(Database.ListLocations.locations[i].Latitude-Location.myLatitude,Database.ListLocations.locations[i].Longtitude-Location.myLongtitude));
				arrow.transform.rotation = Quaternion.Inverse(Input.gyro.attitude * new Quaternion (0, 0, 1, 0))*Quaternion.AngleAxis(angle, Vector3.forward);
			}else if(Database.ListLocations.locations[i].Longtitude-Location.myLongtitude>180){
				var angle = -Vector2.SignedAngle(new Vector2(-10,0),new Vector2(Database.ListLocations.locations[i].Latitude-Location.myLatitude,Database.ListLocations.locations[i].Longtitude-Location.myLongtitude-360));
				arrow.transform.rotation = Quaternion.Inverse(Input.gyro.attitude * new Quaternion (0, 0, 1, 0))*Quaternion.AngleAxis(angle, Vector3.forward);
				//arrow.transform.Rotate(90,0,-Vector2.SignedAngle(new Vector2(-10,0),new Vector2(Database.ListLocations.locations[i].Latitude-Location.myLatitude,Database.ListLocations.locations[i].Longtitude-Location.myLongtitude-360)));
			}else{
				var angle = -Vector2.SignedAngle(new Vector2(-10,0),new Vector2(Database.ListLocations.locations[i].Latitude-Location.myLatitude,Database.ListLocations.locations[i].Longtitude-Location.myLongtitude+360));
				arrow.transform.rotation = Quaternion.Inverse(Input.gyro.attitude * new Quaternion (0, 0, 1, 0))*Quaternion.AngleAxis(angle, Vector3.forward);
				//arrow.transform.Rotate(90,0,-Vector2.SignedAngle(new Vector2(-10,0),new Vector2(Database.ListLocations.locations[i].Latitude-Location.myLatitude,Database.ListLocations.locations[i].Longtitude-Location.myLongtitude+360)));
			}
		}
	}
}
