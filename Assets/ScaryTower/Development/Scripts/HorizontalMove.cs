using UnityEngine;
using System.Collections;
using static FloatUtils;

public class HorizontalMove : MonoBehaviour {

//	private float touchShift;
	private Touch touch;
	
	private bool bPositionCanChange;
	private bool movementCaptured;
	public int actualPosition;
	
	//Tracks position on scene
	public GameObject Left_track;
	public GameObject Center_track;
	public GameObject Right_track;
	
	//move method
	
	//speed for going right/left after player's input
	public float velocity;
	
	private Vector3[] tracksPosition;//0= Left_track.position; 1= Center_track.position; 2= Right_track.position
	public enum State{left = 0, center = 1, right = 2, idle = -1};
	public State state;

	private int mouseButton = 0;
    private Vector2 startPos;
    private Vector2 endPos;
    private float startTime;
    private float endTime;

	private float minDragToChangeTrack => GameConfigManager.Instance.gameSettings.minDragToChangeTrack;
	private float snapDistance => GameConfigManager.Instance.gameSettings.snapDistance;
	private float leftRemap => Left_track.transform.localPosition.x * 2f;
	private float rightRemap => Right_track.transform.localPosition.x * 2f;

	void Start () {
		//how much you need to drag your finger to be a valid movement
//		touchShift = (Screen.width*0.01f)*4.0f;//8.0f;//MainScript.scaleScreenPercentage(125.0f);
		
		//just falling down for default. You begin stopped
		state = State.idle;
		movementCaptured = false;
		bPositionCanChange = true;
		
		//Wich column you are
		//left = 0; center = 1; right = 2;
		actualPosition = 1;
		tracksPosition = new Vector3[3];
		tracksPosition [0] = Left_track.transform.localPosition;
		tracksPosition [1] = Center_track.transform.localPosition;
		tracksPosition [2] = Right_track.transform.localPosition;
	}

    private void OnEnable()
    {
        InputManager.Instance.OnStartTouch += OnStartTouch;
        InputManager.Instance.OnEndTouch += OnEndTouch;
    }

    private void OnDisable()
    {
        InputManager.Instance.OnStartTouch -= OnStartTouch;
        InputManager.Instance.OnEndTouch -= OnEndTouch;
    }
    
	private void OnStartTouch(Vector2 pos, float time)
    {
        startPos = pos;
        startTime = time;
		movementCaptured = false;
    }
    private void OnEndTouch(Vector2 pos, float time)
    {
        endPos = pos;
        endTime = time;

        searchTrack();

        startTime = 0;
    }

    // Update is called once per frame
    void Update () {
		if (movementCaptured) {
			moveToTrackFound ();
		}
		else if (startTime != 0)
		{
			captureInput();
        }
	}

	public void captureInput(){
        var pos = InputManager.Instance.PrimaryPosition() - startPos;
        shift(pos.x);
        captureInputPC ();
	}

	void captureInputPC ()
	{
		if (!movementCaptured || bPositionCanChange) {
			if(Input.GetKeyDown(KeyCode.LeftArrow)){
				if (actualPosition==(int)State.right) {
					state = State.center;
					movementCaptured = true;
				}
				else if (actualPosition==(int)State.center) {
					state = State.left;
					movementCaptured = true;
				}
			}
			else if(Input.GetKeyDown(KeyCode.RightArrow)){
				if (actualPosition==(int)State.left) {
					state = State.center;
					movementCaptured = true;
				}
				else if (actualPosition==(int)State.center) {
					state = State.right;
					movementCaptured = true;
				}
			}
        }
	}
	
	bool insideBoundaries () {
		return insideBoundaries (transform.localPosition.x);
	}
	
	bool insideBoundaries(float x) {
		if (x >= Left_track.transform.localPosition.x &&
		    x <= Right_track.transform.localPosition.x) {
			return true;
		}
		return false;
	}

	public void shift(float x)
	{
		Vector3 pos = tracksPosition[actualPosition];
		pos.x += x;
		if (insideBoundaries(pos.x))
		{
			transform.localPosition = pos;
		}
	}

	public void searchTrack (){
		//Compute current position of the elevator in two decimal places
		float currentPosition = Mathf.Round (transform.localPosition.x*100f)*0.01f;
		
		//Set the correct track depending on its relative position to them
		if (currentPosition >= Right_track.transform.localPosition.x / 2 && (endPos - startPos).x >= minDragToChangeTrack
            || (actualPosition == 1 && (endPos - startPos).x >= minDragToChangeTrack)
            ) {//Right_track.transform.localPosition / 2
			state = State.right;
			movementCaptured = true;
		}
		else if (currentPosition <= Left_track.transform.localPosition.x / 2 && (endPos - startPos).x <= -minDragToChangeTrack
            || (actualPosition == 1 && (endPos - startPos).x <= -minDragToChangeTrack)
            )
        {//Left_track.transform.localPosition / 2
            state = State.left;
			movementCaptured = true;
		}
		else {
            state = State.center;
			movementCaptured = true;
		}
	}
	
	void moveToTrackFound ()
	{
		//The default state of the elevator is idle.
		//When movement is detected and it needs to move somewhere,
		//Then it's either Right, Left, or Center
			
		Vector3 direction = tracksPosition[(int)state] - transform.localPosition;
		transform.Translate(direction*(Time.deltaTime*velocity),Space.World);
		float distance = direction.magnitude;
		if (distance <= snapDistance) {

			transform.localPosition = new Vector3(tracksPosition[actualPosition].x,
			                                 transform.localPosition.y,
			                                 transform.localPosition.z);
			state = State.idle;
			movementCaptured = false;
		}
		else if (distance <= snapDistance*300.0f) {
			actualPosition=(int)state;
			bPositionCanChange = true;
		}
	}
}
