using UnityEngine;
using System.Collections;

public class playerTitanScript : MonoBehaviour 
{

	//Reference: From Tim Dawson's Programming Magic
	
	//Titan Variables
	public float titanSpeed = 50.0f;
	
	//example of a 'setter'
	private Vector3 titanFireVector = Vector3.zero;

	// Use this for initialization
	void Start () 
	{
		Destroy (gameObject, 3);	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//move the Titan in world space
		transform.Translate(titanFireVector * Time.deltaTime, Space.World);
		
		//Move the Titan forwards along the Z axis
		transform.Translate(new Vector3(0.0f, 0.0f, titanSpeed * Time.deltaTime));
	}
}
