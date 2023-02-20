using UnityEngine;
using System.Collections;
using static FloatUtils;

public class HorizontalMove : MonoBehaviour
{
	[Tooltip("How many seconds should the game wait after a touch to go to nearest track?\nThis is to prevent sudden jumps.")]
	[SerializeField] private float minSecondsToMoveToTrack = 0.15f;

    private Touch touch;
	
	private bool bPositionCanChange;
	private bool movementCaptured;
    private float movementCaptureTime;
    private float inertia;
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
    private Vector3 prevPos;
    private float startTime;
    private float endTime;

    private bool verbose => GameConfigManager.Instance.gameSettings.logVerbose;
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
		movementCaptureTime = 0;

        //Wich column you are
        //left = 0; center = 1; right = 2;
        actualPosition = 1;
		tracksPosition = new Vector3[3];
		tracksPosition [0] = Left_track.transform.localPosition;
		tracksPosition [1] = Center_track.transform.localPosition;
		tracksPosition [2] = Right_track.transform.localPosition;
		transform.localPosition = Center_track.transform.localPosition;
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
    }

    // Update is called once per frame
    void Update () {
		if (movementCaptured) {
			// move linearly to the nearest track as soon as inertia has reached velocity
			if (movementCaptureTime >= minSecondsToMoveToTrack)
			{
				moveToTrackFound();
			}
			// add inertia as soon as it starts going to the nearest track
			else if (movementCaptureTime == 0)
			{
				inertia = Mathf.Clamp((transform.localPosition - prevPos).x * -100f, -1f, 1f);
				if (verbose) Debug.Log("[HorizontalMove]: set inertia to " + inertia);
            }
			// apply inertia to position until it's smaller than snap value
			if (Mathf.Abs(inertia) > snapDistance)
			{
				var pos = transform.localPosition;
				pos.x += inertia * Time.deltaTime * velocity;
				transform.localPosition = pos;
				inertia *= 0.9f;
				if (Mathf.Abs(inertia) <= 0.01f) inertia = 0;
                if (verbose) Debug.Log("[HorizontalMove]: change inertia to " + inertia);
            }
            movementCaptureTime += Time.deltaTime;
		}
		else if (startTime != 0 || movementCaptureTime < minSecondsToMoveToTrack)
		{
			captureInput();
        }
	}

	private void SetFlagToGoToNearestTrack()
	{
		movementCaptureTime = 0;
		movementCaptured = true;
        startTime = 0;
    }

    public void captureInput(){
		Vector2 pos = InputManager.Instance.PrimaryPosition() - startPos;
        shift(pos.x);
        captureInputPC ();
	}

	void captureInputPC ()
	{
		if (!movementCaptured || bPositionCanChange) {
			if(Input.GetKeyDown(KeyCode.LeftArrow)){
				if (actualPosition==(int)State.right) {
					state = State.center;
					SetFlagToGoToNearestTrack();
                }
				else if (actualPosition==(int)State.center) {
					state = State.left;
                    SetFlagToGoToNearestTrack();
                }
			}
			else if(Input.GetKeyDown(KeyCode.RightArrow)){
				if (actualPosition==(int)State.left) {
					state = State.center;
                    SetFlagToGoToNearestTrack();
                }
				else if (actualPosition==(int)State.center) {
					state = State.right;
                    SetFlagToGoToNearestTrack();
                }
			}
        }
	}

	bool insideBoundaries()
	{
		return insideBoundaries(transform.localPosition.x);
	}

	bool insideBoundaries(float x)
	{
		return x > Left_track.transform.localPosition.x
			&& x < Right_track.transform.localPosition.x;
	}

	public void shift(float x)
	{
		Vector3 pos = tracksPosition[actualPosition];
		pos.x += x;
		if (pos.x != Left_track.transform.localPosition.x
			&& pos.x != Right_track.transform.localPosition.x
			&& pos.x != Center_track.transform.localPosition.x)
			prevPos = pos;
		if (insideBoundaries(pos.x))
		{
			transform.localPosition = pos;
            if (verbose) Debug.Log("[HorizontalMove]: shift to " + prevPos.ToString());
        }
		else if (IsInTheMiddle && !bPositionCanChange)
		{
			pos.x = Mathf.Clamp(pos.x, Left_track.transform.localPosition.x, Right_track.transform.localPosition.x);
            transform.localPosition = pos;
            if (verbose) Debug.Log("[HorizontalMove]: clamped to " + prevPos.ToString());
        }
    }

	private bool IsInTheMiddle => actualPosition == 1;

    public void searchTrack (){
		//Compute current position of the elevator in two decimal places
		float currentPosition = Mathf.Round (transform.localPosition.x*100f)*0.01f;
		float displacement = (endPos - startPos).x;

        //Set the correct track depending on its relative position to them
        /*if (currentPosition >= Right_track.transform.localPosition.x / 2
            ) {
			state = State.right;
            SetFlagToGoToNearestTrack();
        }
        else if (currentPosition <= Left_track.transform.localPosition.x / 2 
            ) {
            state = State.left;
            SetFlagToGoToNearestTrack();
        }
		else */if (displacement >= minDragToChangeTrack)
		{
			int add = displacement >= Right_track.transform.localPosition.x ? 2 : 1;
			state = (State)Mathf.Clamp(actualPosition + add, 0, 2);
            SetFlagToGoToNearestTrack();
        }
		else if (displacement <= -minDragToChangeTrack)
		{
			int add = displacement <= Left_track.transform.localPosition.x ? 2 : 1;
            state = (State)Mathf.Clamp(actualPosition - add, 0, 2);
            SetFlagToGoToNearestTrack();
        }
        else {
            state = State.center;
            SetFlagToGoToNearestTrack();
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
            if (verbose) Debug.Log("[HorizontalMove]: moveToTrackFound nearby snap");

            transform.localPosition = new Vector3(tracksPosition[actualPosition].x,
			                                 transform.localPosition.y,
			                                 transform.localPosition.z);
			state = State.idle;
			movementCaptured = false;
		}
		else { // if (distance <= snapDistance*300.0f) {
			//if (verbose) Debug.Log("[moveToTrackFound] far");
			actualPosition=(int)state;
			bPositionCanChange = true;
		}
	}
}
