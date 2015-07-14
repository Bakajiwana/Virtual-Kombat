using UnityEngine;
using System.Collections;

public class enemyMeleeScript : MonoBehaviour 
{	
	//Damage dealt to player
	public int meleeDamage = 5; 
	
	//Spawn variables
	public Transform meleeSpawnEffect;
	
	//Score when killed
	public int meleeScore = 10;
	
	//Private variable for the playerDrone 
	private GameObject playerDrone;
	
	//Explosion variables 
	//The explosion force reference: http://docs.unity3d.com/Documentation/ScriptReference/Rigidbody.AddExplosionForce.html
	public Transform meleeExplosion;
	public Transform meleeCollisionExplosion;
	public float radius = 1000.0F;
    public float power = 150.0F;
	
	//Jammer ability Boolean
	private bool jammerBoolean = false; 
	public Transform jammerEffect;
	
	//Start Delay when spawn for player friendly reasons
	private bool delayBoolean = true;
	public float delayTimer = 1.5f;
	
	//Pick up variable 
	//Reference: http://forum.unity3d.com/threads/57562-Random-drop
	public float dropRate = 0.03f; 
	public Transform pickUpDrop;
	
		//Used to parent.....
	public Transform enemyMelee;
	
	// Use this for initialization
	void Start () 
	{		
		//Return playerDrone location
		playerDrone = GameObject.Find("playerDrone");
		
		//Create melee spawn effect
		Instantiate (meleeSpawnEffect, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Start Countdown
		delayTimer -= Time.deltaTime; 
		
		if (delayBoolean == true)
		{
			//Start Countdown
			delayTimer -= Time.deltaTime;
		}
		
		if (delayTimer <= 0f)
		{
			//Turn off delay
			delayBoolean = false;
		}
		
		//Find player by tag if he is still alive then the AI shall track him down
		GameObject[] player = GameObject.FindGameObjectsWithTag("playerDrone");
		
		//If there is one player (used to prevent null reference error) and not jammed by player's ability
		if (player.Length == 1 && jammerBoolean == false && delayBoolean == false)
		{
		//Return the direction of the player by calculating the difference between the player location and itself
		Vector3 direction_to_player = (playerDrone.transform.position - transform.position);
			
		//Use physics to move in the direction of the player
		rigidbody.AddForce(direction_to_player);
		}
	}
	
	//Use the OnCollisionEnter function to detect a collision between enemy attack and player
	void OnCollisionEnter(Collision other)
	{
		//If the melee enemy hits the player
		if (other.gameObject.tag == "playerDrone")
		{		
			//Create explosion 
			Instantiate (meleeCollisionExplosion, transform.position, transform.rotation);
			

			//Use addexplosionforce obtained from Unity References to create an explosion force when destroyed
			
			//Explosion will occur on the enemies location
       		Vector3 explosionPos = transform.position;
			
			//use Physics.OverlapSphere to calculate the radius and the colliders within in
       		Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        	
			//For each collider inside the imaginery OverlapSphere from the line above
			foreach (Collider hit in colliders) 
			{
				//If no colliders was hit, then don't get affected by explosion force
		       	if (!hit) 
				{
					continue;
				}
		        
				//If a rigidbody was hit within the radius of the explosion force
				if (hit.rigidbody)
				{
					//Get knocked back by the AddExplosionForce functions power, explosion position and radius
					hit.rigidbody.AddExplosionForce(power, explosionPos, radius);
				}
			}
			
			//Find the level manager and send an order to activate the playerDamage function
			GameObject.FindGameObjectWithTag ("levelManager").SendMessage ("applyPlayerDamage", meleeDamage);
			
			//Destroy the enemy melee drone
			Destroy (gameObject);
		}
		
		//If hit by player's bullet
		if (other.gameObject.tag == "playerBullet")
		{	
			//activate death function
			enemyMeleeDeath ();
		}
		
		//Physics problem where drone leaves game area.
		if (other.gameObject.tag == "boundary")
		{
			Destroy (gameObject);
		}
	}
	
	
	//function called upon when killed by player
	public void enemyMeleeDeath()
	{
		//Create explosion 
		Instantiate (meleeExplosion, transform.position, transform.rotation);
			
		//Find the level manager and activate score function
		GameObject.FindGameObjectWithTag ("levelManager").SendMessage ("applyScore", meleeScore);
		
		//Reference: Tim showed us how to animate camera
		//Call camera hit animation, rewind the animation first in case already playing it
		Camera.main.animation.Rewind ();
		Camera.main.animation.Play("cameraKill");
		
		//The pick up function is called upon death, chance to drop a collectable
		pickUp ();
			
		//Destroy the enemy melee drone
		Destroy (gameObject);	
	}
	
	//This function is called when the player's jammer ability jams 
	public void jammerMeleeStatus(bool _jammed)
	{
		jammerBoolean = _jammed;	
	}
	
	
	//This function is called to instantiate the jammer effect
	public void enemyJammed()
	{
		//Create the jammer Effect
		Transform jammerField = Instantiate (jammerEffect, transform.position, transform.rotation) as Transform;
		jammerField.transform.parent = enemyMelee;
	}
	
	//Pick up function is called when object is destroyed and gives a chance to drop an ability
	void pickUp()
	{
		//if random number generated is within the drop rate chance
		if(Random.Range (0f,1f) <= dropRate)
		{
			//Drop the pick up collectable for ability
			Instantiate (pickUpDrop, transform.position, transform.rotation);	
		}
	}
	
	
	//function called upon when killed shield overload
	public void meleeOverloadDeath()
	{
		//Create explosion 
		Instantiate (meleeExplosion, transform.position, transform.rotation);
			
		//Destroy the enemy melee drone
		Destroy (gameObject);	
	}
}
