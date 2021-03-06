﻿using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private PlayerScript player;
	
	public PlayerScript.Character character;
	public Elevator.Type elevator;

	// Use this for initialization
	void Start () {
		player = new PlayerScript ();
		
		//assign which character/elevator is selected
		player.character = character; //Player.Character.Mom;
		player.elevator.type = elevator; //Elevator.Type.Wood;
		//TODO: read playerprefs
		
		//Load the correct images from the folder
		LoadSprite();
	}

	void LoadSprite ()
	{
		GetComponentInChildren<Animator> ().runtimeAnimatorController =
			Resources.Load<RuntimeAnimatorController> ("Controllers/"+player.imgFileName);
	}

	// Update is called once per frame
	void Update () {

	}
}
