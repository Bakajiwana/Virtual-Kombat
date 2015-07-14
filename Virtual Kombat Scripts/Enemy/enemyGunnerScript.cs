using UnityEngine;
using System.Collections;

public class enemyGunnerScript : MonoBehaviour 
{
	//Variable used to return players location
	private GameObject playerLocation;
	
	//Spawn Variables
	public Transform gunnerSpawnEffect;
	
	//Muzzle Flash Variables
	public Transform enemyMuzzleFlash;
	public Transform enemyGunner;
	
	//Scoring variable when killed by player
	public int gunnerScore = 10;
	
	//enemyBullet node is where bullets are spawned from
	public Transform enemyBulletNode;
	
	//The enemyBullet is a prefab created when the fire button is pressed
	public Transform enemyBullet;
	
	//Explosion variable
	public Transform gunnerExplosion;	
	
	//Jammer Variable
	private bool jammerBoolean = false;
	public Transform jammerEffect;
	
	//Movement Variable
	public float speed = 10f;
	
	//Pick up variable 
	//Reference: http://forum.unity3d.com/threads/57562-Random-drop
	public float dropRate = 0.07f; 
	public Transform pickUpDrop;
	

	// Use this for initialization
	void Start () 
	{
		//Start the enumerator function for the enemies shooting loops
		StartCoroutine (shoot ());
		
		//Create Spawn effect
		Instantiate (gunnerSpawnEffect, transform.position, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () 
	{	
		//Find player with tag and shove it into an array
		GameObject[] playerObject = GameObject.FindGameObjectsWithTag("playerDrone");
		
		//Aim at the player if not jammed by the players ability and there is more than one player to prevent a null reference error.
		if(playerObject.Length  == 1 && jammerBoolean == false)
		{
			//Find player object location
			playerLocation = GameObject.Find ("playerDrone");
			transform.LookAt(playerLocation.transform);
			
			//Move towards player
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards (transform.position, playerLocation.transform.position, step);
		}
	}
	
	//Function is called when gunner is jammed by player's ability
	public void jammerGunnerStatus(bool _jammed)
	{
		jammerBoolean = _jammed;
	}
	
	//This function is called to instantiate the jammer effect
	public void enemyJammed()
	{
		//Create the jammer Effect
		Transform jammerField = Instantiate (jammerEffect, transform.position, transform.rotation) as Transform;
		jammerField.transform.parent = enemyGunner;
	}
	
	
	//Create a looping process where the enemy shoots bullet
	/*
	  REFERENCE: This method I took from Aldo Naletto and just tweaked the variables to
	  fit my game. All credit goes to him. Website: http://answers.unity3d.com/users/11109/aldonaletto.html
	*/
	IEnumerator shoot()
	{
		//While true and not jammed
		while(true && jammerBoolean == false) //loops forever
		{ 
			//Find player with tag and shove it into an array
			GameObject[] playerObject = GameObject.FindGameObjectsWithTag("playerDrone");
			if(playerObject.Length == 1 && jammerBoolean == false) //if there is 1 player in the scene to prevent a null reference error
			{
				//loop through the children of the bulletnode and create a bullet for each node "from Tim's Asteroid Code"
				foreach (Transform child in enemyBulletNode)
				{
					//create a bullet, assign its transform to a variable, then set the new bullet's firevector
					Transform bullet = Instantiate(enemyBullet, child.position, child.rotation) as Transform;
					bullet.GetComponent<enemyBulletScript>().FireVector = rigidbody.velocity; 
					
					//Create Muzzle Flash
					Transform muzzleFlash = Instantiate (enemyMuzzleFlash, transform.position,transform.rotation) as Transform;
					muzzleFlash.transform.parent = enemyGunner;
				}
				//Each enemyGunner will shoot according to timer
				yield return new WaitForSeconds(1.5f);
			} else { 
				//if no player then return nothing and the while loop will become false.
				yield return null;
			}
		}
	}
	
	//collision function is called when enemy gunner collides with an object
	void OnCollisionEnter (Collision other)
	{
		//If hit by player's bullet
		if (other.gameObject.tag == "playerBullet")
		{	
			//Activate enemyGunnerDeath function
			enemyGunnerDeath ();
		}
		
		//Prevent a Physics problem where drone leaves game area.
		if (other.gameObject.tag == "boundary")
		{
			//Destroy game object when boundary is hit
			Destroy (gameObject);
		}
	}
	
	//This Function is called when enemy is killed by player
	public void enemyGunnerDeath()
	{
		//Create explosion
		Instantiate (gunnerExplosion, transform.position, transform.rotation);
			
		//Find the level manager and apply score
		GameObject.FindGameObjectWithTag ("levelManager").SendMessage ("applyScore", gunnerScore);
		
		//Reference: Tim showed us how to animate camera
		//Call camera hit animation, rewind the animation first in case already playing it
		Camera.main.animation.Rewind ();
		Camera.main.animation.Play("cameraKill");
		
		//Call the pickUp function. There is a chance the enemy will drop a collectable for the player to use.
		pickUp ();
			
		//Destroy the enemy gunner drone
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
	
	//This Function is called when enemy is killed by shield Overload
	public void gunnerOverloadDeath()
	{
		//Create explosion
		Instantiate (gunnerExplosion, transform.position, transform.rotation);
			
		//Destroy the enemy gunner drone
		Destroy (gameObject);	
	}
}

