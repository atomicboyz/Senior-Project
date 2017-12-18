﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Locationtest : MonoBehaviour {
	public GameObject obj;
	public Text text;
	public int maxWait = 10;
	private int i = 0;
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
				text.text = "";
				text.text = "My Location: " + Input.location.lastData.latitude + ", " + service.lastData.longitude+"\n"+i;
				Input.gyro.enabled = true;
				text.text += "\nx:"+Input.gyro.attitude.eulerAngles.x + " y:"+Input.gyro.attitude.eulerAngles.y+" z:"+Input.gyro.attitude.eulerAngles.z;
				obj.transform.rotation = Quaternion.Euler(Input.gyro.attitude.eulerAngles.x,Input.gyro.attitude.eulerAngles.y,Input.gyro.attitude.eulerAngles.z);
				yield return new WaitForSeconds(0.5f);
				i++;
			}
			
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}