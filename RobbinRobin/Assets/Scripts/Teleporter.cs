//Inventory Controller
//
//Handles all inventory related functionality
//as well as sending information to the HUD
//
//by Michael McCoy
//with help from Morgan Wagnon
//--------------------------------------------
using UnityEngine;
using System.Collections;

public class Teleporter : MonoBehaviour 
{
	//Variable that points at the Player
	GameObject player;
	public GameObject targetDestination;
	private Vector3 targetPosition;

	public bool startEnabled = true;
	private bool canTeleport;
	public bool teleportViaButton = false;
	public bool teleportViaButtonActive = false;

	public float destinationXOffset = 0.0f;
	public float destinationYOffset = 0.0f;

	//putting these in for effects later
	public float teleportTime = 0.0f;
	public bool isInTransition = false;
	private SpriteRenderer teleportSprite;
	private float teleportDoneTime = 0.0f;


	// Use this for initialization
	void Start () 
	{
		//Find the player and point this variable
		//to them
		player = GameObject.FindWithTag ("Player");

		//Initialize a pointer the the player controller script
		//playerController = player.GetComponent<PlayerController> ();

		targetPosition = targetDestination.gameObject.transform.position;
		canTeleport = startEnabled;
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (isInTransition) 
		{
			if( Time.time > teleportDoneTime )
			{
				EndEffect();	
			}

		}

		if( teleportViaButton && teleportViaButtonActive && Input.GetButtonDown("Fire2") )
		{
			Teleport();
		}
	}

	//Called on collision with triggers
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		//Increase inventory/player stats if is pickup and has not been awarded yet
		if(!isInTransition && canTeleport)
		{
			if(otherCollider.gameObject.tag == "Player")
			{
				teleportViaButtonActive = true;

				if( !teleportViaButton )
				{
					Teleport();
				}
			}
		}
	}

	void OnTriggerExit2D(Collider2D otherCollider)
	{
		if(otherCollider.gameObject.tag == "Player")
		{
			teleportViaButtonActive = false;
		}
	}


	void EndEffect()
	{
		isInTransition = false;
		//GameObject.DestroyObject (this.gameObject, 0f);
		//playerController.maxSpeed = new Vector2(originalMaxSpeed.x, originalMaxSpeed.y);

	}

	void Teleport()
	{
		teleportDoneTime = Time.time + teleportTime;
		isInTransition = true;
		//spriteToHide.enabled = false;
		
		//currently just teleport, later do fade out, then teleport and fade in based on teleportTime
		player.gameObject.transform.position = targetPosition;
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
