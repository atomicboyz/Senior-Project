using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Location : MonoBehaviour {
	public int maxWait = 10;
	public static float myLatitude;
	public static float myLongtitude;

	private LocationService service;
	// Use this for initialization
	void Start () {
		StartCoroutine (getLocation ());
	}
	
	IEnumerator getLocation(){
		service = Input.location;
		if (!service.isEnabledByUser) {
			Debug.Log("Location Services not enabled by user");
			yield break;
		}
		service.Start();
		while (service.status == LocationServiceStatus.Initializing && maxWait > 0) {
			yield return new WaitForSeconds(1);
			maxWait--;
		}
		if (maxWait < 1){
			Debug.Log("Timed out");
			yield break;
		}
		if (service.status == LocationServiceStatus.Failed) {
			Debug.Log("Unable to determine device location");
		} else {
			while(service.status != LocationServiceStatus.Failed){
				service.Start();
				myLatitude = service.lastData.latitude;
				myLongtitude = service.lastData.longitude;
				yield return new WaitForSeconds(0.5f);
			}
			
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
