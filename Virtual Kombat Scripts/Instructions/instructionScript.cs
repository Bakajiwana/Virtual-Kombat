using UnityEngine;
using System.Collections;

public class instructionScript : MonoBehaviour 
{
	//Button variables
	public bool page1;
	public bool page2;
	public bool page3;
	public bool page4;
	public bool mainMenu;
	
	//Camera script
	public instructionsCameraScript insCamera;

	//When Mouse Hovers on text
	void OnMouseEnter()
	{
		//Change the colour of the text
		renderer.material.color = new Color(255f,0f,0f);
	}
	
	//When mouse doesn't hover on text
	void OnMouseExit()
	{
		//Change the colour of the text
		renderer.material.color = new Color(255f,255f,255f);
	}
	
		//When player clicks and release button
	void OnMouseUp()
	{
		//If the first page is clicked
		if(page1)
		{
			insCamera.goToFirstPage();
		}
		
		//If the second page is clicked
		if(page2)
		{
			insCamera.goToSecondPage ();
		}
		
		//If the third page is clicked
		if(page3)
		{
			insCamera.goToThirdPage ();
		}
		
		//If the fourth page is clicked
		if(page4)
		{
			insCamera.goToFourthPage ();
		}
		
		//If main menu button
		if(mainMenu)
		{
			//Go to the main menu
			Application.LoadLevel (0);
			
			//When loading a new level we set the time scale to 1 to prevent pause state
			Time.timeScale = 1.0f;
		}
	}
}
