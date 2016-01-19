//Trigger_Script
//
//Triggers other objects startEnabled variable as well
//as playing sounds and initiating immediate destruction
//
//by James Bowling; modified by Michael McCoy
//
//10/25/2014: Added new condition to enable Patrol Pigs. Needed because of
//				changes to the AIControllerPatrolPig script -Nathan Bowden

using UnityEngine;
using System.Collections;

public class Trigger_Script : MonoBehaviour {

	//Types of activations possible
	public enum TriggerType
	{
		Enable,
		Kill,
		Destroy,
		Sound,
		ProximityTrigger
	}

	//Type of activation
	public TriggerType triggerType;

	//Target object to be activated
	public GameObject targetObject;

	//Trigger only once and disable?
	public bool triggerOnce = true;

	//How long to wait before destroy the target object
	public float destroyTimer = 0.0f;

	//Define sound
	public AudioClip soundToPlay;

	//Allows triggered objects to turn off on trigger exit
	public bool disableObjectOnExit = false;

	//Variable that points to the attached animator
	Animator anim;

	//Time when can trigger again
	private float timeCanRetriggerEnter = 0.0f;
	private float timeCanRetriggerExit = 0.0f;
	
	// Use this for initialization
	void Start ()
	{
		//Get pointer to this objects animator
		anim = gameObject.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	//Run when the player collides with this trigger
	void OnTriggerEnter2D(Collider2D collider)
	{
        if (collider.gameObject.tag == "Player" &&
            Time.time >= timeCanRetriggerEnter &&
            this.gameObject.tag == "Enemy" && 
            this.gameObject.GetComponent<AIControllerPatrolPig>().m_canSeePlayer )
        {
            //Get HealthController and set isDead flag
            HealthController healthController = targetObject.GetComponent<HealthController>();
            //If the HealthController exists
            if (healthController != null)
            {
                //Set isDead flage
                healthController.isDead = true;
            }
            else
                //Debug.Log("Health Controller is missing on target");

            //If trigger once, disable collision - moved from inside Enable section to allow a one-time trigger for all types of triggers
            if (triggerOnce)
                this.collider2D.enabled = false;
        }

		//If player continue
		else if ((collider.gameObject.tag == "Player") && this.gameObject.tag != "Enemy" && Time.time >= timeCanRetriggerEnter)
		{
			//If type Kill
            if (triggerType == TriggerType.Kill)
            {
                //Get HealthController and set isDead flag
                HealthController healthController = targetObject.GetComponent<HealthController>();
                //If the HealthController exists
                if (healthController != null)
                {
                    //Set isDead flage
                    healthController.isDead = true;
                }
                else
                    //Debug.Log("Health Controller is missing on target");

                //If trigger once, disable collision - moved from inside Enable section to allow a one-time trigger for all types of triggers
                if (triggerOnce)
                    this.collider2D.enabled = false;
            }
            //If type Destroy
            else if (triggerType == TriggerType.Destroy)
            {
                //If the timer is not set to infinite (0)
                if (destroyTimer < 0)
                    destroyTimer = 0;

                //Set the this object to be destroyed
                Destroy(targetObject, destroyTimer);

                //If trigger once, disable collision - moved from inside Enable section to allow a one-time trigger for all types of triggers
                if (triggerOnce)
                    this.collider2D.enabled = false;
            }
            //If this is an Enable trigger, find the type of controller and enable it!
            else if (triggerType == TriggerType.Enable)
            {
                AIController AIController = targetObject.GetComponent<AIController>();
                if (AIController != null)
                {
                    AIController.startEnabled = true;
                    if (anim != null) anim.SetTrigger("trigger");
                }
				AIControllerPatrolPig AIControllerPatrolPig = targetObject.GetComponent<AIControllerPatrolPig>();
				if (AIControllerPatrolPig != null)
				{
					AIControllerPatrolPig.m_isStartEnabled = true;
					if (anim != null) anim.SetTrigger("trigger");
				}
                MovingPlatformLinear movingController = targetObject.GetComponent<MovingPlatformLinear>();
                if (movingController != null)
                {
                    movingController.startEnabled = true;
                    if (anim != null) anim.SetTrigger("trigger");
                }
                OneWayPlatforms oneWayController = targetObject.GetComponent<OneWayPlatforms>();
                if (oneWayController != null)
                {
                    oneWayController.startEnabled = true;
                    if (anim != null) anim.SetTrigger("trigger");
                }
                HazardScript hazardController = targetObject.GetComponent<HazardScript>();
                if (hazardController != null)
                {
                    hazardController.startEnabled = true;
                    if (anim != null) anim.SetTrigger("trigger");
                }
                Destructible_Object destructibleController = targetObject.GetComponent<Destructible_Object>();
                if (destructibleController != null)
                {
                    destructibleController.startEnabled = true;
                    if (anim != null) anim.SetTrigger("trigger");
                }
                InvisibleObject invisibleController = targetObject.GetComponent<InvisibleObject>();
                if (invisibleController != null)
                {
					//Reverse the visability of the invisible object
					invisibleController.startEnabled = !invisibleController.startEnabled;
                    if (anim != null) anim.SetTrigger("trigger");
                }

                //If trigger once, disable collision - moved from inside Enable section to allow a one-time trigger for all types of triggers
                if (triggerOnce)
                    this.collider2D.enabled = false;

            }
            else if (triggerType == TriggerType.Sound)  //If this is a sound trigger, then the sound will play - moved from inside Enable section to fix bug
            {
                Play(soundToPlay);

                //If trigger once, disable collision - moved from inside Enable section to allow a one-time trigger for all types of triggers
                if (triggerOnce)
                    this.collider2D.enabled = false;
            }
            else if (triggerType == TriggerType.ProximityTrigger) //If this is an ProximityTrigger, find the type of AI throw the appropriate flag
            {
                AIController AIController = targetObject.GetComponent<AIController>();
                if (AIController != null)
                {
                    AIController.playerIsNear = true;
                }
            }
			//Reset retrigger time
			timeCanRetriggerEnter = Time.time + 1.0f;
		}
	}

	//Activate when object exits collider
	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player" && Time.time >= timeCanRetriggerExit)
		{
			if (triggerType == TriggerType.ProximityTrigger) //If this is an ProximityTrigger, find the type of AI throw the appropriate flag
			{
				AIController AIController = targetObject.GetComponent<AIController>();
				if( AIController != null)
				{
					AIController.playerIsNear = false;
				}
			}
			if(triggerType == TriggerType.Enable)
			{
				//Reverse the visability of the invisible object 
				InvisibleObject invisibleController = targetObject.GetComponent<InvisibleObject>();
				if (invisibleController != null && disableObjectOnExit)
				{
					invisibleController.startEnabled = !invisibleController.startEnabled;
					if (anim != null) anim.SetTrigger("trigger");
				}
			}
            //If trigger once, disable collision - moved from inside Enable section to allow a one-time trigger for all types of triggers
            if (triggerOnce)
                this.collider2D.enabled = false;

			//Reset retrigger time
			timeCanRetriggerExit = Time.time + 1.0f;
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
}
