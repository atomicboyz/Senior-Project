using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour {
	public Text text;
	public GameObject plane;
	private bool camAvailable;
	private WebCamTexture cameraTexture;
	private Texture defaultBackground;

	public RawImage background;
	public AspectRatioFitter fit;
	public bool frontFacing;
	private int length;
	// Use this for initialization
	void Start () {
		//defaultBackground = background.texture;
		WebCamDevice[] devices = WebCamTexture.devices;
		length = devices.Length;
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

		cameraTexture.Play (); // Start the camera
		plane.GetComponent<Renderer>().material.mainTexture = cameraTexture;
		background.texture = cameraTexture; // Set the texture

		camAvailable = true; // Set the camAvailable for future purposes.
		Screen.autorotateToLandscapeLeft = false;
		Screen.autorotateToLandscapeRight = false;
	}
	
	// Update is called once per frame
	void Update () {
		//text.text += "\ndevicesLength:" + length;
		if (!camAvailable)
			return;
		
		float ratio = (float)cameraTexture.width / (float)cameraTexture.height;
		fit.aspectRatio = ratio; // Set the aspect ratio

		float scaleY = cameraTexture.videoVerticallyMirrored ? -1f : 1f; // Find if the camera is mirrored or not
		background.rectTransform.localScale = new Vector3(1f, scaleY, 1f); // Swap the mirrored camera

		int orient = -cameraTexture.videoRotationAngle;
		background.rectTransform.localEulerAngles = new Vector3(0,0, orient);
}
}
