using UnityEngine;
using System.Collections;
using System;

public class CameraAspectRatio : MonoBehaviour {

	public float adjustment = 2.54f;

    // Use this for initialization
    void Start () {

		string aspectRatio = findAspectRatio ();
		resizeCamera (this.gameObject, aspectRatio);
		//relocateCamera ();
	}

	string findAspectRatio ()
	{
		//Compute aspect ratio from Screen dimensions
		float aspRatio = (float)Screen.width / (float)Screen.height;

		//Return the value in text with 2 decimal places
		return aspRatio.ToString("F2");
	}

	void resizeCamera (GameObject cam, string aspRatio)
	{
		switch (aspRatio) {
		//case "0.67": //HVGA 320x480 2:3 portrait
		//	cam.GetComponent<Camera> ().orthographicSize = 3.83f;
		//	break;
		//case "0.60": //WVGA 480x800
		//	cam.GetComponent<Camera> ().orthographicSize = 4.27f;
		//	break;
		//case "0.56": //FWVGA 480x854
		//	cam.GetComponent<Camera> ().orthographicSize = 4.54f;
		//	break;
		//case "0.59": //WSVGA 600x1024
		//	cam.GetComponent<Camera> ().orthographicSize = 4.355f;
		//	break;
		//case "0.63": //WXGA 800X1280 10:16 portrait
		//	cam.GetComponent<Camera> ().orthographicSize = 4.09f;
		//	break;

		default:
			cam.GetComponent<Camera> ().orthographicSize = 1f / (float)Convert.ToDecimal(aspRatio) * adjustment;
			break;
		}
	}

	void relocateCamera ()
	{
		//Y axis correction, if necessar
		throw new System.NotImplementedException ();
	}
	
	// Update is called once per frame
	void Update () {
#if UNITY_EDITOR
		string aspectRatio = findAspectRatio();
        resizeCamera(this.gameObject, aspectRatio);
#endif
    }
}
