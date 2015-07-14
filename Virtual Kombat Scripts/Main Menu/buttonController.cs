using UnityEngine;
using System.Collections;

public class buttonController : MonoBehaviour 
{
	
	//Boolean to identify each text button
	public bool startDebugBtn = false;
	public bool instructionsBtn = false;
	public bool quitGameBtn = false;
	public bool mainMenuBtn = false;
	public bool resumeBtn = false;
	
	void Start()
	{
		//make sure cursor is on
		Screen.showCursor = true;
	}
	
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
	
	//When player clicks and release on a button
	void OnMouseUp()
	{
		//If start debug button is clicked
		if(startDebugBtn)
		{
			//When loading a new level we set the time scale to 1 to prevent pause state
			Time.timeScale = 1.0f;
			//Go to game
			Application.LoadLevel (1);	
		}
		
		
		//if instructions button is clicked
		if(instructionsBtn)
		{
			//Go to instructions scene
			Application.LoadLevel (2);
			
			//Make sure no pause state
			Time.timeScale = 1.0f;
		}
		
		//if quit button is clicked
		if(quitGameBtn)
		{
			//Quit the god damn game 
			Application.Quit ();
		}
		
		//If main menu button is clicked
		if(mainMenuBtn)
		{
			//Go to the main menu
			Application.LoadLevel (0);
			
			//When loading a new level we set the time scale to 1 to prevent pause state
			Time.timeScale = 1.0f;
		}
		
		//If the resume button is pressed 
		if (resumeBtn)
		{
			//Unpause the game
			GameObject.FindGameObjectWithTag("levelManager").SendMessage ("unPause");
		}		
	}
}
