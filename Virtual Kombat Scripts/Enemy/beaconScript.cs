using UnityEngine;
using System.Collections;

public class beaconScript : MonoBehaviour {
	
	//Spawn Variables (note: going to use the meleeSpawnEffect just to re use)
	public Transform beaconSpawnEffect;
	
	void Start()
	{
		Instantiate (beaconSpawnEffect,transform.position, transform.rotation);
	}

	//Collision function is called when object collides on something
	void OnTriggerEnter (Collider other)
	{
		//If hit by Mortar shell
		if (other.gameObject.tag == "enemyShell")
		{	
			//Destroy all mortar shells to prevent referenceException errors			
			GameObject[] shells = GameObject.FindGameObjectsWithTag ("enemyShell");
			
			foreach (GameObject enemyShell in shells)
			{
				Destroy (enemyShell);
			}
			
			//Destroy all other markers because leftovers that just sit there so DESTROY THEM ALL
			GameObject[] markers = GameObject.FindGameObjectsWithTag ("mortarBeacon");
			
			//Destroy all markers
			foreach (GameObject mortarBeacon in markers)
			{
				Destroy (mortarBeacon);	
			}
			
			
			//Destroy Self
			Destroy (gameObject);
		}
	}
}
