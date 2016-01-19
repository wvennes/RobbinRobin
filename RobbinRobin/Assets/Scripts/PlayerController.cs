//Player Controller
//
//Controls player movment, actions, and anims
//
//by Michael McCoy
//with code from tutorials
//--------------------------------------------
using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //--------------------------------------------------
    //				***Moving Code***
    //Create a new public variable, Vector2
    //and give it default values
    public Vector2 maxSpeed = new Vector2(3, 1);

    //Create a new private/internal variable, Vector2
    //but don't give it default values
    Vector2 movement;

    //Variable to point to our animator
    Animator anim;

    //Tracks player facing
    public bool facingRight = true;

    // pause the game
    [HideInInspector]
    public bool isPaused = false;

    //The x speed of platform on which the player is
    //currently standing
    [HideInInspector]
    public float platformSpeedX = 0.0f;
    //--------------------------------------------------

    //--------------------------------------------------
    //				***Camera Code***
    //Should the Camera follow player or be fixed in space
    public bool CameraFollow = true;

    //Start/Stop following player when player hits
    //this object which is on the Default Layer
    public string cameraStartStopObjName;
    //--------------------------------------------------

    //--------------------------------------------------
    //				***Audio Source and Clips***
    private AudioSource m_audioSource;
    public AudioClip m_trapDoorSound;
    public AudioClip m_foodSound;
    //--------------------------------------------------
    
    //---------------------------------------------------------------------------
    //              ***Trapdoor Code***
    // ground checks
    public bool grounded = false;
    public Transform groundCheck;
    LayerMask whatIsGround;
    float groundRadius = 0.2f;
    float inputX = 0f;
    float inputY = 0f;

    //Timer to keep the grounded anim flag from being
    //immediately reset
    private float groundedTimer = 0.0f;

    //How long to wait in seconds before the grounded
    //flag can be set again
    float groundedTimerDelay = 0.2f;

    // is Robin above or below a trapdoor
    public bool isBelowTrapdoor = false;
    public bool isAboveTrapdoor = false;
    public bool isMovingUp = false;
    public bool isMovingDown = false;
    public bool isKeyDown = false;
    public bool isButtonUpPressed = false;
    public bool isButtonDownPressed = false;
    public bool isLockedToX = false;
    public bool isMovingToX = false;
    public bool hasPassed = false;

    // names of trapdoor triggers to check
    string trapdoorTriggerBelowName = "TriggerBelow";
    string trapdoorTriggerAboveName = "TriggerAbove";

    // x component - locks to this
    float trapdoorPositionX = 0;
    float speedOfTrapdoorLock = 0.1f;

   // private float jumpForce = 650;
    public float jumpHeight = 12;
    //---------------------------------------------------------------------------

    [HideInInspector]
    //Location of respawn when dead
    public Vector3 respawnPoint;

    //Name of check point objects
    public string checkPointObjName = "Check_Point";

    //public Vector2 debugVelocity;

    //Use this for initialization
    void Start()
    {
        //Finds our animator and stores its location
        //in our variable
        anim = GetComponent<Animator>();

        //Set initial respawn point
        respawnPoint = this.transform.position;

        //Add a little to the Y position to force falling and
        //reset the animator
        respawnPoint.y += 1.0f;

        // Finds our AudioSource and stores its location
        // in our variable
        m_audioSource = GetComponent<AudioSource>();

        whatIsGround = 1 << 10 | 1 << 11;
        movement = new Vector2(rigidbody2D.velocity.x, rigidbody2D.velocity.y);
    }

    //Update is called once per frame
    void Update()
    {
        //debugVelocity = rigidbody2D.velocity;
        ////float inputY = Input.GetAxis("Vertical");
        //if ( Input.GetButtonDown("Pause") && !isPaused )
        //{
        //    Time.timeScale = 0f;
        //    isPaused = true;
        //}
        //else if (Input.GetButtonDown("Pause") && isPaused )
        //{
        //    Time.timeScale = 1f;
        //    isPaused = false;
        //}

        //--------------------------------------------------
        //Moving
        //If not playing an anim that demands waiting
        if (!anim.GetBool("wait"))
        {
            //Retrieves input on two virtual axis (-1 to 1)
            //default mapping is the arrow keys
            inputX = Input.GetAxis("Horizontal");

            //float inputY = Input.GetAxis ("Vertical");

            //Set the animator "run"/move flag
            if (inputX == 0) anim.SetBool("run", false);
            else anim.SetBool("run", true);

            //Set the movement variable
            //	Must add platform speed X and the player does
            //	not automatically stay locked on the x axis
            //	while riding a platform
//             movement = new Vector2
//             (
//                 (maxSpeed.x * inputX) + platformSpeedX,
//                 rigidbody2D.velocity.y
//                 //maxSpeed.y * inputY
//             );
        }
        else
        {
            //Set movement to 0
            movement.x = 0.0f;
        }

        if ( !grounded || isMovingToX || isLockedToX || isMovingUp || isMovingDown )
        {
            inputX = 0f;
            inputY = 0f;
        }

        //Set the speed variable in our animator to our
        //x speed
        anim.SetFloat("speed", Mathf.Abs(movement.x));
        //--------------------------------------------------
        // ** Trapdoor Code **
        //--------------------------------------------------
        if (!anim.GetBool("wait") && grounded)
        {
            isButtonUpPressed = Input.GetButtonDown("Move Up") || ( Input.GetAxis("Vertical") > 0.5f );
            isButtonDownPressed = Input.GetButtonDown("Move Down") || ( Input.GetAxis("Vertical") < -0.5f );

//             if(isButtonDownPressed)
//             {
//                 int x = 5;
//             }
//             else if(isButtonUpPressed)
//             {
//                 int x = 5;
//             }
            if ( isButtonDownPressed && isButtonUpPressed )
            {
                isButtonDownPressed = false;
            }

            if ( isKeyDown )
            {
                //isKeyDown = (inputY != 0);
                isKeyDown = (isButtonUpPressed || isButtonDownPressed);
            }
            //isKeyDown = ( inputY == 0 );

            if (!isKeyDown && !isMovingToX && !isLockedToX && !isMovingDown && !isMovingUp && ((isButtonDownPressed && isAboveTrapdoor) || (isButtonUpPressed && isBelowTrapdoor)))
            {





                isMovingToX = true;
                if (isButtonUpPressed && isBelowTrapdoor && grounded)
                {
                    isMovingUp = true;
                    isButtonDownPressed = false;
                    isButtonUpPressed = false;
                }
                else if (isButtonDownPressed && isAboveTrapdoor)
                {
                    isMovingDown = true;
                    isButtonDownPressed = false;
                    isButtonUpPressed = false;
                }
                isKeyDown = true;
            }

            if (isMovingToX)
            {
                LockToX();
            }

            if (isLockedToX)
            {
                // Play Trapdoor Sound
                PlaySound(m_trapDoorSound);
                hasPassed = false;

                if (isMovingUp)
                {
                    //rigidbody2D.AddForce(new Vector2(0, jumpForce));
                    rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, /*rigidbody2D.velocity.y +*/ jumpHeight);
                }
//                 if (isAboveTrapdoor && isBelowTrapdoor)
//                 {
//                     if (isButtonDownPressed)
//                     {
//                         isMovingDown = true;
// 
//                     }
//                     else if (isButtonUpPressed)
//                     {
//                         isMovingUp = true;
//                         rigidbody2D.AddForce(new Vector2(0, jumpForce));
//                     }
//                 }
//                 else if (isAboveTrapdoor)
//                 {
//                     isMovingDown = true;
// 
//                 }
// 
//                 else if (isBelowTrapdoor)
//                 {
//                     isMovingUp = true;
//                     rigidbody2D.AddForce(new Vector2(0, jumpForce));
// 
// 
//                 }
                if (isMovingUp || isMovingDown)
                {
                    foreach (Collider2D c in this.GetComponents<Collider2D>())
                    {
                        if (!c.isTrigger)
                        {
                            c.enabled = false;
                        }
                    }


                    isLockedToX = false;
                    //Set not on ground in Animator
                    anim.SetBool("grounded", false);
                    grounded = false;
                }
            }
        }


        movement = new Vector2
        (
            (maxSpeed.x * inputX),
            rigidbody2D.velocity.y
        );


    }

    //Called at every fixed framerate frame
    //Use when dealing with rigidBody Physics
    void FixedUpdate()
    {

        //When you start to move in a direction opposite to
        //your facing, flip the animation and facing to match
        if (movement.x > 0.0f && !facingRight)
            Flip();
        else if (movement.x < 0.0f && facingRight)
            Flip();
        //--------------------------------------------------

        //---------------------------------------------------------------------------
        // Check for Grounded
        //---------------------------------------------------------------------------
	    if (!isMovingDown && !isMovingUp)
	    {
		    grounded = Physics2D.OverlapCircle
		            (groundCheck.position,
		                groundRadius,
		                whatIsGround);
		
		    //Check to see if the grounded flag needs to be and
		    //if it can be changed
		    if (anim.GetBool("grounded") != grounded && Time.time >= groundedTimer)
		    {
		        //Sets a bool in our animator that tracks whether 
		        //we're on the ground or not
		        anim.SetBool("grounded", grounded);
		
		        //Reset the groundedTimer
		        groundedTimer = Time.time + groundedTimerDelay;
		    }
	    }

        //--------------------------------------------------
        //				***Moving Code***
        //Move the Player if not dead
        if (!anim.GetBool("dead"))
        {
            rigidbody2D.velocity = movement;
        }
        else
        {
            //Set movement to 0
            movement.x = 0.0f;
            rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
        }

        //Sets a float variable that track vertical speed
        anim.SetFloat("vSpeed", rigidbody2D.velocity.y);
        //--------------------------------------------------
    }

    //---------------------------------------------------------------------------
    void LockToX()
    {
        if (this.collider2D.transform.position.x == trapdoorPositionX)
        {
            anim.SetBool("isLocking", false);


            //play the jump wind up animation
            anim.SetTrigger("movingToX");

            isMovingToX = false;
            isLockedToX = true;
            return;
        }

        anim.SetBool("isLocking", true);
        this.transform.position = Vector3.MoveTowards
        (
            this.collider2D.transform.position,
            new Vector3(trapdoorPositionX, this.collider2D.transform.position.y, this.collider2D.transform.position.z),
            speedOfTrapdoorLock
        );
    }

    //---------------------------------------------------------------------------
    //Reverses art facing
    void Flip()
    {
        if ( Time.timeScale != 0f )
        {
            //Reverse the facing of the player
            facingRight = !facingRight;

            //Handles the reversing of the facing
            Vector3 theScale = this.transform.localScale;
            theScale.x *= -1;
            this.transform.localScale = theScale;
        }
    }

    //---------------------------------------------------------------------------
    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.name == trapdoorTriggerBelowName)
        {
            trapdoorPositionX = otherCollider.gameObject.transform.position.x;
            isBelowTrapdoor = true;
        }

        if (otherCollider.gameObject.name == trapdoorTriggerAboveName)
        {
            trapdoorPositionX = otherCollider.gameObject.transform.position.x;
            isAboveTrapdoor = true;
        }

        if (otherCollider.gameObject.name == "StopMovingUp" && isMovingUp && hasPassed)
        {
            //Debug.Log("isMovingUp: " + isMovingUp.ToString());
            isMovingUp = false;
            hasPassed = false;
            foreach (Collider2D c in this.GetComponents<Collider2D>())
            {
                c.enabled = true;
            }
        }

        if (otherCollider.gameObject.name == "StopMovingDown" && isMovingDown && hasPassed)
        {
            //Debug.Log("isMovingDown: " + isMovingDown.ToString());
            isMovingDown = false;
            hasPassed = false;
            foreach (Collider2D c in this.GetComponents<Collider2D>())
            {
                c.enabled = true;
            }
        }
    }

    //---------------------------------------------------------------------------
    void OnTriggerStay2D(Collider2D otherCollider)
    {
        if (otherCollider.gameObject.name == trapdoorTriggerBelowName)
        {
            trapdoorPositionX = otherCollider.gameObject.transform.position.x;
            isBelowTrapdoor = true;
        }

        if (otherCollider.gameObject.name == trapdoorTriggerAboveName)
        {
            trapdoorPositionX = otherCollider.gameObject.transform.position.x;
            isAboveTrapdoor = true;
        }

        if (otherCollider.gameObject.name == "StopMovingUp" && isMovingUp && hasPassed)
        {
            //Debug.Log("isMovingUp: " + isMovingUp.ToString());
            isMovingUp = false;
            hasPassed = false;
            foreach (Collider2D c in this.GetComponents<Collider2D>())
            {
                c.enabled = true;
            }
        }

        if (otherCollider.gameObject.name == "StopMovingDown" && isMovingDown && hasPassed)
        {
            //Debug.Log("isMovingDown: " + isMovingDown.ToString());
            isMovingDown = false;
            hasPassed = false;
            foreach (Collider2D c in this.GetComponents<Collider2D>())
            {
                c.enabled = true;
            }
        }
    }

    //---------------------------------------------------------------------------
    //Called on collision with triggers
    void OnTriggerExit2D(Collider2D otherCollider)
    {

        if (otherCollider.gameObject.name == "hasPassed" && ( isMovingDown || isMovingUp ) )
        {
            hasPassed = true;
        }

        //If collide with start/stop camera object
        if (otherCollider.gameObject.name == cameraStartStopObjName)
        {
            //Reverse following variable
            CameraFollow = !CameraFollow;

        }

        //If collide with check point object
        if (otherCollider.gameObject.name == checkPointObjName)
        {
            //Set respawn point to be this objects location
            respawnPoint = otherCollider.transform.position;

            //Add a little to the Y position to force falling and
            //reset the animator
            respawnPoint.y += 1.0f;
        }

        if (otherCollider.gameObject.name == trapdoorTriggerBelowName)
        {
            isBelowTrapdoor = false;
        }

        if (otherCollider.gameObject.name == trapdoorTriggerAboveName)
        {
            isAboveTrapdoor = false;
        }
    }

    //---------------------------------------------------------------------------
//     void OnCollisionEnter2D(Collision2D otherCollision)
//     {
//         if (otherCollision.gameObject.layer == whatIsGround)
//         {
//             isMovingDown = false;
//             isKeyDown = false;
//         }
//     }

    //---------------------------------------------------------------------------
    //Script that actually plays sounds
    public void Play(AudioClip sound)
    {
        //If sound exists
        if (sound != null)
            // As it is not 3D audio clip, position doesn't matter.
            AudioSource.PlayClipAtPoint(sound, transform.position);

    }

    private void PlaySound(AudioClip soundToPlay)
    {
        m_audioSource.clip = soundToPlay;

        m_audioSource.Play();
    }

    private void PlaySound(string soundToPlay)
    {
        if(soundToPlay == "Food")
        {
            PlaySound(m_foodSound);
        }
    }

}