using UnityEngine;
using System.Collections;

public class playerMovementScript : MonoBehaviour 
{
	//Movement Variables
	public float playerMoveSpeed = 1000.0f; 
	Vector3 playerVelocity; //Main variable that controls movement using physics
	
	//Rotate Variables
	public float playerRotateSpeed = 40.0f; 
	
	
	// Use this for initialization
	void Start () 
	{

	}
	
	
	// Update is called once per frame
	void Update () 
	{
		//Create an input variable where the basic movement keys and joysticks are called
		Vector3 inputVector = new Vector3(Input.GetAxis ("Horizontal"), 0.0f, Input.GetAxis ("Vertical"));
		
		//Assign the speed and input variable into the movement variable
		playerVelocity = inputVector * playerMoveSpeed;
		
		//Calculate the 3D position of the mouse cursor and rotate the ship to face it smoothly over time
		Vector3 playerMouseAim = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.y - transform.position.y));
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (playerMouseAim - transform.position), Time.deltaTime * playerRotateSpeed);
	
		//Joystick rotation to rotate the player drone using a joystick
		//Vector3 controllerRotate = new Vector3 (Input.GetAxis ("Right Joystick X"), 0.0f, Input.GetAxis ("Right Joystick Y"));
		//float angle = Mathf.Atan2 (controllerRotate.x, controllerRotate.z) * Mathf.Rad2Deg;
		//transform.Rotate (0.0f, angle, 0.0f);
	}
	
	
	// FixedUpdate is called once per physics update
	void FixedUpdate()
	{
		//Use Rigidbody to control an objects position through physics simulation
		rigidbody.AddForce (playerVelocity);
	}
	
	public void dashMovement(float _movementSpeed)
	{
		playerMoveSpeed = _movementSpeed;
	}
}
