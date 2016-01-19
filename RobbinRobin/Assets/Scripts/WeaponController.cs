//Weapon Controller
//
//Allows players and AIs to fire projectiles
//
//by Michael McCoy
//with code from tutorials
//--------------------------------------------
using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
	//Projectile Prefab
	public Transform projectilePrefab;
	
	//How long between shots
	public float shootCooldown = 1.0f;
	
	//Time when can shoot again
	private float timeUntilNextShot;

	//How long to ignore input on shoot button
	public float pauseInputOnShootTime = 1.0f;

	//Projectile location offsets
	public float projectileXOffset = 0.0f;
	public float projectileYOffset = 0.0f;
	
	//Store who owns this component
	bool isPlayer = false;
	
	//Store the facing of the owner
	bool facingRight = false;
	
	//Delay until projectile appears
	public float projectileSpawnDelay = 0.5f;
	
	//Internal time until instantiating projectile
	float createProjectileTime;
	
	//Bollean to track if waiting to shoot
	bool waitingToShoot = false;

	//If false, AI will not shoot until something says it can
	public bool shootActive = false;

	//Link to animator
	Animator anim;

	//Define shoot sound
	public AudioClip shootSound;

	//Start active/enabled
	private bool startEnabled = true;

	public enum InventoryItem
	{
		None,
		Pickup1Name,
		Pickup2Name,
		Pickup3Name
	}

	//Inventory Ammo for shooting
	public InventoryItem ammoPickup;

	// Use this for initialization
	void Start () 
	{
		//Finds our animator and stores its location
		//in our variable
		anim = GetComponent<Animator> ();
		
		//Check owner of component
		if(this.gameObject.tag == "Player")
			isPlayer = true;
		else
			//Automatically make gameObject wait
			//one cooldown cycle b4 AI shoots
			StartCooldown();	
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Make this start enabled = player/ai start enabled
		if(isPlayer)
		{
			//startEnabled = this.gameObject.GetComponent<PlayerController>().startEnabled;
		}
		else
		{
			startEnabled = this.gameObject.GetComponent<AIController>().startEnabled;
		}

		//Reset the animator wait variable when time
		//runs out
		if (Time.time >= anim.GetFloat ("waitTime"))
			anim.SetBool ("wait", false);
		
		//Wait until projectileFireDelay expires before
		//creating a projectile in the world
		if(Time.time >= createProjectileTime && waitingToShoot)
		{
			//Play sound
			Play(shootSound);

			//Create projectile in the world
			CreateProjectile();
			
			//Turn off waitingToShoot
			waitingToShoot = false;
		}
		//Is this on the player?
		if(isPlayer)
		{
			//Can only shoot if the player is on the ground
			if(anim.GetBool ("grounded"))
			{
				//Check shoot button presses and cooldown time
				//would also use LeftControl but it's tied to
				if (Input.GetButtonDown ("Fire1")
				    && Time.time >= timeUntilNextShot)
				{
				 	//If this weapon does not use ammo, shoot
					if(ammoPickup == InventoryItem.None)
					{
						//Fire projectile
				 		Shoot();
				 		//Start the cool down timer
						StartCooldown();
					}
					else
					{
						//Get a pointer to this objects inventory controller
						InventoryController inv = gameObject.GetComponent<InventoryController>();

						if (inv != null)
						{
							//If this is the ammo inventory and the controller exists
							if(ammoPickup == InventoryItem.Pickup1Name)
							{
								//If there is ammo
								if(inv.Pickup1InitialValue >= 1.0f)
								{
									//Fire projectile
									Shoot();
									//Start the cool down timer
									StartCooldown();
									//Reduce ammo
									inv.Pickup1InitialValue--;
								}
								else
								{
									//Debug.Log("WeaponController: Not enough ammo");
								}
							}

							//If this is the ammo inventory and the controller exists
							if(ammoPickup == InventoryItem.Pickup2Name)
							{
								//If there is ammo
								if(inv.Pickup2InitialValue >= 1.0f)
								{
									//Fire projectile
									Shoot();
									//Start the cool down timer
									StartCooldown();
									//Reduce ammo
									inv.Pickup2InitialValue--;
								}
								else
								{
									//Debug.Log("WeaponController: Not enough ammo");
								}
							}

							//If this is the ammo inventory and the controller exists
							if(ammoPickup == InventoryItem.Pickup3Name)
							{
								//If there is ammo
								if(inv.Pickup3InitialValue >= 1.0f)
								{
									//Fire projectile
									Shoot();
									//Start the cool down timer
									StartCooldown();
									//Reduce ammo
									inv.Pickup3InitialValue--;
								}
								else
								{
									//Debug.Log("WeaponController: Not enough ammo");
								}
							}
						}
						else
						{
							//Debug.Log("WeaponController: No InventoryController");
						}
					}
				} 
			}
		}

		//If this is the AI make sure it is startEnabled or don't shoot
		if(startEnabled)
		{
			if(!isPlayer)
			{
				if( shootActive )
				{
					//If enough time has expired and the AI is not dead - shoot
					if(Time.time >= timeUntilNextShot && !anim.GetBool("dead"))
					{
						//Fire projectile
						Shoot();
						//Start the cool down timer
						StartCooldown();
					}
				}
			}
		}
	}
	
	//Sets the time at which another shot can be fired
	void StartCooldown()
	{
		//Set the time until next shot variable
		timeUntilNextShot = Time.time + shootCooldown;
	}
	
	//Start the fire a projectile process
	void Shoot()
	{
		//Animator health value
		anim.SetTrigger ("shoot");
		
		//Set time to wait until accepting key imputs
		anim.SetFloat ("waitTime",Time.time + pauseInputOnShootTime);
				
		//Set animator wait variable
		anim.SetBool ("wait", true);
		
		//Set time at which to create the projectile
		createProjectileTime = Time.time + projectileSpawnDelay;	
		
		//Set waitingToShoot var
		waitingToShoot = true;
	}

	//Create a projectile in the world
	void CreateProjectile()
	{
		//Temp offset variables
		float xOffset = projectileXOffset;
		float yOffset = projectileYOffset;
		
		//----------------------------------------------------
		//Create the projectile
		var projectileTransform = Instantiate(projectilePrefab)
								  as Transform;
				
		//Create a pointer the the projectiles controller				  
		ProjectileController projectile = projectileTransform.
				gameObject.GetComponent<ProjectileController>();
				
		//Get the facing of the owner
		if(isPlayer)
			facingRight = gameObject.GetComponent
						  <PlayerController>().facingRight;
		else
			facingRight = gameObject.GetComponent
						  <AIController>().facingRight;
		//----------------------------------------------------
		
		//Make sure the projectile was created properly
		if(projectile != null)
		{

			//Check the owners facing
			if(!facingRight)
			{
				//Reverse the projectile art
				projectile.Flip();
				
				//Reverse the projectiles velocity
				xOffset *= -1;
			}
			//ADD Y FLIPPING AND VELOCITY CODE
		}
		
		//Move the projectile to the correct place
		projectileTransform.position = new Vector3
				(this.transform.position.x + xOffset,
				 this.transform.position.y + yOffset,
				 this.transform.position.z);
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
