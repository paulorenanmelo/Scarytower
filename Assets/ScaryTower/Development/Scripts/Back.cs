using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Back : MonoBehaviour {
	
	public bool quit = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape))
        {
			if (quit)
				Application.Quit();
    		else if(SceneManager.GetActiveScene().name != "Menu")
			{
				SceneManager.LoadScene("Menu");
			}
			else
			{
				GameConfigManager.Instance.gameRuntime.goToState = STState.StartMenu;
			}
        }
	}
}
