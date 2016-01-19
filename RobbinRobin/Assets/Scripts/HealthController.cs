//Health System
//
//Handles taking damage, playing hurt-dead anims
//and hit points
//
//by Michael McCoy
//with code from tutorials
//modifications by James Bowling for invulnerability
//--------------------------------------------
using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour 
{
	//Number of lives
	public float lives = 3.0f;

	//Max starting health
	public float maxHealth = 100.0f;

	//[HideInInspector]
	//Current health
	public float health;

	//Check for if the player is invulnerable - this ties to the powerup script
	//If checked, player can never be hurt or killed
	public bool isInvulnerable = false;

	//Link to animator
	public Animator anim;
	
	//Pause input on hurt
	public bool pauseInputOnHurt = false;
	
	//Time to wait after starting death anim before
	//destroying character
	public float timeToWaitOnDeath = 4.0f;
	
	//Is this the player
	//bool isPlayer = false;

	public bool isDead = false;

    public AudioSource m_respawnSource;
	//Respawn variable
	[HideInInspector]
	public bool respawn = false;

	//-------------------------------------------------------
	//				***Fading Code***
	//Time over which the player fades back in
	private float fadeInTime = 0.0f;

	//Time over which the player fades out
	private float fadeOutTime = 0.0f;

	//Time since death
	private float elapsedTime = 0.0f;
	
	//How long in seconds it takes for this sprite to
	//fade out after death
	private float fadeOnDeathTime;
	
	//A variable to hold pointer to this objects sprite
	private SpriteRenderer mySprite;
	//-------------------------------------------------------
	
	//Define hurt sound
	public AudioClip hurtSound;
	
	//Define death sound
	public AudioClip deathSound;
	
	//Define respawn sound
	public AudioClip respawnSound;

    //Level to goto when player is out of lives
    public string OnFinalDeathLoadLevel = "none";
    private float m_respawnSoundWait = 1f;

	// Use this for initialization
	void Start () 
	{
		//Finds our animator and stores its location
		//in our variable
		anim = GetComponent<Animator> ();
		
		//Set intial health
		health = maxHealth;
		
		//Animator health value
		anim.SetFloat ("health", health);
		
		//Set the isPlayer variable
		//if(gameObject.name == "Player")
			//isPlayer = true;

		//Get pointer to the sprite
		mySprite = gameObject.GetComponent<SpriteRenderer> ();

		//If there is saved health and lives data, use it
		//This data gets saved by EndOfLevel.cs
		if( gameObject.CompareTag("Player") )
		{
			if (PlayerPrefs.GetFloat ("Health") != 0.0f)
				health = PlayerPrefs.GetFloat ("Health");
			if (PlayerPrefs.GetFloat ("Lives") != 0.0f)
				lives = PlayerPrefs.GetFloat ("Lives");
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Reset the animator wait variable when time runs out
		if (Time.time >= anim.GetFloat ("waitTime"))
		{
			anim.SetBool ("wait", false);
			
			//If the player needs to respawn then start that sequence
			if (respawn)
			{
				isDead = false;

				respawnPlayer();
			}
		}





        /*
		//Fade the sprite out when dead
		if(anim.GetBool("dead"))
		{
			elapsedTime += Time.deltaTime;
			mySprite.color = new Color(mySprite.color.r,
			                           mySprite.color.g,
			                           mySprite.color.b,
			                           1.0f - elapsedTime/fadeOutTime);
		}
        */
 




		//Fade the object in after respawn
		if(fadeInTime > 0 && elapsedTime < fadeInTime)
		{
			elapsedTime += Time.deltaTime;
			mySprite.color = new Color(mySprite.color.r,
			                           mySprite.color.g,
			                           mySprite.color.b,
			                           elapsedTime/fadeInTime);
		}
		else
		{
			//Make sure character is completely faded in
			if(fadeInTime > 0)
			{
				mySprite.color = new Color(mySprite.color.r,
				                           mySprite.color.g,
				                           mySprite.color.b,
				                           1.0f);
			}

			//Reset the fadeInTime
			fadeInTime = 0.0f;
		}



		//Update animator variable
		anim.SetFloat ("health", health);
	}

	void FixedUpdate()
	{
		//Kill target if health <= zero
		//If the player is invulnerable, don't kill them
		if (!isInvulnerable)
		{
			if(isDead == true && respawn != true)
			{
				//Debug.Log ("I'm dead!");
				isDead = false;
				Dead ();	
			}
		}
	}

	//Respawn the player at player start or last checkpoint
	void respawnPlayer()
	{

		//Debug.Log ("isDead is " + isDead +" at the start of respawnPlayer");

		//Move the player to the appropriate respawn point
        if ( gameObject.tag == "Player" )
        {
            gameObject.transform.position =
                        gameObject.GetComponent<PlayerController>().respawnPoint;
        }
        else if ( gameObject.tag == "Enemy" )
        {
            //gameObject.transform.position = gameObject.GetComponent<AIControllerPatrolPig>().m_pigHomePoint;
            gameObject.GetComponent<AIControllerPatrolPig>().Respawn();
            //gameObject.GetComponent<AIControllerPatrolPig>().m_currentPigAlertState = AIControllerPatrolPig.AlertState.BackToStart;
            //gameObject.GetComponent<AIControllerPatrolPig>().m_currentPigAlertStat
        }

		//Reset health
		health = maxHealth;

		//Reset the respawn flag
		respawn = false;

		//Turn off the dead flag
		anim.SetBool ("dead", false);

		//Reset waitTime variable
		anim.SetFloat ("waitTime", 0.0f);

		//Reset elapsed time
		elapsedTime = 0.0f;

		//Start fading back in
		//fadeInTime = timeToWaitOnDeath / 2.0f;
        fadeInTime = 0.5f;

		//Reset the animator to allow the respawned player to function properly
		anim.Play ("Any State");

		//Play sound
        if (gameObject.tag == "Player")
        {
            Play(respawnSound);
        } 
	}

	//Script that actually plays sounds
	public void Play(AudioClip sound)
	{
		//If sound exists
		if(sound != null)
			// As it is not 3D audio clip, position doesn't matter.
			AudioSource.PlayClipAtPoint(sound, transform.position);
	}

	void Dead()
	{
		isDead = false;

        //Set Player Velocity to 0 on death
        if(gameObject.tag == "Player")
        {
            rigidbody2D.velocity = new Vector2(0f, 0f);
            //rigidbody2D.gravityScale = 0f;
            foreach (Collider2D c in this.GetComponents<Collider2D>())
            {
                if (!c.enabled)
                {
                    c.enabled = true;
                }
            }
            
            //Set the dead boolean to tell all code that this object is dead
            anim.SetBool("dead", true);

            //Play death anim
            anim.SetTrigger("death");
            
        }

		//Play sound
		Play(deathSound);
	    if(gameObject.tag == "Enemy")
        {
            StartCoroutine("PlayeEnemyRespawnSound");
        }
		//Set the dead boolean to tell all code that this object is dead
		anim.SetBool("dead", true);
		
		//Set animator wait variable
		anim.SetBool ("wait", true);
		
		//Set time to wait until accepting key imputs
		anim.SetFloat ("waitTime", Time.time + timeToWaitOnDeath );
		
		//Play death anim
		anim.SetTrigger("death");
		
		//Reset elapsed time
		elapsedTime = 0.0f;
		
		//Set fadeOutTime
		fadeOutTime = timeToWaitOnDeath;
		
		//Take away one life
        if ( gameObject.tag == "Player" )
		{
            lives--;
        }
        else
        {
            lives++;
        }
		//Debug.Log ("I have taken one of your lives away, now you have " + lives + " lives");

		//If out of lives = game over
		if(lives < 1.0)
		{
            //Goto the gameOverLevel if this is on the player
            if (gameObject.tag == "Player")
                Invoke("LoadFinalDeathLevel", timeToWaitOnDeath);

			//Destroy this object
			Destroy (gameObject, timeToWaitOnDeath);
		}
		else //Respawn at start or last checkpoint
		{
			//Start respawn sequence
			isDead = false;
			respawn = true;
		}
	}

    //Load next level
    private void LoadFinalDeathLevel()
    {

        //Load gameOverLevel
        Application.LoadLevel(OnFinalDeathLoadLevel);
    }

    IEnumerator PlayeEnemyRespawnSound()
    {
        yield return new WaitForSeconds(m_respawnSoundWait);

        m_respawnSource.clip = respawnSound;
        m_respawnSource.Play();
    }
}
