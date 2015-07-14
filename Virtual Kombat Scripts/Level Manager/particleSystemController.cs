using UnityEngine;
using System.Collections;

public class particleSystemController : MonoBehaviour 
{
	//This script will just destroy off particle systems within their time limit
	public bool forceBlast;
	public bool muzzleFlash;
	public bool enemyExplosion;
	public bool enemyShellExplosion;
	public bool dashEffect;
	public bool shieldSurplusEffect;
	public bool healthRestoreEffect;
	public bool shieldOverloadEffect;
	public bool jammerEffect;
	public bool tripleShotEffect;
	public bool pickUpEffect;
	
	//Variables that will destroy other objects that aren't particle to save scripts
	public bool tripleShotNode;
	
	
	//Timer Variables
	public float forceBlastLength = 2f;
	public float muzzleFlashLength = 0.1f;
	public float enemyExplosionLength = 1f;
	public float enemyShellExplosionLength = 0.6f;
	public float pickUpEffectLength = 0.6f;
	
	//Prevent Rotation variables
	//For the Shield Overload effect
	private Quaternion shieldOverLoadRotation;
	private Quaternion tripleShotRotation;

	// Use this for initialization
	void Start () 
	{
		if(forceBlast)
		{
			Destroy (gameObject,forceBlastLength);
		}
		
		if(muzzleFlash)
		{
			Destroy (gameObject, muzzleFlashLength);	
		}
		
		if(enemyExplosion)
		{
			Destroy (gameObject, enemyExplosionLength);
		}
		
		if(enemyShellExplosion)
		{
			Destroy (gameObject, enemyShellExplosionLength);	
		}
		
		if(dashEffect)
		{
			Destroy(gameObject, playerAbilityScript.dashTimer);
		}
		
		if(shieldSurplusEffect)
		{
			Destroy (gameObject, playerAbilityScript.shieldSurplusTimer);	
		}
		
		if(healthRestoreEffect)
		{
			Destroy (gameObject, playerAbilityScript.healthRestoreTimer);
		}
		
		if(shieldOverloadEffect)
		{
			Destroy (gameObject, playerAbilityScript.shieldOverloadTimer);
			//Also stop the effect from rotating with the parent
			//Prevent child rotation by returning the first rotation value
			shieldOverLoadRotation = transform.rotation;
		}
		
		if(jammerEffect)
		{
			Destroy (gameObject, playerAbilityScript.jammerTimer);
		}	
		
		if(tripleShotEffect)
		{
			Destroy (gameObject, playerAbilityScript.tripleShotTimer);
			//Prevent rotation
			tripleShotRotation = transform.rotation;
		}
		
		if(pickUpEffect)
		{
			Destroy (gameObject, pickUpEffectLength);	
		}
		
		if(tripleShotNode)
		{
			Destroy (gameObject, playerAbilityScript.tripleShotTimer);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{		
		if(shieldOverloadEffect)
		{
			//apply the first rotation value and freeze rotation of shield overload
			transform.rotation = shieldOverLoadRotation;
		}
		
		if(tripleShotEffect)
		{
			//Apply the first rotation value and freeze rotation of triple shot effect
			transform.rotation = tripleShotRotation;
		}
	}
}
