using UnityEngine;
using System.Collections;
using System.Data.Common;

public class Player : STMonoBehaviour {

	private PlayerScript player;
	
	public PlayerScript.Character character;
	public Elevator.Type elevator;
	private HorizontalMove horizontalMove;
	private IdleMove idleMove;
    private Animator anim;
    private bool isPlaying = true;

    public override void Init () {
		base.Init();

		player = new PlayerScript ();
		
		//assign which character/elevator is selected
		player.character = character; //Player.Character.Mom;
		player.elevator.type = elevator; //Elevator.Type.Wood;
		//TODO: read playerprefs
		
		//Load the correct images from the folder
		LoadSprite();

		// animator has a default state which is playing on awake
		isPlaying = true;
    }

    private void Update()
    {
        if(Runtime.state != STState.Playing && isPlaying)
		{
			if (verbose) Debug.Log("[Player]: StopPlayback");
			SetStatus(PlayerStatus.Paused);
		}
		else if (Runtime.state == STState.Playing && !isPlaying)
		{
			if (verbose) Debug.Log("[Player]: StartPlayback");
			SetStatus(PlayerStatus.Animated);
        }
    }

	private enum PlayerStatus { Paused, Animated }
	private void SetStatus(PlayerStatus status)
	{
		switch (status)
		{
			case PlayerStatus.Paused:
				SetEnabled(false);
                break;
			case PlayerStatus.Animated:
                SetEnabled(true);
                break;
			default:
				break;
		}
	}

	private void SetEnabled(bool enabled)
	{
        anim.enabled = enabled;
        horizontalMove.enabled = enabled;
        idleMove.enabled = enabled;
        isPlaying = enabled;
    }

    void LoadSprite ()
	{
		anim = GetComponentInChildren<Animator>(true);
        idleMove = GetComponentInChildren<IdleMove>(true);
        horizontalMove = GetComponentInChildren<HorizontalMove>(true);
		anim.runtimeAnimatorController =
			Resources.Load<RuntimeAnimatorController>("Controllers/" + player.imgFileName);
	}
}
