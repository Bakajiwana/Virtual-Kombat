using UnityEngine;
using System.Collections;

public class enemyMortarScript : MonoBehaviour 
{
	//The Mortar enemy will be tagged as playerGunner because they have similar agencies
	
	//Variable used to return players location
	private GameObject playerLocation;
	
	//Spawn variables
	public Transform mortarSpawnEffect;
	
	//Scoring variable when killed by player
	public int mortarScore = 10;
	
	//enemyBullet node is where bullets are spawned from
	public Transform enemyBulletNode;

	//Mortar Muzzle blast variable
	public Transform enemyMortarBlast;
	
	//Used for parenting.
	public Transform enemyMortarDrone;
	
	//The enemyMortar is a prefab created when the fire button is pressed
	public Transform enemyMortar;
	
	//Explosion variable
	public Transform mortarExplosion;	
	
	//Mortar speed
	public float mortarSpeed = 50f; 
	
	//Jammer Variable
	private bool jammerBoolean = false;
	public Transform jammerEffect;
	
	//Movement Variable
	public float speed = 1f;
	
	//Pick up variable 
	//Reference: http://forum.unity3d.com/threads/57562-Random-drop
	public float dropRate = 0.10f; 
	public Transform pickUpDrop;
	

	// Use this for initialization
	void Start () 
	{
		//Start the enumerator function for the enemies shooting loops
		StartCoroutine (shoot ());
		
		//Create spawn effect
		Instantiate(mortarSpawnEffect, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () 
	{	
		//Find player with tag and shove it into an array
		GameObject[] playerObject = GameObject.FindGameObjectsWithTag("playerDrone");
		
		//Aim at the player if there is a player in the scene (to prevent null reference error when player dies) and if not jammed
		if(playerObject.Length  == 1 && jammerBoolean == false)
		{
			//Find player object location
			playerLocation = GameObject.Find ("playerDrone");
			//Look at player object
			transform.LookAt(playerLocation.transform);
			
			//Move towards player
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, playerLocation.transform.position, step);
		}
	}
	
	//Function is called when player's jammer ability is activated
	public void jammerGunnerStatus(bool _jammed)
	{
		jammerBoolean = _jammed;
	}
	
	
	//This function is called to instantiate the jammer effect
	public void enemyJammed()
	{
		//Create the jammer Effect
		Transform jammerField = Instantiate (jammerEffect, transform.position, transform.rotation) as Transform;
		jammerField.transform.parent = enemyMortarDrone;
	}
	
	//Create a looping process where the enemy shoots bullet
	/*
	  REFERENCE: This method I took from Aldo Naletto and just tweaked the variables to
	  fit my game. All credit goes to him. Website: http://answers.unity3d.com/users/11109/aldonaletto.html
	*/
	IEnumerator shoot()
	{
		while(true) //loops forever
		{ 
			//Find player with tag and shove it into an array
			GameObject[] playerObject = GameObject.FindGameObjectsWithTag("playerDrone");
			if(playerObject.Length == 1 && jammerBoolean == false) //if there is 1 player in the scene
			{
				//loop through the children of the bulletnode and create a bullet for each node
				foreach (Transform child in enemyBulletNode)
				{
					//create a Mortar shell, assign its transform to a variable, then set the new bullet's firevector
					Transform mortarShell = Instantiate(enemyMortar, child.position, child.rotation) as Transform;
					mortarShell.rigidbody.AddForce (transform.up * mortarSpeed, ForceMode.Impulse);
					
					//Create Mortar Blast (or some huge muzzle flash for mortars)
					Transform mortarBlast = Instantiate (enemyMortarBlast, transform.position + new Vector3(0,4,0),transform.rotation) as Transform;
					mortarBlast.transform.parent = enemyMortarDrone;
				}
				//Each enemyGunner will shoot according to timer
				yield return new WaitForSeconds(2.5f);
			} else { 
				//if no enemy then return nothing and the while loop will become false.
				yield return null;
			}
		}
	}
	
	//Collision function is called when object collides on something
	void OnCollisionEnter (Collision other)
	{
		//If hit by player's bullet
		if (other.gameObject.tag == "playerBullet")
		{	
			//Activate enemyMortarDeath function
			enemyMortarDeath ();
		}
		
		//Physics problem where drone leaves game area.
		if (other.gameObject.tag == "boundary")
		{
			Destroy (gameObject);
		}
	}
	
	
	//This function is called when Mortar is killed by player
	public void enemyMortarDeath()
	{
		//Create explosion 
		Instantiate (mortarExplosion, transform.position, transform.rotation);
			
		//Find the level manager and apply score for killing mortar enemy
		GameObject.FindGameObjectWithTag ("levelManager").SendMessage ("applyScore", mortarScore);
		
		//Reference: Tim showed us how to animate camera
		//Call camera hit animation, rewind the animation first in case already playing it
		Camera.main.animation.Rewind ();
		Camera.main.animation.Play("cameraKill");
		
		//Pick up function is called when destroyed by enemy
		pickUp ();
			
		//Destroy the enemy mortar drone
		Destroy (gameObject);	
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
	
	
	//This function is called when Mortar is killed by shield Overload (using gunner name because Mortar is tagged with enemy Gunner because they have the same agency).
	public void gunnerOverloadDeath()
	{
		//Create explosion 
		Instantiate (mortarExplosion, transform.position, transform.rotation);
		
		
		//Destroy the enemy mortar drone
		Destroy (gameObject);	
	}
}

