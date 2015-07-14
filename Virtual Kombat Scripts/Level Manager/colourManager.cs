using UnityEngine;
using System.Collections;

public class colourManager : MonoBehaviour 
{
	//Reference: Tim Dawson's Book of Magic
	
	//Colour Variables
	public Color[] colours;
	int colourIndex = 0;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void colourChange()
	{
		//Increment the colour index which will change the colour
		colourIndex++;
		
		//If the colour index exceeds the amount of colours within the array. Reset back to start of the array
		if(colourIndex >= colours.Length)
		{
			colourIndex = 0;	
		}
		
		//This is what sets the environment colours
		renderer.material.SetColor ("_Color", colours[colourIndex]);
	}
}
