using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour {
	public GameObject plane;
	private bool camAvailable;
	public static WebCamTexture cameraTexture;
	private Quaternion baseRotation;
	//private Texture defaultBackground;

	//public RawImage background;
	//public AspectRatioFitter fit;
	public bool frontFacing;
	//private int length;
	// Use this for initialization
	void Start () {
		//defaultBackground = background.texture;
		WebCamDevice[] devices = WebCamTexture.devices;
		if (devices.Length == 0)
			return;

		for (int i = 0; i < devices.Length; i++)
		{
			var curr = devices[i];

			if (curr.isFrontFacing == frontFacing)
			{
				cameraTexture = new WebCamTexture(curr.name, Screen.width, Screen.height);
				break;
			} else {
				cameraTexture = new WebCamTexture(curr.name, Screen.width, Screen.height);
			}
		}	

		if (cameraTexture == null)
			return;

		
		plane.GetComponent<Renderer>().material.mainTexture = cameraTexture;
		baseRotation = plane.transform.rotation;
		//background.texture = cameraTexture; // Set the texture
		cameraTexture.Play (); // Start the camera
		camAvailable = true; // Set the camAvailable for future purposes.
	}
	
	// Update is called once per frame
	void Update () {
		if (!camAvailable)
			return;
		plane.transform.rotation = baseRotation * Quaternion.AngleAxis(cameraTexture.videoRotationAngle, Vector3.up);
		float ratio = (float)cameraTexture.width / (float)cameraTexture.height;
		//fit.aspectRatio = ratio; // Set the aspect ratio
		
		var width = Camera.main.orthographicSize * 2.0f/10.0f * Screen.width/Screen.height;
		var height = width*ratio;
		// plane.transform.localScale = new Vector3(height,0.1f,width);
		//plane.transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(0,Screen.height/2,Camera.main.nearClipPlane));
		float scaleY = cameraTexture.videoVerticallyMirrored ? -0.1f : 0.1f; // Find if the camera is mirrored or not
		plane.transform.localScale = new Vector3(height, scaleY, width); // Swap the mirrored camera

		// int orient = -cameraTexture.videoRotationAngle;
		// plane.transform.localEulerAngles = new Vector3(-90,0, orient);
}
}
