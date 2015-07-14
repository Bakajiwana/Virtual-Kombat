using UnityEngine;
using System.Collections;

public class playerBulletScript : MonoBehaviour 
{	
	//Reference: From Tim Dawson's Programming Magic
	
	//Bullet Variables
	public float bulletSpeed = 100.0f;
	
	//example of a 'setter'
	private Vector3 bulletFireVector = Vector3.zero;
	public Vector3 FireVector
	{
		set {bulletFireVector = value; }
	}	

	// Use this for initialization
	void Start () 
	{
		Destroy (gameObject, 2);
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
		//If the bullet collides with melee enemy
		if (other.gameObject.tag == "enemyMelee")
		{		
			//Destroy self
			Destroy (gameObject);
		}
		
		//If the bullet collides with gunner enemy
		if (other.gameObject.tag == "enemyGunner")
		{		
			//Destroy self
			Destroy (gameObject);
		}
	}
}
