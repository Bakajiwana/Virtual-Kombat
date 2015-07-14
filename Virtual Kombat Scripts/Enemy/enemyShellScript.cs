using UnityEngine;
using System.Collections;

public class enemyShellScript : MonoBehaviour 
{

	//Reference: From Tim Dawson's Programming Magic
	
	//mortar Variables
	public float mortarDamage = 10.0f;
	public float explosionDamage = 5.0f;
	public float mortarTimer = 4f;
	
	//Explosion variable
	public Transform shellExplosion;
	public float radius = 200.0F;
    public float power = 1500.0F;
	
	//Target Variable
	private GameObject mortarBeacon;
	private bool targetBoolean = true;
	
	//Playerdrone variable
	private GameObject playerDrone;
	
	//Movement variable
	public float mortarSpeed = 15.0f; 
	
	void Start()
	{
		Destroy (gameObject,5);
	}
	
	
	// Update is called once per frame
	void Update () 
	{		
		//Countdown to when mortal shell destroys itself and instantiate an explosion
		mortarTimer -= Time.deltaTime;
		
		//Find player and beacons with tag and shove it into an array
		GameObject[] playerObject = GameObject.FindGameObjectsWithTag("playerDrone");
		
		//When timer runs out
		if(mortarTimer <= 0 && targetBoolean == true && playerObject.Length >= 1)
		{
			//Send message to player drone's playerStatusScript to drop a mortar beacon
			GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("dropBeacon");
			
			//Return mortar beacon location
			mortarBeacon = GameObject.FindGameObjectWithTag("mortarBeacon");
			
			
			targetBoolean = false;
		}
		
		if(targetBoolean == false)
		{
			//Move towards marker
			float step = mortarSpeed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, mortarBeacon.transform.position, step);
		}
	}
	
	//Collision function when the mortar hits something 
	void OnTriggerEnter (Collider other)
	{
		//If the mortar collides with anything
		if (other.gameObject.tag == "playerDrone")
		{		
			//Instantiate an explosion
			Instantiate (shellExplosion, transform.position, transform.rotation);
			
			//Find the level manager and activate damage function
			GameObject.FindGameObjectWithTag ("levelManager").SendMessage ("applyPlayerDamage", mortarDamage);
			AreaOfEffect ();
			
			//Destroy mortar shell
			Destroy (gameObject);
		}
		
		//The player can shoot the mortar shells
		if (other.gameObject.tag == "mortarBeacon")
		{
			//Instantiate an explosion
			Instantiate (shellExplosion, transform.position, transform.rotation);
			
			//Area of effect creates the explosion force and hurts the player if within radius
			AreaOfEffect ();
			
			//Destroy itself
			Destroy(gameObject);
		}
	}
	
	//Create an area of effect explosion
	//Using Vector3.Distance
	//
	//This function is called to produce an area of effect, an explosion force and radius
	void AreaOfEffect()
	{
		//Tweak this code by using Rigidbody.AddExplosionForce to blow the enemies away
		//and also hurts the player 
       	Vector3 explosionPos = transform.position;
		
		//Return all colliders into an array within the OverlapSphere radius
       	Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
		
		
		//For each collider inside the OverlapSphere, get pushed back by the AddExplosionForce
		foreach (Collider hit in colliders) 
		{
			if (!hit) 
			{
				continue;
			}
	            	
			if (hit.rigidbody)
			{
				hit.rigidbody.AddExplosionForce(power, explosionPos, radius);
			}
		}
		
		//Return the playerDrone position
		playerDrone = GameObject.Find("playerDrone");
		
		//To prevent a null reference error make sure that when the player is dead this script will stop looking for player
		GameObject[] player = GameObject.FindGameObjectsWithTag("playerDrone");
		
		//This prevents a null reference error
		if (player.Length == 1)
		{
			//The way the radius in this function works is using Vector3.Distance to identify the player's distance compared to the mortar shells explosion
			float dist = Vector3.Distance (playerDrone.transform.position, transform.position);
			
			//if the player is within the distance radius
			if(dist < radius)
			{
				//Find the level manager and activate damage function
				GameObject.FindGameObjectWithTag ("levelManager").SendMessage ("applyPlayerDamage", mortarDamage);
			}	
		}
	}
}
