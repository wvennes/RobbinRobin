
using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {
	
	//Start active/enabled
	public bool startEnabled = true;

	[HideInInspector]
	//Tracks if the player is close or not
	public bool playerIsNear = false;

	//--------------------------------------------------
	//               ***Moving Code***
	//Create an enuration to specify initial move
	//direction
	public enum eDirection
	{
		Right,
		Left
	}
	//Declare a new enumeration variable
	public eDirection moveDirection;
	
	//Create a max speed vector
	public Vector2 maxSpeed = new Vector2(10,10);
	
	//Internal movement vector
	Vector2 movement;
	
	//Boolean to keep track of facing
	public bool facingRight = true;
	
	//Change directions when AI hits the specified object
	public string reverseObjectName = "AI_Reverse";
	
	//Internal facing direction
	Vector2 direction = new Vector2(1,0);
	
	//Variable to point at Animator
	Animator anim;
	
	//Seconds to wait before flipping is possible
	//	Prevents continual flipping every frame
	public float flipWaitSeconds = 1.0f;
	
	//The time at which the object can flip again
	float timeWhenCanFlip = 0.0f;
	//--------------------------------------------------

	//--------------------------------------------------
	//				***Fade on Death Code***
	//A variable to hold pointer to this objects sprite
	private SpriteRenderer mySprite;

	//How long in seconds it takes for this sprite to
	//fade out after death
	private float fadeOnDeathTime;

	//Times since death
	private float timeSinceDeath = 0.0f;

	//Pointer to the HealthController on this object
	private HealthController healthController;

	//Pointer to the WeaponController on this object
	private WeaponController weaponController;
	//--------------------------------------------------

	// Use this for initialization
	void Start () 
	{
		//--------------------------------------------------
		//				***Moving Code***
		//Flip the art if facing left on start
		if(moveDirection == eDirection.Left) Flip();

		//Set default values for movement
		movement.x = 0.0f;
		movement.y = 0.0f;
		//--------------------------------------------------
		
		//Get pointer to the animator
		anim = GetComponent<Animator>();

		//Get pointer to the sprite
		mySprite = gameObject.GetComponent<SpriteRenderer> ();

		//Get pointer to attached Health and Weapon Controllers
		healthController = gameObject.GetComponent<HealthController> ();
		weaponController = gameObject.GetComponent<WeaponController> ();

		//Save the timeToWaitOnDeath from HealthController
		fadeOnDeathTime = healthController.timeToWaitOnDeath;

		//If not startEnabled set the animator variable wait
		//to freeze it
		if (!startEnabled)
			anim.SetBool ("wait", true);

		//If the weapon controller exists and it its shootActive is
		//initially true, set playerIsNear to true to keep it from being
		//set to false automatically
		if(weaponController != null)
		{
			if(weaponController.shootActive) playerIsNear = true;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If not startEnabled freeze it this AI
		if (startEnabled)
		{
			//Set the movement variable
			movement = new Vector2
				(
					maxSpeed.x * direction.x,
					maxSpeed.y * direction.y
				);
			
			//Set the speed variable in our animator to our
			//x speed
			anim.SetFloat ("speed", Mathf.Abs (movement.x));
		}
	}
	
	//Called at every fixed framerate frame
	//Use when dealing with rigidBody Physics
	void  FixedUpdate()
	{
		//--------------------------------------------------
		//					***Moving Code***
		//Set the rigidbody2D velocity so it can move
		//the object, if it's not dead
		if(!anim.GetBool("dead"))
		{
			rigidbody2D.velocity = movement;

			if(playerIsNear)
			{
				//Debug.Log ("AnimState: " + anim.GetCurrentAnimatorStateInfo );
				anim.SetBool("playerClose", true);
				//Debug.Log ("AnimName: " + anim.name);
				//if( animation.IsPlaying(shootAnimName) )
				{
					anim.SetBool("animDone", true);
				}
				weaponController.shootActive = true;
			}
			else
			{
				anim.SetBool("playerClose", false);
				weaponController.shootActive = false;
			}
		}
		else //Fade the sprite out when dead
		{
			timeSinceDeath += Time.deltaTime;
			mySprite.color = new Color(mySprite.color.r,
			                           mySprite.color.g,
			                           mySprite.color.b,
			                           1.0f - timeSinceDeath/fadeOnDeathTime);
		}
		//--------------------------------------------------
	}
	
	//Flip the art facing and movement direction
	void Flip()
	{
		//Set timeWhenCanFlip
		timeWhenCanFlip = Time.time + flipWaitSeconds;
		
		//Reverse the facing of the AI
		facingRight = !facingRight;
		
		//Reverse the art facing
		Vector3 theScale = this.transform.localScale;
		theScale.x *= -1;
		this.transform.localScale = theScale;
		
		//REverse the movement direction
		direction.x *= -1;
	}
	
	//Called when another collider hits this one
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		//If AI collides with reverseObjectName then Flip():
		//		reverse art facing and move direction
 		if(otherCollider.gameObject.name == reverseObjectName)
			if(Time.time >= timeWhenCanFlip) Flip();
	}
	
	//Called when another collider hits this one
	void OnCollisionEnter2D(Collision2D otherCollider)
	{
		//If AI collides with player then Flip():
		//		reverse art facing and move direction
		if(otherCollider.gameObject.name == "Player")
			if(Time.time >= timeWhenCanFlip) Flip();
	}
}
