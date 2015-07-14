using UnityEngine;
using System.Collections;

public class pickUpScript : MonoBehaviour 
{
	//Generate random number
	int randomAbility = (Random.Range (1,10));	
	
	//Pick Up effect 
	public Transform pickUpEffect; 
	
	//Move toward variables
	private GameObject playerDrone;
	public float speed; 

	// Use this for initialization
	void Start () 
	{
		//Destroy the object in 15 seconds
		Destroy (gameObject, 15);
		
		//Return playerDrone location
		playerDrone = GameObject.Find("playerDrone");
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Find player with tag and shove it into an array
		GameObject[] playerObject = GameObject.FindGameObjectsWithTag("playerDrone");
		
		//Prevent a null reference error if the player dies and a pick up is trying to find player.
		if(playerObject.Length  == 1)
		{
			float step = speed * Time.deltaTime;
			//Move towards the player
			transform.position = Vector3.MoveTowards(transform.position, playerDrone.transform.position, step);
		}
	}
	
	//Function is called when object collides with anything
	void OnCollisionEnter (Collision other)
	{
		//If the pick up collides with the playerDrone
		if(other.gameObject.tag == "playerDrone")
		{
			//Use a switch case to determine which ability should be used
			switch (randomAbility)
			{
			case 1: //Use Dash
				GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("dashActivate");
				break;
			case 2: //Use Shield Surplus
				GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("shieldSurplusActivate");
				break;
			case 3: //Use Health Restore
				GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("healthRestoreActivate");
				break;
			case 4: //Use shield over load
				GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("shieldOverloadActivate");
				break;
			case 5: //Use Jammer
				GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("jammerActivate");
				break;
			case 6: //Use triple shot
				GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("tripleShotActivate");
				break;
			case 7: //Use razor 
				GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("razorActivate");
				break;
			case 8: //Use harbinger
				GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("harbingerActivate");
				break;
			case 9: //Use titan wave
				GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("titanActivate");
				break;			
			}
			//Create Pick Up effect
			Instantiate (pickUpEffect, transform.position, transform.rotation);
			//Destroy itself
			Destroy (gameObject);
		}
	}
}
