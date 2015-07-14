using UnityEngine;
using System.Collections;

public class playerOverHeatScript : MonoBehaviour {
	
	//rotation variables
	private Quaternion iniRot;

	// Use this for initialization
	void Start () 
	{
		//Destroy object in x seconds
		Destroy (gameObject, 7f);
		//Prevent child rotation by returning the first rotation value
		iniRot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () 
	{		
		//apply the first rotation value and freeze rotation
		transform.rotation = iniRot;
	}
}
