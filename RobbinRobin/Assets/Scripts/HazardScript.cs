/*Hazard Script
*
* Place on a hazard sprite with a Box Collider 2D component with "is Trigger" checked
* Set the amount of damage that the hazard does upon entering
* Can be modified to do damage over time while the player stands in the hazard
*
* Created by Morgan Wagnon for Guildhall Academy
*/
using UnityEngine;
using System.Collections;

public class HazardScript : MonoBehaviour {

	//Variables that point to the PlayerController and health controller
	private HealthController healthController;
	private PlayerController playerController;

	//Start active/enabled
	public bool startEnabled = true;

	//Trigger only once and disable
	public bool triggerOnce = false;

	//Damage per second while the player is standing in the hazard. Set this to zero if you do not want gradual damage
	public float damagePerTime = 25.0f;

	//Hurt collider every timeBetweenDamage seconds
	public float timeBetweenDamage = 1.0f;

	//Tracks now long since last applied damage
	private float damageTimer = 0.0f;

	//Only affect the player?
	public bool onlyHurtPlayer = true;

	//This runs when the sprite is rendered into the screen
	void Start () 
	{

	}

	//Runs every frame
	void Update()
	{

	}

	//When something collides with the hazard, store the thing that collided with it in the variable "other"
	//For OnTiggerStay to fire, the player has to keep standing in the hazard
	void OnTriggerStay2D (Collider2D collider)
	{
		//Subtract time since last frame from time remaining until applying damage
		damageTimer -= Time.deltaTime;

		//Debug.Log (damageTimer);

		//Initialize HealthController
		healthController = collider.gameObject.GetComponent<HealthController>();

		//If the collider has a healthController
		if (healthController != null)
		{
			//Check if collider = player and onlyHurtPlayer or if it is notHurtPlayer
			if ((collider.gameObject.tag == "Player" && onlyHurtPlayer) || !onlyHurtPlayer) 	
			{
				if (startEnabled)
				{
					//If enough time has passed since damaging the player
					if (damageTimer <= 0.0f) 
					{
						//Reset time since damage last applied
						damageTimer = timeBetweenDamage;

						//Check if the player is invulnerable, otherwise don't hurt him
						if (!healthController.isInvulnerable)
						{
							//If object is still alive
							if(healthController.health > 0.0f)
							{
								//If health is more than damage
								if(healthController.health > damagePerTime)
									//Apply full damage
									healthController.health -= damagePerTime;
								else
									//Set health = 0
									healthController.health = 0.0f;
								
								//Process the hit in coordination with the HealthController
								ProcessHit(collider);
							}
						}
					
					}
				}
			}
		}
	}

	//When something collides with the hazard, store the thing that collided with it in the variable "other"
	//For OnTiggerStay to fire, the player has to keep standing in the hazard
	void OnTriggerExit2D (Collider2D collider)
	{
		//Initialize the time between applying damage
		damageTimer = 0.0f;
	}

	//Process the hit in coordination with the HealthController
	void ProcessHit(Collider2D collider)
	{
		//Apply damage only if health is above 0
		if(healthController.health > 0)
		{
			//Update animator variable
			healthController.anim.SetFloat ("health", healthController.health);
			
			//Play sound
			healthController.Play(healthController.hurtSound);
			
			//Play hurt anim
			healthController.anim.SetTrigger ("hurt");
			
			//If pause input on hurt
			if(healthController.pauseInputOnHurt)						
			{
				//Set animator variables
				healthController.anim.SetBool ("wait", true);
				
				//Set time to wait until accepting key imputs
				healthController.anim.SetFloat ("waitTime",Time.time + healthController.anim.GetCurrentAnimatorStateInfo(0).length);
			}
		}
		//If the target is dead, set it so
		if(healthController.health <= 0)
		{
			healthController.isDead = true;
		}

		//If trigger once, disable collision
		if (triggerOnce)
		{
			this.collider2D.enabled = false;
			Destroy (this.gameObject, timeBetweenDamage/2.0f);
		}
	}
}
