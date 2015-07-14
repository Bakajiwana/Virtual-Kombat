using UnityEngine;
using System.Collections;

public class instructionsCameraScript : MonoBehaviour 
{	
	//Booleans which activates the move towards functions after bing clicked
	private bool firstPage = false;
	private bool secondPage = false;
	private bool thirdPage = false;
	private bool fourthPage = false;
	
	//Directory variables (positions to where the camera will move towards)
	public Transform directory1;
	public Transform directory2;
	public Transform directory3;
	public Transform directory4;
	
	//Vector3.MoveTowards variables
	public float speed = 20f;


	// Update is called once per frame
	void Update () 
	{
		//Create local variable for movement
		float step = speed * Time.deltaTime;
		
		if(firstPage)
		{
			transform.position = Vector3.MoveTowards (transform.position, directory1.position, step);
		}
		
		if(secondPage)
		{
			transform.position = Vector3.MoveTowards (transform.position, directory2.position, step);
		}
		
		if(thirdPage)
		{
			transform.position = Vector3.MoveTowards (transform.position, directory3.position, step);
		}
		
		if(fourthPage)
		{
			transform.position = Vector3.MoveTowards (transform.position, directory4.position, step);
		}
	}
	
	public void goToFirstPage()
	{
		//Activate first page boolean and turn off the rest of the pages
		firstPage = true;
		secondPage = false;
		thirdPage = false;
		fourthPage = false;	
	}
	
	public void goToSecondPage()
	{
		//Activate second page boolean and turn off the rest of the pages
		firstPage = false;
		secondPage = true;
		thirdPage = false;
		fourthPage = false;
	}
	
	public void goToThirdPage()
	{
		//Activate third page boolean and turn off the rest of the pages
		firstPage = false;
		secondPage = false;
		thirdPage = true;
		fourthPage = false;	
	}
	
	public void goToFourthPage()
	{
		//Activate fourth page boolean and turn off the rest of the pages
		firstPage = false;
		secondPage = false;
		thirdPage = false;
		fourthPage = true;	
	}
}
