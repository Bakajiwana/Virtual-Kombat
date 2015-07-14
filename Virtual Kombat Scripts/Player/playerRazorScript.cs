using UnityEngine;
using System.Collections;

public class playerRazorScript : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		//Destroy Self in 6 seconds
		Destroy (gameObject, 6);
	}
}
