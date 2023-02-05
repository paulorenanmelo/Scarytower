using UnityEngine;
using System.Collections;

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
	private const float snapDistance = 0.0333f;
	
	//speed for going right/left after player's input
	public float velocity;
	
	private Vector3[] tracksPosition;//0= Left_track.position; 1= Center_track.position; 2= Right_track.position
	public enum State{left = 0, center = 1, right = 2, idle = -1};
	public State state;

//	private Touch initialTouch, intermedTouch, finalTouch;
	private float initialTime;//, intermedTime, finalTime;
//	public float timeLimit1, timeLimit2, displacementLimit1, displacementLimit2;
	private float displacement;

	// Use this for initialization
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
		tracksPosition [0] = Left_track.transform.position;
		tracksPosition [1] = Center_track.transform.position;
		tracksPosition [2] = Right_track.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		captureInput();
		if (movementCaptured) {
			moveToTrackFound ();
		}
	}

	public void captureInput(){
		if (!movementCaptured || bPositionCanChange) {
			if (Input.touchCount > 0) {
				Touch t = Input.GetTouch(0);
				switch (t.phase) {
				case TouchPhase.Began:
	//				initialTouch = t;
					initialTime = Time.time;
					break;
				case TouchPhase.Moved:
					float x = t.deltaPosition.x;
					displacement+= x;
					move (t.deltaPosition.x);
					break;
				case TouchPhase.Canceled:
					searchTrack();
					displacement = 0;
					initialTime = 0;
					break;
				case TouchPhase.Ended:
					searchTrack();
					initialTime = 0;
					displacement = 0;
					break;
				default:
					break;
				}
			}
		}
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
	
	bool finishMovement ()
	{
		//return true if	Time limit is exceeded, displacement limit is exceeded
		/*if (Time.time - initialTime > timeLimit1) {
			return true;
		}*/
		return false;
	}
	
	bool insideBoundaries () {
		return insideBoundaries (transform.position.x);
	}
	
	bool insideBoundaries(float x) {
		if (x >= Left_track.transform.position.x &&
		    x <= Right_track.transform.position.x) {
			return true;
		}
		return false;
	}
	
	public void move(float x){
		if (insideBoundaries(transform.position.x + (x*0.014f))) {
			transform.Translate(x*0.014f, 0, 0);
		}
	}
	
	public void searchTrack (){
		//Compute current position of the elevator in two decimal places
		float currentPosition = Mathf.Round (transform.position.x*100)*0.01f;
		
		//Set the correct track depending on its relative position to them
		if (currentPosition >= Right_track.transform.position.x / 2) {//Right_track.transform.position / 2
			state = State.right; movementCaptured = true;
		}
		else if (currentPosition <= Left_track.transform.position.x / 2) {//Right_track.transform.position / 2
			state = State.left; movementCaptured = true;
		}
		else {
			state = State.center; movementCaptured = true;
		}
	}
	
	void moveToTrackFound (float displacement){
		//if displacement is positive, direction is right, otherwise is left?
	}
	
	void moveToTrackFound ()
	{
		//The default state of the elevator is idle.
		//When movement is detected and it needs to move somewhere,
		//Then it's either Right, Left, or Center
			
		Vector3 direction = tracksPosition[(int)state] - transform.position;
		transform.Translate(direction*(Time.deltaTime*velocity),Space.World);
		float distance = direction.magnitude;
		if (distance <= snapDistance) {

			transform.position = new Vector3(tracksPosition[actualPosition].x,
			                                 transform.position.y,
			                                 transform.position.z);
			state = State.idle;
			movementCaptured = false;
		}
		else if (distance <= snapDistance*300.0f) {
			actualPosition=(int)state;
			bPositionCanChange = true;
		}
	}
}
