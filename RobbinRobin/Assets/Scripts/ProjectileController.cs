//Projectile Controller
//
//Controls projectiles movement and life span
//Projectile MUST be a TRIGGER!
//
//by Michael McCoy
//with code from tutorials
//--------------------------------------------
using UnityEngine;
using System.Collections;

public class ProjectileController : MonoBehaviour 
{

	//Sets the max speed of the projectile
	public Vector2 maxSpeed = new Vector2(10,0);
	
	//Private variable to control movement
	Vector2 movement;
	
	//Vector2 variable to contain movement direction
	Vector2 direction = new Vector2(1,0);
	
	//How long till projectile kills itself
	public float lifeSeconds = 4.0f;
	
	//Is this a player projectile
	public bool isPlayerProjectile = false;
	
	//Amount of damage projectile causes
	public int damage = 25;	

	//Variable that points to the PlayerController
	private HealthController healthController;


	// Use this for initialization
	void Start () 
	{
		//Set the projectiles auto-death timer
		Destroy (gameObject,lifeSeconds);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Set the movement variable
		movement = new Vector2
		(
			maxSpeed.x * direction.x,
			maxSpeed.y * direction.y
		);
	}
	
	//Called at every fixed framerate frame
	//Use when dealing with rigidBody Physics
	void  FixedUpdate()
	{
		//Move the Object
		rigidbody2D.velocity = movement;
	}
	
	public void Flip()
	{
		//Handles the reversing of the facing
		Vector3 theScale = this.transform.localScale;
		theScale.x *= -1;
		this.transform.localScale = theScale;
		
		//Change the movement direction
		direction.x *= -1;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		//If the projectile is not from a player
		if(isPlayerProjectile == false)
		{
			//If the target is the player
			if(collider.gameObject.tag == "Player")
			{
				//Process the hit in coordination with the HealthController
				ProcessHit(collider);
			}
		}

		//If projectile is from player
		if(isPlayerProjectile == true)
		{
			//If what it hits is an Enemy
			if(collider.gameObject.tag == "Enemy")
			{
				//Process the hit in coordination with the HealthController
				ProcessHit(collider);
			}
		}
	}

	//Process the hit in coordination with the HealthController
	void ProcessHit(Collider2D collider)
	{
		//Initialize HealthController
		healthController = collider.gameObject.GetComponent<HealthController>();
		
		//If object is still alive
		if(healthController.health > 0)
		{
			//If health is more than damage
			if(healthController.health > damage)
				//Apply full damage
				healthController.health -= damage;
			else
				//Set health = 0
				healthController.health = 0.0f;
			
			//Destroy projectile
			Destroy (this.gameObject);
			
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
	}
}
