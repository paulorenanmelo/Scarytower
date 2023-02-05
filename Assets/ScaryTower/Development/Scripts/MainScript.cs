using UnityEngine;
using System.Collections;

public class MainScript : MonoBehaviour {

    public static float FXVolume;
	public static float MusicVolume;
    public static float centerScreenX;
    public static float centerScreenY;
	public static float cameraSize;
	
	public static int bgCount;
	public static float gameSpeed;
	
	public static float coinProbability;
	
	public static Vector2 enemySize;
	public static int enemyCount;
	public static float enemySpacing;
	public static float enemySpacingRangomRange;
	
	public static Vector3 leftPosition;
	public static Vector3 centerPosition;
	public static Vector3 rightPosition;
    
	public static void LoadGame(){
        centerScreenX = Screen.width / 2;
        centerScreenY = Screen.height / 2;
        MusicVolume = PlayerPrefs.GetFloat("musicVolume");
        FXVolume = PlayerPrefs.GetFloat("fxVolume");
	}

    public static void SaveGame(){
        PlayerPrefs.SetFloat("musicVolume", MusicVolume);
        PlayerPrefs.SetFloat("fxVolume", FXVolume);
    }
	
	/// <summary>
	/// Get a GameObject and return its transform.localScale already scaled to the 100% of the screen
	/// </summary>
	/// <param name='objectToScale'>
	/// GameObject to scale. (ex.: background)
	/// </param>
	public static Vector3 scaleToScreen(GameObject objectToScale){
		Vector3 outputSize = objectToScale.GetComponent<Collider>().bounds.size;//assign Z size (constant)
		float aspectRatio = Camera.main.orthographicSize / ((float)Screen.height / (float)Screen.width);
		outputSize.y= Camera.main.orthographicSize/aspectRatio;// * 2.0f;
		outputSize.x= outputSize.y* ((float)Screen.width / (float)Screen.height);
		
		return outputSize;
	}
	
	/// <summary>
	/// Scales an object relatively to actual screen with both X and Y freedom.
	/// </summary>
	/// <returns>
	/// The transform.localScale Vector3.
	/// </returns>
	/// <param name='objectToScale'>
	/// GameObject to scale.
	/// </param>
	/// <param name='_percentageXofScreen'>
	/// Percentage of screen width.
	/// </param>
	/// <param name='_percentageYofScreen'>
	/// _percentage of screen height.
	/// </param>
	public static Vector3 scaleFromScreen(GameObject objectToScale, float _percentageXofScreen, float _percentageYofScreen){
		Vector3 outputSize = objectToScale.GetComponent<Collider>().bounds.size;//assign Z size (constant)
		float aspectRatioCamera = Camera.main.orthographicSize / ((float)Screen.height / (float)Screen.width);
		outputSize.y= Camera.main.orthographicSize/aspectRatioCamera;
		outputSize.x= outputSize.y* ((float)Screen.width / (float)Screen.height);
		
		outputSize.x *= _percentageXofScreen / 100.0f;
		outputSize.y *= _percentageYofScreen / 100.0f;
		return outputSize;
	}
	
	/// <summary>
	/// Scales an object relatively to actual screen with X percentage and 1:2 proportion to Y.
	/// </summary>
	/// <returns>
	/// The transform.localScale Vector3.
	/// </returns>
	/// <param name='objectToScale'>
	/// GameObject to scale.
	/// </param>
	/// <param name='_percentageXofScreen'>
	/// Percentage of screen width.
	/// </param>
	public static Vector3 scaleFromScreen(GameObject objectToScale, float _percentageXofScreen){
		Vector3 outputSize = objectToScale.GetComponent<Collider>().bounds.size;//assign Z size (constant)
		float aspectRatioCamera = Camera.main.orthographicSize / ((float)Screen.height / (float)Screen.width);
		outputSize.y= Camera.main.orthographicSize/aspectRatioCamera;
		outputSize.x= outputSize.y* ((float)Screen.width / (float)Screen.height);
		
		outputSize.x *= _percentageXofScreen / 100.0f;
		outputSize.y = outputSize.x * 2.0f;
		return outputSize;
	}
	
	/// <summary>
	/// Gets a percentage float and transform into world coordinates number.
	/// </summary>
	/// <param name='percentage'>
	/// Percentage to scale relatively to actual screen.
	/// </param>
	public static float scaleScreenPercentage(float percentage){
		float outputSize;
		float aspectRatio = ((float)Screen.height / (float)Screen.width);
		outputSize = (Camera.main.orthographicSize/aspectRatio) *
			(aspectRatio/100.0f*percentage);
		return outputSize;
	}
}