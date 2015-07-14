using UnityEngine;
using System.Collections;

public class enemyBulletScript : MonoBehaviour {

	//Reference: From Tim Dawson's Programming Magic
	
	//Bullet Variables
	public float bulletSpeed = 10.0f;
	public int bulletDamage = 10; 
	public Transform enemyBulletEffect;
	
	//example of a 'setter' "from Tim's Asteroid Code"
	private Vector3 bulletFireVector = Vector3.zero;
	public Vector3 FireVector
	{
		set {bulletFireVector = value; }
	}	

	// Use this for initialization
	void Start () 
	{
		//Destroy the bullet in 4 seconds so it's not permenantly in the game
		Destroy (gameObject, 4);	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//move the bullet in world space
		transform.Translate(bulletFireVector * Time.deltaTime, Space.World);
		
		//Move the bullet forwards along the Z axis
		transform.Translate(new Vector3(0.0f, 0.0f, bulletSpeed * Time.deltaTime));
	}
	
	//Collision function when the bullet hits something 
	void OnCollisionEnter (Collision other)
	{
		//If the bullet collides with the player drone 
		if (other.gameObject.tag == "playerDrone")
		{		
			//Find the level manager and activate damage function
			GameObject.FindGameObjectWithTag ("levelManager").SendMessage ("applyPlayerDamage", bulletDamage);
			
			//Create bullet effect on impact
			Instantiate (enemyBulletEffect,transform.position, transform.rotation);
			
			//Destroy bullet
			Destroy (gameObject);
		}
		
		//If Bullet hits other enemies friendly fire is off btw
		if (other.gameObject.tag == "enemyMelee")
		{
			//Create bullet effect on impact
			Instantiate (enemyBulletEffect,transform.position, transform.rotation);
			
			//Destroy the bullet
			Destroy(gameObject);
		}
	}
}
