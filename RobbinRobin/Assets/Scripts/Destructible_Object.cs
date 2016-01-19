// Destructible Tiles Script
//
// Controls the delayed destruction of objects, and plays the destruction animation.
//
//by Jon Clark
//with code from tutorials and in class discussion

using UnityEngine;
using System.Collections;

public class Destructible_Object : MonoBehaviour
{
	//Time over which the player fades out
	private float fadeOutTime = 0.0f;
	
	//Time since death
	private float elapsedTime = 0.0f;
	
	//How long in seconds it takes for this sprite to
	//fade out after death
	private float fadeOnDeathTime;

	//Link to animator
	private Animator anim;

	//A variable to hold pointer to this objects sprite
	private SpriteRenderer mySprite;

	//Start active/enabled
	public bool startEnabled = true;
	
	//Allow players to shoot this
	public bool destroyWithProjectile = true;

	//Define death sound
	public AudioClip destructionSound;

	//How long to WaitForSeconds until destroying this object
	public float timeToWaitOnDeath = 1.0f;

	//Is this object dying?
	private bool isDying = false;


	//Called on level startup
	void Start()
	{
		//Finds our animator and stores its location
		//in our variable
		anim = GetComponent<Animator> ();

		//Get pointer to the sprite
		mySprite = gameObject.GetComponent<SpriteRenderer> ();
	}

	//Runs every frame
	void Update()
	{
		//Fade the sprite out when isDying
		if(isDying)
		{
			elapsedTime += Time.deltaTime;
			mySprite.color = new Color(mySprite.color.r,
			                           mySprite.color.g,
			                           mySprite.color.b,
			                           1.0f - elapsedTime/fadeOutTime);
		}

		if(!destroyWithProjectile && startEnabled)
		{
			destroyThySelf();
		}
	}

	//Fires when a trigger hits this object
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		
		if (otherCollider.gameObject.tag == "Player_Projectile")
		{ 
			if (destroyWithProjectile && startEnabled)
			{
				//Destroy the projectile
				GameObject.Destroy(otherCollider.gameObject, 0.0f);

				destroyThySelf();
			}
		}
	}

	//Kill yourself and play animations if available
	void destroyThySelf()
	{
		//If an Animator exists
		if(anim != null)
			//Set animator variables to play death anim
			anim.SetTrigger("death");
		
		//Play sound
		if(destructionSound != null)
			Play(destructionSound);
		
		//Set the timer for this object to destroy itself
		GameObject.Destroy (this.gameObject, timeToWaitOnDeath);
	
		//Set fadeOutTime
		fadeOutTime = timeToWaitOnDeath;
		
		//Set isDying = true
		isDying = true;
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
