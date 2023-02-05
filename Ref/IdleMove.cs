using UnityEngine;
using System.Collections;

public class IdleMove : MonoBehaviour {
	
	//Idle Rotation Movement
	private bool rotateRight;
	private float rotation;
	public float idleRotationSpeed;
	public float idleRotationLimit;
	
	//Idle Translation Movement
	private float angle;
	public float idleMovementSpeed;
	public float idleMovementLimit;
	
	// Use this for initialization
	void Start () {
		//
		rotation = 0;
		rotateRight = true;
		
		//Angle to extract sin function to elevator idle movement
		angle = 0;//scans from 0 to 360
		//Limit of Idle rotation movement in angles
		//idleMovementLimit = 1.5f;
		//Speed of rotation idle movement
		//idleMovementSpeed = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
		idleMovement();
		
		//APPLY MOVEMENT TO THE ROTATION VALUE
		if (rotateRight) {
			rotation+= idleRotationSpeed;
		}
		else {
			rotation-= idleRotationSpeed;
		}
		//APPLY A LIMIT TO THE ROTATION VALUE
		if (rotation>idleRotationLimit) {
			rotateRight = false;
		}
		else if(rotation < -idleRotationLimit) {
			rotateRight = true;
		}
		//APPLY THE ROTATION VALUE TO THE TRANSFORMATION OF THE OBJECT THAT WILL HAVE THIS CLASS
		transform.rotation = Quaternion.AngleAxis(rotation, Vector3.forward);
	}
	
	void idleMovement (){
		//CALLED EVERY FRAME
		//increment the angle (from 0 to 360)
		angle+= idleMovementSpeed;
		if (angle>=360.0f) {
			angle = 0.0f;
		}
		//compute the sin of the angle
		float y = Mathf.Sin (angle);
		//apply the sin function through time to the position of the object that have this class
		transform.position = new Vector3(transform.position.x,
			transform.position.y+(y*Time.deltaTime*idleMovementLimit),
			transform.position.z);
	}
	
	public void RightForce(){
		//idea: implement a physics force to change this idle movement
		//that interferes on the stabilization of the lift
	}
}