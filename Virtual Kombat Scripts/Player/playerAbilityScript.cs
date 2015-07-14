using UnityEngine;
using System.Collections;

public class playerAbilityScript : MonoBehaviour 
{	
	//Press RMB TextMesh Variable when a secondary weapon is unlocked
	public TextMesh pressRMBText;
	
	//PlayerDrone used for parenting
	public Transform playerDrone;
	
	//Dash Variables - Increase speed of player
	public static float dashTimer = 10f;
	public float dashMaxTimer = 10f;
	private bool dashBoolean = false; 
	public TextMesh dashText;
	public Transform dashEffect;
	
	//Shield Surplus Variables - Shield is invulnerable 
	public static float shieldSurplusTimer = 10f;
	public float shieldSurplusMaxTimer = 10f;
	private bool shieldSurplusBoolean = false;
	public TextMesh shieldSurplusText;
	public Transform shieldSurplusEffect;
	
	//Health Restore Variables - Restore Health
	public static float healthRestoreTimer = 0f;
	private bool healthRestoreBoolean = false;
	private int healthRestore = 100;
	public TextMesh healthRestoreText;
	public Transform healthRestoreEffect;
	
	//Shield Overload Variables - AOe attack destroy all enemies in the field 
	public static float shieldOverloadTimer = 3f; 
	public float shieldOverloadMaxTimer = 3f;
	public static bool shieldOverloadBoolean = false; 
	public TextMesh shieldOverloadText;
	public Transform shieldOverloadEffect;
	
	//Jammer Variables - Disable all enemies 
	public static float jammerTimer = 5f; 
	public float jammerMaxTimer = 5f; 
	private bool jammerBoolean = false;
	public TextMesh jammerText;
	
	//Triple Shot Variables - create 2 extra bullet nodes
	public static float tripleShotTimer = -1f;
	private bool tripleShotBoolean = false;
	public Transform node1;
	public Transform node2;
	public Transform playerBulletNode;
	public TextMesh tripleShotText;
	public Transform tripleShotEffect;
	
	//Razor Gun Variables - Activate Razor Gun Secondary Weapon 
	private float razorTimer = 20f;
	public float razorMaxTimer = 20f; 
	private bool razorBoolean = false;
	public TextMesh razorText;
	
	//Harbinger variable - Shoot rockets that track enemy 
	private float harbingerTimer = 20f;
	public float harbingerMaxTimer = 20f;
	private bool harbingerBoolean = false;	
	public TextMesh harbingerText;
	
	//Titan Wave - Shoot a wave to destroy enemies
	private float titanTimer = 20f;
	public float titanMaxTimer = 20f;
	private bool titanBoolean = false;	
	public TextMesh titanText;
	

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		//If the dash ability has been activated
		if (dashBoolean == true)
		{
			//Start the countdown
			dashTimer -= Time.deltaTime;
			//Start the ability
			dash ();
		}
		
		//If the shield surplus ability has been activated
		if (shieldSurplusBoolean == true)
		{
			//start the countdown
			shieldSurplusTimer -= Time.deltaTime;
			//Start the ability
			shieldSurplus();
		}
		
		//Health restore ability
		if (healthRestoreBoolean == true)
		{
			//Count down timer (This is just used to display the health restore text for a certain time)
			healthRestoreTimer -= Time.deltaTime;
			
			//If health timer is greater than 0 then display text
			if (healthRestoreTimer >= 0.0f)
			{
				//Show Text to player that they aquired this ability
				healthRestoreText.gameObject.SetActive (true);
			}
			else
			{
				//Show Text to player that they aquired this ability
				healthRestoreText.gameObject.SetActive (false);
				//Turn boolean off
				healthRestoreBoolean = false;
			}
		}

		
		//If the shield overload ability has been activated 
		if (shieldOverloadBoolean == true)
		{
			//Start the timer
			shieldOverloadTimer -= Time.deltaTime;
			//Call the Shield Overload function
			shieldOverload();
		}	
		
		//If jammer ability has been activated
		if (jammerBoolean == true)
		{
			//Start the Jammer timer 
			jammerTimer -= Time.deltaTime;
			//Call the jammer function
			jammer ();
		}
		
		//Triple shot timer is called when the triple shot function starts the timer
		//The reason for this timer is to set the triple shot text mesh active
		if (tripleShotTimer >= 0.0f)
		{
			//While the triple shot timer is set. Start the count down
			tripleShotTimer -= Time.deltaTime;
			//Send playerDrone Status Script to turn off overheating
			GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("tripleShotHeatControl", true);
			//Show Text to player that they aquired this ability
			tripleShotText.gameObject.SetActive (true);
		}
		else
		{
			tripleShotTimer = -1f;
			//Show Text to player that they aquired this ability
			tripleShotText.gameObject.SetActive (false);
			//Send playerDrone Status Script to turn on overheating
			GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("tripleShotHeatControl", false);
			//Deactivate triple shot
			tripleShotBoolean = false; 
		}
		
		//If razor gun is activated
		if(razorBoolean == true)
		{
			razorTimer -= Time.deltaTime; 
			razor();
		}
		
		//If harbinger gun is activated
		if(harbingerBoolean == true)
		{
			harbingerTimer -= Time.deltaTime;
			harbinger();
		}
		
		//If titan gun is activated
		if(titanBoolean == true)
		{
			titanTimer -= Time.deltaTime; 
			titan();
		}
	}
	
	//Function is called to activate the dash ability
	public void dashActivate()
	{
		dashTimer = dashMaxTimer;
		dashBoolean = true;
		//Create dash trail
		Transform dashTrail = Instantiate (dashEffect, transform.position, transform.rotation) as Transform;
		dashTrail.transform.parent = playerDrone;
	}
	
	
	//The Dash Ability - Speed is increased for a limited of time
	void dash()
	{
		//If timer is above 0
		if (dashTimer >= 0.0f)
		{
			//Send movementspeed change to movement script
			GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("dashMovement", 1500f);
			//Show Text to player that they aquired this ability
			dashText.gameObject.SetActive (true);
		}
		else
		{
			//Send mevementspeed change back to normal
			GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("dashMovement", 1000f);
			dashBoolean = false; 
			//Show Text to player that they aquired this ability
			dashText.gameObject.SetActive (false);
		}
	}	
	
	//Function is called to activate the shield surplus ability
	public void shieldSurplusActivate()
	{
		shieldSurplusTimer= shieldSurplusMaxTimer;
		shieldSurplusBoolean = true; 
		//Create the Shield Surplus Effect
		Transform shieldSurplusField = Instantiate (shieldSurplusEffect, transform.position, transform.rotation) as Transform;
		shieldSurplusField.transform.parent = playerDrone;
		
		//Send music manager to start the invincible aku aku song
		GameObject.FindGameObjectWithTag("music").SendMessage ("shieldSurplusSong");
	}
	
	
	//The shield surplus ability - Infinite shield for a limited of time
	void shieldSurplus()
	{
		//If there is still time in the shield surplus timer
		if (shieldSurplusTimer >= 0.0f)
		{
			//Send message to level manager to activate shield surplus
			GameObject.FindGameObjectWithTag ("levelManager").SendMessage ("shieldSurplusStatus", true);
			//Show Text to player that they aquired this ability
			shieldSurplusText.gameObject.SetActive (true);
		}
		else
		{
			//Send message to level manager to deactivate shield surplus
			GameObject.FindGameObjectWithTag ("levelManager").SendMessage ("shieldSurplusStatus", false);
			shieldSurplusBoolean = false;
			//Show Text to player that they aquired this ability
			shieldSurplusText.gameObject.SetActive (false);
			
			//Turn off Invincible Aku Aku
			GameObject.FindGameObjectWithTag("music").SendMessage ("mainSongPlay", Random.Range (1,6));
		}
	}
	
	//The Health Restore Ability - Health is restored to 100%
	public void healthRestoreActivate()
	{
		//Health restore timer is set just for the text mesh to be shown
		healthRestoreTimer = 3f;
		//Activate the boolean just for the text mesh to be shown
		healthRestoreBoolean = true;
		//Send message to level manager to apply a health restore according to the healthRestore variable
		GameObject.FindGameObjectWithTag ("levelManager").SendMessage ("healthRestoreStatus", healthRestore);
		//Create the Health Restore Effect
		Transform healthRestoreField = Instantiate (healthRestoreEffect, transform.position, transform.rotation) as Transform;
		healthRestoreField.transform.parent = playerDrone;
	}
	
	
	//Function is called to activate the shield overload ability
	public void shieldOverloadActivate()
	{
		shieldOverloadTimer = shieldOverloadMaxTimer;
		shieldOverloadBoolean = true;
		//Create the Shield Overload Effect
		Transform shieldOverloadField = Instantiate (shieldOverloadEffect, transform.position, transform.rotation) as Transform;
		shieldOverloadField.transform.parent = playerDrone;
	}
	
	
	//The Shield Overload Ability - Player AOe ability, Destroy all enemies in the level in 3 seconds
	//Note: within the 3 second countdown to when all enemies are destroyed an animation can be played
	void shieldOverload()
	{				
		//Find all enemies using their tags, so all enemies can be destroyed
		GameObject [] meleeEnemies = GameObject.FindGameObjectsWithTag ("enemyMelee");
		GameObject [] gunnerEnemies = GameObject.FindGameObjectsWithTag ("enemyGunner");
		
		//After the timer runs out destroy all enemies
		//This ability should have some time to do its animation
		if (shieldOverloadTimer >= 0.0f)
		{
			//Show Text to player that they aquired this ability
			shieldOverloadText.gameObject.SetActive (true);
		}
		else
		{
			//When the timer runs out.... DESTROY ALL ENEMIES
			
			//AND PLAY CAMERA ANIMATION if boolean in level manager isn't true for game over
			//Lock the camera with a boolean to prevent a recent bullet hitting an enemy activating the camera hit animation which takes the cameraGameOver animation away
			if (levelManagerScript.gameOverCameraLock == false)
			{
				Camera.main.animation.Rewind ();
				Camera.main.animation.Play ("cameraOverload");
			}
			
			for(int i = 0; i< meleeEnemies.Length; i++)
			{
				
				//Send message to enemey Melee to stop moving
				meleeEnemies[i].SendMessage ("meleeOverloadDeath");
				
			}
			
			for(int i = 0; i < gunnerEnemies.Length; i++)
			{				
				//Send message to enemey gunner to stop shooting
				gunnerEnemies[i].SendMessage ("gunnerOverloadDeath");				
			}		
			
				
			//After that the player should only get a fixed amount of points like how the nuke in Black Ops zombies works. After getting the nuke. You will always get 400 points. Not the amount of zombies that died all together.
			GameObject.FindGameObjectWithTag ("levelManager").SendMessage ("applyScore", 100);
			
			//Show Text to player that they aquired this ability
			shieldOverloadText.gameObject.SetActive (false);
			shieldOverloadBoolean = false;
		}			
	}
	
	//Function is called when the jammer ability is activated
	public void jammerActivate()
	{
		jammerTimer = jammerMaxTimer; 	
		jammerBoolean = true; 
		
		//Instantiate a jammer effect on every enemy
		//Find all enemies using their tags, so they can all be found
		GameObject [] enemyMelees = GameObject.FindGameObjectsWithTag ("enemyMelee");
		GameObject [] enemyGunners = GameObject.FindGameObjectsWithTag ("enemyGunner");
		
		for(int i = 0; i< enemyMelees.Length; i++)
		{			
			//Send message to enemey Melee to instantiate jammerEffect
			enemyMelees[i].SendMessage ("enemyJammed");			
		}
		
		for(int i = 0; i < enemyGunners.Length; i++)
		{			
			//Send message to enemey gunner to instantiate jammerEffect
			enemyGunners[i].SendMessage ("enemyJammed");			
		}
	}
	
	//The Jammer Ability - Jam all enemies in the level. Every Enemy stops attacking player
	void jammer()
	{
		//Find all enemies using their tags, so they can all be found and 
		GameObject [] melee = GameObject.FindGameObjectsWithTag ("enemyMelee");
		GameObject [] gunner = GameObject.FindGameObjectsWithTag ("enemyGunner");
		
		if (jammerTimer >= 0.0f)
		{
			for(int i = 0; i< melee.Length; i++)
			{
				
				//Send message to enemey Melee to stop moving
				melee[i].SendMessage ("jammerMeleeStatus", true);
				
			}
			
			for(int i = 0; i < gunner.Length; i++)
			{
				
				//Send message to enemey gunner to stop shooting
				gunner[i].SendMessage ("jammerGunnerStatus", true);
				
			}
			//Show Text to player that they aquired this ability
			jammerText.gameObject.SetActive (true);
		}
		else
		{
			for (int i = 0; i< melee.Length; i++)
			{
				
				//Send message to enemey Melee to stop moving
				melee[i].SendMessage ("jammerMeleeStatus", false);
				
			}
			for (int i = 0; i < gunner.Length; i++)
			{
				
				//Send message to enemey gunner to stop shooting
				gunner[i].SendMessage ("jammerGunnerStatus", false);
				
			}
			//Show Text to player that they aquired this ability
			jammerText.gameObject.SetActive (false);
			jammerBoolean = false; 
		}
	}
	
	//Activate Triple Shot ability
	public void tripleShotActivate()
	{
		if(tripleShotTimer == -1)
		{
			tripleShotTimer = 7.0f;
			//Make sure only one set of nodes can only be in the game. Prevent multiple nodes spawning.
			if (tripleShotBoolean == false)
			{
				tripleShot ();
				tripleShotBoolean = true;
			}	
		}
	}
	
	//The triple shot ability - Create 2 extra nodes to shoot from
	void tripleShot()
	{
		//Spawn nodes at position of bulletnode
		Transform secondaryNode1 = Instantiate (node1, transform.position + Vector3.right, transform.rotation) as Transform;
		secondaryNode1.transform.parent = playerBulletNode;
		
		Transform secondaryNode2 = Instantiate (node2, transform.position + Vector3.left, transform.rotation)as Transform;
		secondaryNode2.transform.parent = playerBulletNode;	
		
		//Create Triple Shot Effect
		Transform tripleShotField = Instantiate (tripleShotEffect, transform.position, transform.rotation) as Transform;
		tripleShotField.transform.parent = playerDrone;
	}

	
	//Activate Razor ability
	public void razorActivate()
	{
		 if (harbingerBoolean == false && titanBoolean == false)
		{
			//Start the razor timer
			razorTimer = razorMaxTimer;
			
			//Activate the razor boolean but ensure every other weapon is off
			razorBoolean = true;
			harbingerBoolean = false; 		
			titanBoolean = false; 	
		}
		
	}
	
	//The Razor gun - Shoot saw blades that bounces off everything and destroys any enemy it touches
	void razor()
	{
		if (razorTimer >= 0.0f)
		{
			//Send message to player status to activate razor function 
			GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("razorAbilityStatus", true);
			//Show Text to player that they aquired this ability
			razorText.gameObject.SetActive (true);
			pressRMBText.gameObject.SetActive (true);
			//Ensure other secondary weapon text is off
			harbingerText.gameObject.SetActive (false);
			titanText.gameObject.SetActive (false);
		}
		else
		{
			//Send message to player status to deactivate razor function 
			GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("razorAbilityStatus", false);
			razorBoolean = false; 
			//Turn off text
			razorText.gameObject.SetActive (false);
			pressRMBText.gameObject.SetActive (false);
		}
			
	}
	
	
	//This function is called when harbinger gun is activated
	public void harbingerActivate()
	{
		if (razorBoolean == false && titanBoolean == false)
		{
			//Activate the harbinger timer
			harbingerTimer = harbingerMaxTimer;
			
			//Ensure all other weapons except harbinger is off
			razorBoolean = false;
			harbingerBoolean = true; 
			titanBoolean = false; 
		}
	}
	
	//The Harbinger Gun - Shoots a pulse shot that does not destroy itself when it collides with an enemy
	void harbinger()
	{
		if (harbingerTimer >= 0.0f)
		{
			//Send message to player status to activate harbingers
			GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("harbingerAbilityStatus", true);
			//Show Text to player that they aquired this ability
			harbingerText.gameObject.SetActive (true);
			pressRMBText.gameObject.SetActive (true);
			//Ensure other secondary weapon text is off
			titanText.gameObject.SetActive (false);
			razorText.gameObject.SetActive (false);
		}
		else
		{
			//Send message to player status to deactivate harbingers
			GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("harbingerAbilityStatus", false);
			//Turn off text
			harbingerText.gameObject.SetActive (false);
			pressRMBText.gameObject.SetActive (false);
			harbingerBoolean = false;
		}
	}	
	
	//Activate Titan Gun
	public void titanActivate()
	{
		 if (razorBoolean == false && harbingerBoolean == false)
		{
			titanTimer = titanMaxTimer;
			razorBoolean = false;
			harbingerBoolean = false; 
			titanBoolean = true; 
		}
	}
	
	//The Titan Gun - Shoots a titan wave that destroys all enemies insides its wave.
	void titan()
	{
		if (titanTimer >= 0.0f)
		{
			//Send message to player status to activate Titan Waves
			GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("titanAbilityStatus", true);
			//Show Text to player that they aquired this ability
			titanText.gameObject.SetActive (true);
			pressRMBText.gameObject.SetActive (true);
			//Ensure other secondary weapon text is off
			razorText.gameObject.SetActive (false);
			harbingerText.gameObject.SetActive (false);
		}
		else
		{
			//Send message to player status to deactivate titan Waves
			GameObject.FindGameObjectWithTag ("playerDrone").SendMessage ("titanAbilityStatus", false);
			//Turn off text
			titanText.gameObject.SetActive (false);
			pressRMBText.gameObject.SetActive (false);
			titanBoolean = false;
		}
	}
}
