//Audio Volume
//
//Handles playing sounds on trigger enter and 
//continuing on trigger stay
//
//Must have AudioSource on same object
//
//by Myque Ouellette and Michael McCoy
//--------------------------------------------

using UnityEngine;
using System.Collections;

public class AudioVolume : MonoBehaviour {

	//Define sound
	public AudioClip soundToPlay;

	//Points to AudioSource component on this object 
	private AudioSource soundSource;

	//Volume scale 0.0f - 1.0f (0-100%)
	private float volume;

	//Is this sound looping?
	private bool loop;

	[HideInInspector]
	//Is there currently a sound playing?
	public bool isPlaying = false;

	//Should this sound stop on trigger exit?
	public bool stopOnTriggerExit = true;

	//Runs immediately on level start
	void Start()
	{
		//Find the AudioSource component on this object
		soundSource = gameObject.GetComponent<AudioSource> ();

		//Set the looping variable on the soundSource
		loop = soundSource.loop;
	}

	//Called when an object enters this trigger
	void OnTriggerEnter2D(Collider2D collider)
	{
		//If the sound is not playing and the object is the player
		//Start it playing
		if(!isPlaying && collider.gameObject.tag == "Player")
		{
			isPlaying = true;
			Play (soundToPlay, loop);
		}
	}

	//Called when an object exits this trigger
	void OnTriggerExit2D(Collider2D collider)
	{
		//If this sound is player, the sound is supposed to stop on trigger exit,
		//and the object exiting is the player then stop the sound
		if( isPlaying && stopOnTriggerExit && collider.gameObject.tag == "Player")
		{
			isPlaying = false;
			Stop (soundToPlay);
		}
	}
	
	//Script that actually plays sounds
	public void Play(AudioClip sound, bool loop)
	{
		//If sound exists
		if(sound != null)
		{
			if(loop)
			{
				//Debug.Log("afafafaf");
				//Play sound at specified volume 0.0f - 1.0f
				//AudioSource.PlayClipAtPoint(sound, transform.position, volumeZeroToOne);
				soundSource.clip = soundToPlay;
				soundSource.Play();
			}
			else
			{
				//Play sound at specified volume 0.0f - 1.0f
				soundSource.PlayOneShot(sound, soundSource.volume);
			}
		}
	}

	//Script that actually plays sounds
	public void Stop(AudioClip sound)
	{
		//If sound exists
		if(sound != null)
			// Stop the sound playing
			soundSource.Stop ();
	}
}
