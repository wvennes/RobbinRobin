/*Kill Volume Script
*
* Place on a kill volume sprite with a Box Collider 2D component with "is Trigger" checked
* Set the amount of damage that the kill volume does upon touching it
*
* Based on hazard script from Morgan Wagnon
* Modifications made by James Bowling
*/
using UnityEngine;
using System.Collections;

public class KillVolumeScript : MonoBehaviour
{
	
	//Variables that point to the PlayerController and health controller
	private HealthController healthController;
	
	//Start active/enabled
	public bool startEnabled = true;

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
	

	//For a kill volume the player or enemy only has to collide once with it
	void OnTriggerEnter2D (Collider2D collider)
	{

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
					//Check if the player is invulnerable, otherwise don't hurt him
					if (!healthController.isInvulnerable)
					{
						//If object is still alive
						if(healthController.health > 0.0f)
						{
							healthController.health = 0.0f;
							healthController.isDead = true;
						}
					}
				}
			}
		}
	}
}
