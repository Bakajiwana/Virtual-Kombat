using UnityEngine;
using System.Collections;

public class mainCameraScript : MonoBehaviour 
{
	//Smooth follow script reference:
	//http://answers.unity3d.com/questions/30830/camera-setup-for-top-down-shooters.html
	
	//Camera Variables
	public float m_FollowRate = 10f;
	public float m_FollowHeight = 20f;
	
	//Player variables
	public Transform playerDrone;
	
	// Update is called once per frame
	void LateUpdate () 
	{
		GameObject [] players = GameObject.FindGameObjectsWithTag ("playerDrone");
		if (players.Length >= 1)
		{
			transform .position = Vector3.Lerp(transform.position, playerDrone.transform.position + new Vector3(0f, m_FollowHeight, 0f), Time.deltaTime * m_FollowRate);
	
		}
	}
}
