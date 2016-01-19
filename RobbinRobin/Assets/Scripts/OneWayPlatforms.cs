//One Way Platforms
//
//Allows players to move up and through one
//way platforms
//
//From Fists of Freedom and
//Modified by James Bowling
//-------------------------------------------------
using UnityEngine;
using System.Collections;

public class OneWayPlatforms : MonoBehaviour
{
	// Variable initialization
	private GameObject playerObject;
	private Vector2 playerVelocity;
	public bool collideWithEnemy;   // This is for if the enemy should run into a one-way platform that is front of them or walk in front of it
	public bool startEnabled = true;


	// This initializes the basic components from the user input in the Inspector tab
	void Start ()
	{
		collideWithEnemy = false;
		playerObject = GameObject.FindGameObjectWithTag("Player");  	// Tags come from the "Tag" drop-down under the Inspector for a GameObject
        playerVelocity = playerObject.rigidbody2D.velocity;	// The speed of the player moving vertically
		this.collider2D.enabled = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
        playerVelocity = playerObject.rigidbody2D.velocity;  // This checks the speed of the player vertically constantly

		// If player is moving vertically (upwards) then the collision is turned off, otherwise it is turned on
		if (playerVelocity.y > 0.1f && !collideWithEnemy)
		{
			if (startEnabled == true)
				this.collider2D.enabled = false;
		}
		else
			this.collider2D.enabled = true;			
	}

	// These two are to check if they have collided with a platform that is also a trigger
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Enemy")
			collideWithEnemy = true;
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.tag == "Enemy")
			collideWithEnemy = false;
	}

	// These two are to check if they have already collided with a platform with collision
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Enemy")
			collideWithEnemy = true;
	}

	void OnCollisionExit2D(Collision2D col)
	{
		if (col.gameObject.tag == "Enemy")
			collideWithEnemy = false;
	}
}
