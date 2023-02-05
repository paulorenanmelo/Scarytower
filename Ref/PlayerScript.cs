using UnityEngine;
using System.Collections;

public class PlayerScript {

	public enum Character{
		Dad,
		Boy,
		Girl,
		Mom
	}
	public Character character;
	public Elevator elevator;

	public string imgFileName{
		get{
			//folders are named like this on the unityProject
			return elevator.type.ToString() + character.ToString();
		}
	}

	// Use this for initialization
	public PlayerScript() {
		elevator = new Elevator ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
