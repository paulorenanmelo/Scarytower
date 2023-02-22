using UnityEngine;
using System.Collections;
using System;

public class CameraAspectRatio : STMonoBehaviour
{
	[Serializable] public enum AlignmentModeTypes { alignWithTopEdge, alignWithBottomEdge, alignCentered }

	private Camera cam;
	private string aspectRatio;
    [SerializeField] private float adjustment = 2.54f;
	[SerializeField] private AlignmentModeTypes alignmentMode = AlignmentModeTypes.alignWithTopEdge;


    public float Adjustment { get => adjustment; set => adjustment = value; }
    public AlignmentModeTypes AlignmentMode { get => alignmentMode; set => alignmentMode = value; }

    public override void Init () {

		base.Init();
        string _aspectRatio = findAspectRatio();
        if (_aspectRatio == aspectRatio) return;
        aspectRatio = _aspectRatio;
        resizeCamera(this.gameObject, aspectRatio);
        relocateCamera();
    }

    public override void Tick()
    {
        string _aspectRatio = findAspectRatio();
        if (_aspectRatio == aspectRatio) return;
		aspectRatio = _aspectRatio;
        resizeCamera(this.gameObject, aspectRatio);
        relocateCamera();
        if (verbose) Debug.Log("[CameraAspectRatio]: changed to " + aspectRatio);
		aspectRatioChanged?.Invoke();
    }

    string findAspectRatio ()
	{
		//Compute aspect ratio from Screen dimensions
		float aspRatio = (float)Screen.width / (float)Screen.height;

		//Return the value in text with 2 decimal places
		return aspRatio.ToString("F2");
	}

	void resizeCamera (GameObject _cam, string aspRatio)
	{
		if(aspRatio.Length > 0)
		{
			if(cam == null) cam = _cam.GetComponent<Camera>();
            cam.orthographicSize = 1f / (float)Convert.ToDecimal(aspRatio) * adjustment;
		}
	}

    /// <summary>
    /// Y axis correction, if necessary so that the bg starts at Y = 0 downwards, and the character should offset accordingly
    /// </summary>
    void relocateCamera ()
	{
		var pos = cam.transform.position;
		switch (alignmentMode)
		{
			case AlignmentModeTypes.alignWithTopEdge:
                pos.y = -cam.orthographicSize;
                break;
			case AlignmentModeTypes.alignWithBottomEdge:
                pos.y = cam.orthographicSize;
                break;
			case AlignmentModeTypes.alignCentered:
			default:
                // Camera is centered by default, but let's set it here in case this is changed in runtime
				pos.y = 0;
                break;
		}
		cam.transform.position = pos;
	}
}
