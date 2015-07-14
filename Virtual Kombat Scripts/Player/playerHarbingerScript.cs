using UnityEngine;
using System.Collections;

public class playerHarbingerScript : MonoBehaviour 
{	
	//Reference: From Tim Dawson's Programming Magic
	
	//Bullet Variables
	public float harbingerSpeed = 100.0f;
	
	//example of a 'setter'
	private Vector3 harbingerFireVector = Vector3.zero;


	// Use this for initialization
	void Start () 
	{
		Destroy (gameObject, 2);	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//move the bullet in world space
		transform.Translate(harbingerFireVector * Time.deltaTime, Space.World);
		
		//Move the bullet forwards along the Z axis
		transform.Translate(new Vector3(0.0f, 0.0f, harbingerSpeed * Time.deltaTime));
	}
}