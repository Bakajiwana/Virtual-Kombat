using UnityEngine;
using System.Collections;

public class levelManagerScript : MonoBehaviour 
{
	//Variable for player's health and create a variable for the text mesh
	//Reference: Making a health bar: http://wiki.unity3d.com/index.php/Health_Bar_Tutorial
	//and http://docs.unity3d.com/Documentation/Components/gui-Layout.html
	private int playerHealth;
	private int playerMinHealth = 0;
	public int playerMaxHealth= 100;
	
	//Health Bar and shield bar Variables
	public Texture healthBackImage;
	public Texture healthFrontImage;
	public Texture shieldFrontImage;
	public Texture healthBorderImage;
	private float healthPercent;
	private float shieldPercent;
	private int playerShieldDisplay;
	
	//Health and Shield Bar GUI locations
	private float left;
	private float top;
	private float height;
	private float backWidthHealth;
	private float frontWidthHealth;
	private float frontWidthShield;
	
	//Over Heat Bar Variables
	public Texture heatBackImage;
	public Texture heatFrontImage;
	public Texture heatBorderImage;
	private float heatPercent;
	
	//Over heat GUI locations
	private float heatLeft;
	private float heatTop;
	private float backHeightHeat;
	private float frontHeightHeat;
	private float heatWidth;
	
	
	//Shield variables
	public static int playerShield;
	private int playerMinShield = -1;
	public int playerMaxShield = 100;
	public int rechargeRate = 1;
	public float regenerationSpeed = 0.75f;
	
	//Scoring variables
	private int playerScore = 0;
	public TextMesh playerScoreText;
	private int playerHighScore = 0;
	public TextMesh playerHighScoreText;
	
	//Pause Variables
	public Transform pauseNode;
	
	//GameOver Variables
	public Transform gameOverNode;
	public TextMesh gameOverHighScoreText;
	public Transform gameOverBackground;
	public TextMesh newHighScoreText;
	public static bool gameOverCameraLock = false;
	public float gameOverBreakTimer = 2f;
	
	//Spawning Variables
	private float meleeSpawnTimer;
	public float meleeMaxTimer = 3f;
	private float gunnerSpawnTimer;
	public float gunnerMaxTimer = 5f;
	private float mortarSpawnTimer;
	public float mortarMaxTimer = 7f;
	
	//Model Variables
	public Transform enemyMelee;
	public Transform enemyGunner;
	public Transform enemyMortar;
	
	//Shield Surplus Ability variables
	private bool shieldSurplusAbility = false; 
	
	//Mouse Cursor Variables
	public Texture2D cursor;
	private int cursorSizeX = 3000;
	private int cursorSizeY = 3000;
	
	//Player Damage Variables (When player gets hit)
	private bool playerDamage = false;
	public Transform playerDamageBackground;
	public float playerDamageMaxTimer = 1f;
	public float playerDamageTimer; 
	
	
	// Use this for initialization
	void Start () 
	{
		//Hide cursor so custom cursor can be shown
		Screen.showCursor = false;
		
		gameOverCameraLock = false;
		
		//Player health and shield should equal to max health and shield
		playerHealth = playerMaxHealth;	
		playerShield = playerMaxShield;
		
		//This is the auto regenerating coroutine for the shield
		StartCoroutine (regenerate());
				
		//Start the match by showing stored high score
		playerHighScore = PlayerPrefs.GetInt ("highscore", 0);
		playerHighScoreText.text = playerHighScore.ToString("N0");
		
		//Make sure game over text is turned off
		gameOverNode.gameObject.SetActive (false);
		
		//Make sure playerDamage Timer = playerDamageMaxTimer
		playerDamageTimer = playerDamageMaxTimer; 
	}	
	
	// Update is called once per frame
	void Update () 
	{		
		//If player gets hit show feedback
		if(playerDamage == true)
		{
			//Start count down of showing feedback
			playerDamageTimer -= Time.deltaTime;	
		}
		
		//If damage timer runs out take feedback screen away
		if(playerDamageTimer <= 0f && playerDamage == true)
		{
			playerDamageBackground.gameObject.SetActive (false);
			playerDamage = false;
			playerDamageTimer = playerDamageMaxTimer;
		}
		
		//Find all player and shove it in an array
		GameObject[] player = GameObject.FindGameObjectsWithTag("playerDrone");
		
		//If the game over camera locked then wait for the game over timer to decrease to bring in the game over menu. This is just so players don't click on the menu when they least expect their death.
		if(gameOverCameraLock == true)
		{
			//countdown gameover timer
			gameOverBreakTimer -= Time.deltaTime;	
		}
		
		//if game over countdown complete then show gameover screen
		if(gameOverBreakTimer <= 0)
		{
			gameOver ();	
		}
		
		
		//Activate Pause Button when escape button is pressed
		if (Input.GetKeyDown (KeyCode.Escape))
		{
			//If gameworld is unpaused
			if (Time.timeScale == 1)
        	{
				//Pause the game and display Paused text
         		Time.timeScale = 0;
				pauseNode.gameObject.SetActive (true);
      		}
       		else
        	{
				unPause ();
        	}
    	}
		
		//THE SPAWN SYSTEM: inspired by how Geometry wars makes their enemies spawn in timers and frequency
		//Start the Countdown
		meleeSpawnTimer -= Time.deltaTime;
		gunnerSpawnTimer -= Time.deltaTime;
		mortarSpawnTimer -= Time.deltaTime; 
		
		//If the Melee Timer runs out then make sure no collider is in the way and spawn meleeEnemy
		if (meleeSpawnTimer <= 0 && player.Length >= 1)
		{
			//Reference: Tweaking Tim's Asteroid Spawn checking code	
			
			//This will be used to check all colliders in a layer
			LayerMask mask = 0;
			
			//Vector newPosition will be in the center of the map
			Vector3 newPositionMelee = Vector3.zero;
			
			//Boolean whether all enemies have been placed
			bool placed = false;
			int tries = 0;
			
			//While the enemy that is required to be placed hasn't been placed, try 100 times...
			while (!placed && tries < 100)
			{
				//Find a random location for enemy to spawn
				newPositionMelee = new Vector3(Random.Range (-34f, 34f), 1f, Random.Range (-19f, 19f));
				
				//Check if enemy has been placed 
				if(!Physics.CheckSphere (newPositionMelee, 3f, mask))
				{
					//placed equal true if enemy has spawned
					placed = true;
				}
				else
				{
					//or keep trying if the enemy has not been placed
					tries++;
				}
			}
			
			//if less than 100 tries then clone enemy
			if(tries < 100)
			{
				//Spawn melee Enemies
				Instantiate (enemyMelee, newPositionMelee, Quaternion.Euler (0f, Random.Range (-180f, 180f), 0f));
			}
			
			//Restart Melee Spawn timer
			meleeSpawnTimer = meleeMaxTimer;
		}
		
		//If gunner timer runs out spawn gunner enemy
		if (gunnerSpawnTimer <= 0 && player.Length >= 1)
		{
			//This will be used to check all colliders in a layer
			LayerMask mask = 0;
			
			//Vector newPosition will be in the center of the map
			Vector3 newPositionGunner = Vector3.zero;
			
			//Boolean whether all enemies have been placed
			bool placed = false;
			int tries = 0;
			
			//While the enemy that is required to be placed hasn't been placed, try 100 times...
			while (!placed && tries < 100)
			{
				//Find a random location for enemy to spawn
				newPositionGunner = new Vector3(Random.Range (-34f, 34f), 1f, Random.Range (-19f, 19f));
				
				//Check if enemy has been placed 
				if(!Physics.CheckSphere (newPositionGunner, 3f, mask))
				{
					//placed equal true if enemy has spawned
					placed = true;
				}
				else
				{
					//or keep trying if the enemy has not been placed
					tries++;
				}
			}		
			
			//if less than 100 tries then clone enemy
			if(tries < 100)
			{
				//Spawn enemy gunners
				Instantiate (enemyGunner, newPositionGunner, Quaternion.Euler (0f, Random.Range (-180f, 180f), 0f));				
			}
			
			//Restart the Gunner Spawn timer
			gunnerSpawnTimer = gunnerMaxTimer;
		}
		
		if (mortarSpawnTimer <= 0 && player.Length >= 1)
		{
			//This will be used to check all colliders in a layer
			LayerMask mask = 0;
			
			//Vector newPosition will be in the center of the map
			Vector3 newPositionMortar = Vector3.zero;
			
			//Boolean whether all enemies have been placed
			bool placed = false;
			int tries = 0;
			
			//While the enemy that is required to be placed hasn't been placed, try 100 times...
			while (!placed && tries < 100)
			{
				//Find a random location for enemy to spawn
				newPositionMortar = new Vector3(Random.Range (-34f, 34f), 1f, Random.Range (-19f, 19f));
				
				//Check if enemy has been placed 
				if(!Physics.CheckSphere (newPositionMortar, 3f, mask))
				{
					//placed equal true if enemy has spawned
					placed = true;
				}
				else
				{
					//or keep trying if the enemy has not been placed
					tries++;
				}
			}	
			
			//if less than 100 tries then clone enemy
			if(tries < 100)
			{
				//Spawn Mortar Enemy
				Instantiate (enemyMortar, newPositionMortar, Quaternion.Euler (0f, Random.Range (-180f, 180f), 0f));
			}
			
			//Reset Mortar Spawn Timer
			mortarSpawnTimer = mortarMaxTimer;
		}
	}
	
	
	
	//OnGUI is called to control GUI
	void OnGUI()
	{	
		//Show Custom Cursor but only if not in game over
		//Reference: http://answers.unity3d.com/questions/145024/unity3d-custom-cursors.html
		if (gameOverCameraLock == false)
		{
			GUI.DrawTexture(new Rect(Event.current.mousePosition.x-cursorSizeX/2, Event.current.mousePosition.y - cursorSizeY/2, cursorSizeX, cursorSizeY), cursor);
		}
		
		//Adjust the the values so no negatives or extra health is calculated
		if(playerHealth < 0)
		{
			playerHealth = 0;
		}
		
		if(playerHealth > playerMaxHealth)
		{
			playerHealth = playerMaxHealth;	
		}
		
		if(playerMaxHealth < 1)
		{
			playerMaxHealth = 1;	
		}
		
		//The playerShieldDisplay Variable is used as a replacement for playerShield because we want the player shield to become a negative for it becomes a timer when the value hits a negative
		if(playerShield < 0)
		{
			playerShieldDisplay = 0; 
		}
		
		if(playerShield > playerMaxShield)
		{
			playerShield = playerMaxShield;
		}
		
		if(playerShield > 0)
		{
			playerShieldDisplay = playerShield;
		}
		
		//GUI Values of health and shield 
		//Gauge Percentage
		healthPercent = playerHealth/ (float)playerMaxHealth;
		shieldPercent = playerShieldDisplay/ (float)playerMaxShield;
		//Left and top of screen
		left = Screen.width / 2.35f;
		top = Screen.height / 50f;
		//Width and height of screen
		backWidthHealth = Screen.width / 8f;
		frontWidthHealth = healthPercent * backWidthHealth; 
		frontWidthShield = shieldPercent * backWidthHealth;
		height = Screen.height / 15f;
		
		//Health Bar using GUI draw textures
		//GUI DrawTexture 		Location							Image			Scale Parameter			Transparency and aspect ratio
		GUI.DrawTexture (new Rect(left, top, backWidthHealth, height), healthBackImage, ScaleMode.StretchToFill, true, 1.0f);
		GUI.DrawTexture (new Rect(left, top, frontWidthHealth, height), healthFrontImage, ScaleMode.StretchToFill, true, 1.0f);
		GUI.DrawTexture (new Rect(left, top, frontWidthShield, height), shieldFrontImage, ScaleMode.StretchToFill, true, 1.0f);
		GUI.DrawTexture (new Rect(left, top, backWidthHealth, height), healthBorderImage, ScaleMode.StretchToFill, true, 1.0f);
		
		//GUI Values of over heat bar
		//Percentage
		/*For Percentage of heat bar if done normally the heat bar will start from the top and go down. 
		I don't want that. So I am doing maxHeat - current heat then divide that by the max to start the percentage the other way around. 
		So I just create an icy texture at the front image to show that it is cool and that image will go up revealing the back image, and the back image will have the red bar showing
		that the gun is overheating.*/
		heatPercent = (playerStatusScript.overHeatCoolDown - playerStatusScript.heat)/ (float)playerStatusScript.overHeatCoolDown;
		//Left and top of screen
		heatLeft = Screen.width / 1.08f;
		heatTop = Screen.height / 60f;
		//Height and width of screen
		backHeightHeat = Screen.height / 2f;
		frontHeightHeat = heatPercent * backHeightHeat;
		heatWidth = Screen.width/25f;
		
		//Over heat bar using GUI draw textures
		GUI.DrawTexture (new Rect(heatLeft, heatTop, heatWidth, backHeightHeat), heatBackImage, ScaleMode.StretchToFill, true, 1.0f);
		GUI.DrawTexture (new Rect(heatLeft, heatTop, heatWidth, frontHeightHeat), heatFrontImage, ScaleMode.StretchToFill, true, 1.0f);
		GUI.DrawTexture (new Rect(heatLeft, heatTop, heatWidth, backHeightHeat), heatBorderImage, ScaleMode.StretchToFill, true, 1.0f);
	}
	
	
	//this function is called when player is damaged
	public void applyPlayerDamage(int _damage)
	{
		if(gameOverCameraLock == false)
		{
			//Make camera react with animation showed by Tim
			Camera.main.animation.Rewind ();
			Camera.main.animation.Play("cameraKill");
		}
		
		if(shieldSurplusAbility || playerAbilityScript.shieldOverloadBoolean == true)
		{
			//Player shield will increase to 100
			playerShield = 100;
			//Enemies cannot damage player
			_damage = 0;
		}
		else 
		{
			//Show feedback that player just got hit. 
			playerDamage = true; 
			playerDamageBackground.gameObject.SetActive (true);
			
			//If Shield is more than 0 
			if(playerShield > playerMinShield)
			{
				//Player loses shield according to damage
				playerShield -= _damage;
			}		
			//If player has equal or less than 0 health
			else if(playerShield <= playerMinShield)
			{
				//Player loses health according to the amount of damage melee has
				playerHealth -= _damage;
			}			
			//If players health reaches below 0: death...
			if (playerHealth <= playerMinHealth)
			{
				//if score is more than highscore
				if(playerScore > PlayerPrefs.GetInt ("highscore", 0))
				{
					//Store High Score
					PlayerPrefs.SetInt ("highscore", playerScore);
				}
								
				//Lock the camera with a boolean to prevent a recent bullet hitting an enemy activating the camera hit animation which takes the cameraGameOver animation away
				gameOverCameraLock = true;
				
				//Show gameOver background
				gameOverBackground.gameObject.SetActive(true);
				
				//Play camera game over animation
				Camera.main.animation.Play ("cameraGameOver");
				
				//Send Music Manager to play the game over song
				GameObject.FindGameObjectWithTag("music").SendMessage ("gameOverSong");
							
				//Send death message to player drone and activate death function
				GameObject.FindGameObjectWithTag("playerDrone").SendMessage ("Death", gameObject);	
			}
		}
	}
	
	void gameOver()
	{
		//Show cursor
		Screen.showCursor = true;		
		
		//turn on game over menu
		gameOverNode.gameObject.SetActive (true);
		
		//If the player beats the high score show the player they beat it
		if(playerScore > playerHighScore)
		{
			newHighScoreText.gameObject.SetActive (true);
			//Using the player score because the game over high score doesn't update for some reason...
			gameOverHighScoreText.text = "High Score: " + playerScore;
		}
		else
		{
			gameOverHighScoreText.text = "High Score: " + playerHighScore;
		}	
	}
	
	//This function is called to activate the shield surplus ability
	public void shieldSurplusStatus(bool shieldSurplusBool)
	{
		shieldSurplusAbility = shieldSurplusBool;
	}
	
	public void healthRestoreStatus (int _healthRestoreAmount)
	{
		playerHealth = _healthRestoreAmount;
	}

	//When player kills enemy increment score
	public void applyScore(int _score)
	{
		//Increment the amount of points depending on set score
		playerScore += _score;
		
		//Update player score 3D Text Mesh
		playerScoreText.text = "Score:   " + playerScore; 
		
		//For Every x score decrease the spawn timers
		if ((playerScore % 500) == 0)
		{			
			//Change the Colours in the Colour Manager Script
			//Find all environment tags and shove in array
			GameObject [] levelEnvironment = GameObject.FindGameObjectsWithTag ("environment");
			
			for(int i = 0; i< levelEnvironment.Length; i++)
			{
				
				//Send message to enemey Melee to stop moving
				levelEnvironment [i].SendMessage ("colourChange");
				
			}
			
			//If timer is more than x time then decrement max timer
			if(meleeMaxTimer > 0.3f)
			{
				meleeMaxTimer -= 0.20f;
			}
			
			if(gunnerMaxTimer > 0.3f)
			{
				gunnerMaxTimer -= 0.20f;
			}
			
			if(mortarMaxTimer > 0.3f)
			{
				mortarMaxTimer -= 0.20f;
			}
		}
		
		if((playerScore % 1000) == 0)
		{
			//Change the song in the Music Manager
			GameObject.FindGameObjectWithTag("music").SendMessage ("mainSongPlay", Random.Range (1,6));
		}
		
	}
	
	
	
	//Create a looping process where the shield will regenerates over time when it hits 0
	/*
	  REFERENCE: This method I took from Aldo Naletto and just tweaked the variables to
	  fit my game. All credit goes to him. Website: http://answers.unity3d.com/users/11109/aldonaletto.html
	*/
	IEnumerator regenerate()
	{
		while(true) //loops forever
		{ 
			if(playerShield < playerMaxShield) //if shield is less than 100
			{
				playerShield += rechargeRate; //increase shield and wait for specified time
				yield return new WaitForSeconds(regenerationSpeed);
			} 
			else 
			{ 
				//if shield >= 100, just yield nothing
				yield return null;
			}
		}
	}
	
	//Function is called to unpause the game
	public void unPause()
	{
		//Unpause game and take pause text away
    	Time.timeScale = 1;
		pauseNode.gameObject.SetActive (false);
		//Hide cursor so custom cursor can be shown
		Screen.showCursor = false;
	}
		
}
