using UnityEngine;
using System.Collections;

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
    		else
				Application.LoadLevel("menu");
        }
	}
}
