using UnityEngine;
using System.Collections;

public class playerStatusScript : MonoBehaviour 
{
	
	//Explosion variable
	public Transform playerExplosion;
	
	//playerBullet node is where bullets are spawned from
	public Transform playerBulletNode;
	
	//The playerBullet is a prefab created when the fire button is pressed
	public Transform playerBullet;
	
	//Muzzle Flash Variables
	public Transform playerDrone;
	public Transform playerMuzzleFlash;
	
	//Rapid Fire Variables
	public float fireRate = 0.1f;
	private float nextFire = 0.0f;
	
	//Overheat variables
	public static int overHeatCoolDown = 30;
	public static int overHeatMin = 0;
	public static int heat = 0; 
	private bool playerOverHeat = false;
	private bool tripleShotStatus = false;
	public TextMesh overHeatWarningText;
	
	//Overheat transforms 
	public Transform overHeatSteam;
	
	//Razor Gun Variables 
	public Transform playerRazor; 
	private bool razorBoolean = false; 
	public float razorFireRate = 0.3f;
	public float razorSpeed = 4000.0f;
	
	//Harbinger Gun Variables
	public Transform playerHarbinger; 
	private bool harbingerBoolean = false;
	public float harbingerFireRate = 1f;	
	public float harbingerSpeed = 3000.0f;
	
	//Titan Wave Gun Variables
	public Transform playerTitan;
	private bool titanBoolean = false;
	public float titanFireRate = 2.5f; 
	public float titanSpeed = 500.0f; 
	
	//Drop Mortar Beacon Variables
	public Transform mortarBeacon;
	

	// Use this for initialization
	void Start () 
	{
		//Start the coroutine for cooling down the heat variable
		StartCoroutine (overHeat());
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Create the fire button using the left mouse button where the drone shoots bullets
		//Reference: Tim Dawson's programming magic and some tweaks using the GetButton instead of GetButtonDown
		if (!playerOverHeat)
		{
			if (Input.GetButton("Fire1") && Time.time > nextFire)
			{
				//Rapid firing time
				nextFire = Time.time + fireRate;
				//loop through the children of the bulletnode and create a bullet for each node
				foreach (Transform child in playerBulletNode)
				{
					//create a bullet, assign its transform to a variable, then set the new bullet's firevector
					Transform bullet = Instantiate(playerBullet, child.position, child.rotation) as Transform;
					bullet.GetComponent<playerBulletScript>().FireVector = rigidbody.velocity;
					//Create Muzzle Flash
					Transform muzzleFlash = Instantiate (playerMuzzleFlash, transform.position,transform.rotation) as Transform;
					muzzleFlash.transform.parent = playerDrone;
					//For every bullet fired increase the heat
					heat++;
				}
			}
		}
		
		//If razor gun secondary weapon is set to true than enable the right click button fire
		if(razorBoolean == true)
		{
			//If right click button is down 
			if (Input.GetButton("Fire2") && Time.time > nextFire)
		{
				//Rapid firing time
				nextFire = Time.time + razorFireRate;
				//loop through the children of the bulletnode and create a bullet for each node
				foreach (Transform child in playerBulletNode)
				{
					//launch a razor, assign its transform to a variable
					Transform razor = Instantiate(playerRazor, child.position, child.rotation) as Transform;
					razor.rigidbody.AddForce (transform.forward * razorSpeed);
				}
			}	
		}
		
		if (harbingerBoolean == true)
		{	
			//If harbinger ability is activated ~Insert Harbinger Behaviour here~ 
			if (Input.GetButton ("Fire2") && Time.time > nextFire)
			{
				//Rapid Firing Time
				nextFire = Time.time + harbingerFireRate;
				//loop through the children of the bulletnode and create a bullet for each node
				foreach (Transform child in playerBulletNode)
				{
					//launch the Harbinger
					Transform rocket = Instantiate(playerHarbinger, child.position, child.rotation) as Transform;
					rocket.rigidbody.AddForce (transform.forward * harbingerSpeed);
				}
			}
		}
		
		if (titanBoolean == true)
		{ 
			if (Input.GetButton ("Fire2") && Time.time > nextFire)
			{
				//Rapid Firing Time
				nextFire = Time.time + titanFireRate;
				//loop through the children of the bulletnode and create a titan wave for each node
				foreach (Transform child in playerBulletNode)
				{
					//launch the Titan Wave
					Transform titanWave = Instantiate(playerTitan, child.position, child.rotation) as Transform;
					titanWave.rigidbody.AddForce (transform.forward * titanSpeed);
				}
			}
		}
		
		
		//If Gun over heats
		if(heat >= overHeatCoolDown)
		{
			//Set over heat boolean to true
			playerOverHeat = true;
			//Instantiate a steam feedback as a child
			//PROBLEM SOLVED: steam spawns wrong rotation. Had to adjust in game space and copy paste initial rotation.
			Transform steam = Instantiate (overHeatSteam, transform.position, Quaternion.Euler(277.8374f,128.5436f,225.5029f)) as Transform;
			steam.transform.parent= playerDrone;
			
		}
		//else if the gun is at minimum heat
		else if(heat == overHeatMin)
		{
			//set the over heat boolean to false
			playerOverHeat = false;
		}
		
		//If heat is more than an x amount then display to player that Overheat is imminent
		if(heat > 23)
		{
			overHeatWarningText.gameObject.SetActive (true);
		}
		else
		{
			overHeatWarningText.gameObject.SetActive (false);
		}
		
		
		
		//If tripleShotStatus is true there should be no heat produced. (Unlimited shots)
		if(tripleShotStatus == true)
		{
			heat = 0;	
		}
	}
	
	
	//Function is called when razor gun is activated
	public void razorAbilityStatus (bool _razorStatus)
	{
		//Activate the razor boolean
		razorBoolean = _razorStatus; 	
	}
	
	//Function is called when the harbinger gun is activated 
	public void harbingerAbilityStatus (bool _harbingerStatus)
	{
		harbingerBoolean = _harbingerStatus;
	}
	
	//Function is called when the titan gun is activated
	public void titanAbilityStatus (bool _titanStatus)
	{
		titanBoolean = _titanStatus; 	
	}
	
	//The Death Function, used for death....
	void Death()
	{
		//Create an explosion
		Instantiate (playerExplosion, transform.position, transform.rotation);
			
		//destroy gameobject
		Destroy (gameObject);			
	}
	
	//Function called during a collision
	void OnCollisionEnter (Collision other)
	{
		//Physics problem where drone leaves game area.
		if (other.gameObject.tag == "boundary")
		{
			//Kill player and game over
			GameObject.FindGameObjectWithTag ("levelManager").SendMessage ("applyPlayerDamage", 200);
		}
	}
	
	//Use enumerator to cool down the heat of the gun
	IEnumerator overHeat()
	{
		while(true) // loops forever
		{
			if (heat > overHeatMin) //if the heat is above 0
			{
				heat--; //cool down rate heat - 1 every 0.2 second
				yield return new WaitForSeconds(0.2f);
			} else {
				//if the heat isn't over 0 then don't do anything
				yield return null;
			}
		}
	}
	
	public void dropBeacon()
	{
		Instantiate (mortarBeacon, new Vector3(transform.position.x, 0.1f, transform.position.z), transform.rotation);
	}
	
	
	//This function is called when the triple shot ability is on so that overheat won't increase
	public void tripleShotHeatControl(bool _status)
	{
		tripleShotStatus = _status;
	}
}
