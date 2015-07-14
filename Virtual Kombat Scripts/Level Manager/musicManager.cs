using UnityEngine;
using System.Collections;

public class musicManager : MonoBehaviour 
{
	//Toggle through music script
	//reference: http://answers.unity3d.com/questions/27017/toggling-through-music.html (its in javascript)
	public AudioClip[] mainPlayList;
	public AudioClip gameOverClip;
	public AudioClip shieldSurplusClip;
	
	// Use this for initialization
	void Start () 
	{
		//At start choose a random song
		mainSongPlay (Random.Range (1,6));
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void mainSongPlay(int _randomSong)
	{
		/*	
		Reference of all songs used:
		•	Adrenaline FX by axeFX 
		•	Crash Bandicoot - Invincible Aku aku by Mutato Muzika 
		•	Street Fighter IV – Snowy Rail Yard Stage (Russia) by Hideyuki Fukaswa 
		•	Street Fighter IV – Drive-In At Night Stage (USA) by Hideyuki Fukaswa 
		•	Street Fighter IV – Training Stage by Hideyuki Fukaswa 
		•	Street Fighter IV – Cruise Ship Stem Stage (Europe) by Hideyuki Fukaswa
		•	Street Fighter IV – Crowded Downtown Stage (China) by Hideyuki Fukaswa
		•	Mass Effect – Therum Battle by Jack Wall and Sam Hullick
		•	Mass Effect – Virmire Battle by Jack Wall and Sam Hullick
		•	Mass Effect -  Saren by Jack Wall and Sam Hullick
		*/

		//If a song is currently playing then stop that
		if (audio.isPlaying)
		{
			audio.Stop();
		}
		
		//Proceed to choosing a song 
		audio.clip = mainPlayList[_randomSong];
		audio.Play ();
	}
	
	public void gameOverSong()
	{
		//If a song is currently playing then stop that
		if (audio.isPlaying)
		{
			audio.Stop();
		}
		
		//Play the game Over song which is the Mass Effect - Saren Song
		audio.clip = gameOverClip;
		audio.Play ();
	}
	
	public void shieldSurplusSong()
	{
		//If a song is currently playing then stop that
		if (audio.isPlaying)
		{
			audio.Stop();
		}
		
		//Play the invincible aku aku song
		audio.clip = shieldSurplusClip;
		audio.Play ();
	}
}
