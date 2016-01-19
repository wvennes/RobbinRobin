//Powerup Controller
//
//Handles powerup functionality for pickups
//
//by Myque Ouellette
//with modifications by James Bowling
//--------------------------------------------
using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour
{
    //Variable that points at the Player
    private GameObject player;

    //Variable that points to the player projectile prefab
    private Transform playerProjectilePrefab;

	//Original damage amount
	private int originalDamage;

    //Variable that points to enemies
    GameObject enemy;

    //Create an enuration to specify which stat to modify, speed or health
    public enum whatToModify
    {
        Speed,
        Invulnerability,
        Damage,
    }

    //Creates the variable for the enum (required for enum to show in the inspector)
    public whatToModify statModified;

    public float newMaxSpeed = 15.0f;
    private Vector2 originalMaxSpeed;
    public int damageMultiplier = 1;

    private float originalEnemyHealth;
    public float powerUpTime = 5.0f;

    //This is the trigger for turning on invulnerability in the Health Controller
    private bool invulnerableOn;

    //How long this powerup lasts in seconds
    [HideInInspector]
	public float powerEndTime = 4.0f;

    //Is active on startup
    public bool isPowerUpActive = false;

    //Pointer to Controllers and Renderer
    private HealthController healthController;
    private PlayerController playerController;

    //private ProjectileController projectileController;
    private SpriteRenderer spriteToHide;

    // Use this for initialization
    void Start()
    {
		//Find the player and point this variable
        //to them
        player = GameObject.FindWithTag("Player");

        //Initialize a pointers to controllers and renderer
        healthController = player.GetComponent<HealthController>();
        playerController = player.GetComponent<PlayerController>();

        spriteToHide = this.GetComponent<SpriteRenderer>();

        //Save the original max speed
        originalMaxSpeed = playerController.maxSpeed;

        //Set invulnerable on flag
        invulnerableOn = healthController.isInvulnerable;

		//Get a pointer to the player projectile prefab
		playerProjectilePrefab = player.GetComponent<WeaponController>().projectilePrefab;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //If active check to see if it needs to end
        if (isPowerUpActive)
        {
            if (Time.time > powerEndTime)
			{
				EndEffect();
			}
        }
    }

    //Called on collision with triggers
    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        //Increase player stats if is pickup and has not been awarded yet
        if (!isPowerUpActive)
        {
            //If player
            if (otherCollider.gameObject.tag == "Player")
            {
				//Set end time
                powerEndTime = Time.time + powerUpTime;

                //Set active flag
                isPowerUpActive = true;

                //Hide this sprite by turning off its renderer
                spriteToHide.enabled = false;

                //If speed boost, change the speed in the player controller
                if (statModified == whatToModify.Speed)
                    playerController.maxSpeed = new Vector2(newMaxSpeed, originalMaxSpeed.y);
                //If setting player to invulnerable, turn it on in the health controller
                if (statModified == whatToModify.Invulnerability)
                    healthController.isInvulnerable = true;
                //If increasing player's damage through its projectile
                if (statModified == whatToModify.Damage)
                {
					//Store the original damage value
					originalDamage = playerProjectilePrefab.GetComponent<ProjectileController>().damage;

					//Change the projectile damage by damageMultiplier
					playerProjectilePrefab.GetComponent<ProjectileController>().damage = 
						playerProjectilePrefab.GetComponent<ProjectileController>().damage * damageMultiplier;
                }
            }
        }
    }

    void EndEffect()
    {
        //Turn is active off
        isPowerUpActive = false;

        //If speed boost, turn it off
        if (statModified == whatToModify.Speed)
            playerController.maxSpeed = new Vector2(originalMaxSpeed.x, originalMaxSpeed.y);
        //If setting player to invulnerable, turn it off
        if (statModified == whatToModify.Invulnerability)
            healthController.isInvulnerable = false;
        //If damage boost, turn it back to original damage
        if (statModified == whatToModify.Damage)
        {
			//Reset damage to original value
			playerProjectilePrefab.GetComponent<ProjectileController>().damage = originalDamage;
        }
		
		//Destroy this object
		GameObject.DestroyObject(this.gameObject, 0f);
	}
	
	//Script that actually plays sounds
    public void Play(AudioClip sound)
    {
        //If sound exists
        if (sound != null)
            // As it is not 3D audio clip, position doesn't matter.
            AudioSource.PlayClipAtPoint(sound, transform.position);
    }
}
