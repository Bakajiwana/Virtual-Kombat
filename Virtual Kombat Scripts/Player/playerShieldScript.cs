using UnityEngine;
using System.Collections;

public class playerShieldScript : MonoBehaviour 
{	
	//Alpha Variables
	public float shieldAlpha;
	private float shieldMaxAlpha = 1f;
	private float rechargeRate = 0.01f;
	private float regenerationSpeed = 0.3f;

	// Use this for initialization
	void Start () 
	{
		//Start coroutine to regenerate alpha of shield
		StartCoroutine (regenerateAlpha());
	}
	
	// Update is called once per frame
	void Update () 
	{ 	
		shieldAlpha = (float)levelManagerScript.playerShield / 100f;
		//Control the alpha of the shield
		renderer.material.color = new Color(0f, 255f, 255f,shieldAlpha);
	}

	
	IEnumerator regenerateAlpha()
	{
		while(true) // loops forever
		{
			if (shieldAlpha < shieldMaxAlpha)
			{
				shieldAlpha += rechargeRate; //Recharge Rate
				yield return new WaitForSeconds(regenerationSpeed);
			} else {
				yield return null;
			}
		}
	}		
}
