//Pickup Controller
//
//Handles pickups and their functionality
//
//by SuperCrush with modifications by Michael McCoy
//-------------------------------------------------
using UnityEngine;
using System.Collections;

public class PickupController : MonoBehaviour 
{

	//Pointer to the Player object
	GameObject player;

	[HideInInspector]
	//Check to se if this has already been awarded to the player
	public bool awarded = false;

	//Define pickup sound
	public AudioClip pickupSound;

	//Use this for initialization
	void Start () 
	{
		//Point to Player object
		player = GameObject.FindGameObjectWithTag("Player");

		//DO SOMETHING WITH INVENTORY
		//Add one to the max number available if goal is to collect all
	}
	
	//Update is called once per frame
	void Update () 
	{
	
	}

	//Called when something collides with this object
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		//If Player
		if ( otherCollider.gameObject.tag == "Player" && !awarded )
		{
			//Play pickup sound
			Play(pickupSound);
		}
	}

	public void KillThySelf()
	{
		Destroy (gameObject);
	}

	//Script that actually plays sounds
	public void Play(AudioClip sound)
	{
		//If sound exists
		if(sound != null)
			// As it is not 3D audio clip, position doesn't matter.
			AudioSource.PlayClipAtPoint(sound, transform.position);
	}
}
