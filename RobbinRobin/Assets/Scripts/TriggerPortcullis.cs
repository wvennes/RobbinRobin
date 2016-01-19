﻿using UnityEngine;
using System.Collections;

public class TriggerPortcullis : MonoBehaviour {

    public AudioSource audioSource;

    //variable to track when player is able to toggle tutorial screen
    private bool canTrigger;


    //bool to test that button presses are being received
    public bool isButtonDown = false;

    public Portcullis Portcullis;



    public bool triggerViaButton = false;
    public bool triggerViaButtonActive = false;

    //Types of activations possible
    public enum TriggerType
    {
        Enable,
        Kill,
        Destroy,
        Sound,
        ProximityTrigger,
        ToggleVar,
        Portcullis
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

    public bool toggleVariable = true;

    //Variable that points to the attached animator
    Animator anim;

    //Time when can trigger again
    private float timeCanRetriggerEnter = 0.0f;
    private float timeCanRetriggerExit = 0.0f;

    // Use this for initialization
    void Start()
    {
        //Get pointer to this objects animator
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {


        /*
        if (triggerViaButton && triggerViaButtonActive && Input.GetButtonDown("Fire2") && (triggerType == TriggerType.Enable) && Portcullis.isFinished)
        {

            
            //InvisibleObject invisibleController = targetObject.GetComponent<InvisibleObject>();
            //if (invisibleController != null)
            //{
            //    //Reverse the visability of the invisible object
            //    invisibleController.startEnabled = !invisibleController.startEnabled;
            //    if (anim != null) anim.SetTrigger("trigger");
            //}




            Portcullis.isRaising = !Portcullis.isRaising;
            Portcullis.isFinished = false;


        }
        */


        if (!isButtonDown && triggerViaButton && triggerViaButtonActive && 
            ( Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire1") || Input.GetAxis("Vertical") > 0.5f) 
            && (triggerType == TriggerType.Enable) && 
            Portcullis.isFinished)
        {
            Portcullis.isRaising = !Portcullis.isRaising;
            Play(soundToPlay);
            Portcullis.isFinished = false;

            isButtonDown = Input.GetAxis("Vertical") > 0.5f;

            if (anim != null) anim.SetTrigger("trigger");

        }
        else if ( Input.GetAxis("Vertical") <= 0f )
        {
            isButtonDown = false;
        }




    }


    // Update is called once per frame
    void FixedUpdate()
    {


    }






    //Run when the player collides with this trigger
    void OnTriggerEnter2D(Collider2D collider)
    {
        //If player continue
        if ((collider.gameObject.tag == "Player") && (Time.time >= timeCanRetriggerEnter))
        {
            triggerViaButtonActive = true;

            if (triggerViaButton == false)
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
    }

    //Activate when object exits collider
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player" && Time.time >= timeCanRetriggerExit)
        {
            triggerViaButtonActive = false;

            if (triggerViaButton == false)
            {

                if (triggerType == TriggerType.ProximityTrigger) //If this is an ProximityTrigger, find the type of AI throw the appropriate flag
                {
                    AIController AIController = targetObject.GetComponent<AIController>();
                    if (AIController != null)
                    {
                        AIController.playerIsNear = false;
                    }
                }
                if (triggerType == TriggerType.Enable)
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
    }

    //Script that actually plays sounds
    public void Play(AudioClip sound)
    {
        //If sound exists
        if (sound != null)
        {
            audioSource.clip = sound;
            // As it is not 3D audio clip, position doesn't matter.
            audioSource.Play();
        }

    }


}
