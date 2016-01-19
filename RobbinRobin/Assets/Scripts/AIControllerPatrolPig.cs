/* 
 *  AI Controller for patrol pigs
 * 
 * 10/25/2014: Removed references to weapon controller since patrol pigs cannot shoot projectiles -Nathan Bowden
 * 
 * 
 */



using UnityEngine;
using System.Collections;

public class AIControllerPatrolPig : MonoBehaviour 
{

	//Start active/enabled
    [HideInInspector]
	public bool m_isStartEnabled = true;

	[HideInInspector]
	//Tracks if the player is close or not
	public bool m_isPlayerNear = false;

    //---------------------------------------------------------------------------
    // ** Initial direction for pig to patrol when in Patrol mode.
	public enum PigInitialDirection
	{
		Right,
		Left
	}
	private PigInitialDirection m_currentMoveDirection = PigInitialDirection.Left;
    public PigInitialDirection m_initialMoveDirection = PigInitialDirection.Left;

    //---------------------------------------------------------------------------
    // ** Make initial behavior of pig able to be set
    public enum PatrolBehaviors
    {
        Stationary,
        Flip,
        Patrol
    }
    public PatrolBehaviors m_pigBehavior = PatrolBehaviors.Stationary;

    //---------------------------------------------------------------------------
    // ** Stealth flags
    public enum AlertState
    {
        NotAlert,
        ChasePlayer,
        LostPlayer,
        BackToStart,
        CaughtPlayer
    }
    public AlertState m_currentPigAlertState = AlertState.NotAlert;
    public float m_secondsTilPlayerLost = 5.0f;
    public float m_secondsTilBackToStart = 2.0f;

    float m_currentAlertStateTimer = 0.0f;
	
	//Create a max speed vector
	public Vector2 m_pigInitialMovementSpeed = new Vector2( 2f, 0f );
    public Vector2 m_pigAlertMovementSpeed = new Vector2(4f, 0f);

    // Home point to return to
    [HideInInspector]
    public Vector3 m_pigHomePoint;
	
	//Internal movement vector
	Vector2 m_movement;
    bool m_isFacingHomePoint = false;
	
	//Boolean to keep track of facing
	public bool m_isFacingRight = true;
	
	//Change directions when AI hits the specified object
	public string m_reverseObjectName = "AI_Reverse";
	
	//Internal facing direction
	Vector2 m_currentDirection = new Vector2(1,0);
	
	//Variable to point at Animator
    private SpriteRenderer m_sprite;
    Animator m_anim;
	
	//Seconds to wait before flipping is possible
	//	Prevents continual flipping every frame
	public float m_flipWaitSeconds = 1.0f;
	float m_currentFlipTimerSeconds = 0.0f;

    //---------------------------------------------------------------------------
    // ** Raycast variables
    int m_layerMask;
    //RaycastHit2D m_hit;
    public bool m_canSeePlayer = false;
	//--------------------------------------------------

    public bool m_hasPlayedAlertSound = false;

    private AudioSource m_audioSource;

    public AudioClip m_alertSound;
    public Object publicHitObject;
	// Use this for initialization
	void Start () 
	{
        Physics2D.IgnoreLayerCollision(19, 19, true);
        Physics2D.IgnoreLayerCollision(19, 20, true);
		//--------------------------------------------------
		//				***Moving Code***
		//Flip the art if facing left on start
        if (!m_isFacingRight || m_initialMoveDirection == PigInitialDirection.Left) Flip();

		//Set default values for movement
		m_movement.x = 0.0f;
		m_movement.y = 0.0f;
		//--------------------------------------------------
		
		//Get pointer to the animator
		m_anim = GetComponent<Animator>();

        //--------------------------------------------------
        m_audioSource = GetComponent<AudioSource>();
        //--------------------------------------------------

		//Get pointer to the sprite
		m_sprite = gameObject.GetComponent<SpriteRenderer> ();

		//If not startEnabled set the animator variable wait
		//to freeze it
		if (!m_isStartEnabled)
			m_anim.SetBool ("wait", true);

		//set playerIsNear to true to keep it from being set to false automatically
		m_isPlayerNear = true;

        if ( m_pigBehavior == PatrolBehaviors.Patrol )
        {
            m_isStartEnabled = true;
        }

        m_pigHomePoint = transform.position;
        m_currentMoveDirection = m_initialMoveDirection;

        m_layerMask = 1 << 8 | 1 << 11;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//If not startEnabled freeze it this AI
		if ( m_pigBehavior == PatrolBehaviors.Patrol && m_currentPigAlertState == AlertState.NotAlert )
		{
			//Set the movement variable
            m_movement = new Vector2
            (
                m_pigInitialMovementSpeed.x * m_currentDirection.x,
                rigidbody2D.velocity.y
            );

        }
        else if ( m_currentPigAlertState == AlertState.ChasePlayer )
        {
            if (!m_hasPlayedAlertSound)
            {
                PlaySound(m_alertSound);
                m_hasPlayedAlertSound = true;
            }

            m_movement = new Vector2
            (
                m_pigAlertMovementSpeed.x * m_currentDirection.x,
                rigidbody2D.velocity.y
            );
            this.gameObject.layer = 20;
            m_anim.SetBool("isAlert", true);
            m_isFacingHomePoint = false;
        }
        else if ( m_currentPigAlertState == AlertState.LostPlayer )
        {
            this.gameObject.layer = 19;
            m_anim.SetBool("isAlert", false);
            m_movement = new Vector2(0f, rigidbody2D.velocity.y);
        }
        else if ( m_currentPigAlertState == AlertState.CaughtPlayer )
        {
            m_anim.SetBool("caughtPlayer", true);
            m_movement = new Vector2(0f, rigidbody2D.velocity.y);
        }
        else if ( m_currentPigAlertState == AlertState.BackToStart )
        {
            if (!m_isFacingHomePoint)
            {
                float direction = m_pigHomePoint.x - transform.position.x;
                if ( ( direction < 0f && m_currentDirection.x > 0f ) ||
                    ( direction > 0f && m_currentDirection.x < 0f ) )
                {
                    Flip();
                    FlipCurrentMovement();
                }
                m_isFacingHomePoint = true;
            }

//             float step = ( 0.5f * m_pigInitialMovementSpeed.x) * 2f  * Time.deltaTime;
//             transform.position = Vector3.MoveTowards(transform.position, m_pigHomePoint, step);
            
            if ( m_currentDirection.x * ( m_pigHomePoint.x - transform.position.x ) <= 0f )
            {
                m_currentPigAlertState = AlertState.NotAlert;
                m_movement = new Vector2(0f, rigidbody2D.velocity.y);

                m_currentMoveDirection = m_initialMoveDirection;
                if ((!m_isFacingRight && m_currentMoveDirection == PigInitialDirection.Right) ||
                    (m_isFacingRight && m_currentMoveDirection == PigInitialDirection.Left))
                {
                    //FlipCurrentMovement();
                    Flip();
                }
                m_isFacingHomePoint = false;
            }
            else
            {
                m_movement = new Vector2
                (
                    m_pigInitialMovementSpeed.x * m_currentDirection.x,
                    0f
                );
            }
        }

			
		//Set the speed variable in our animator to our
		//x speed
		m_anim.SetFloat ("speed", Mathf.Abs (m_movement.x));

        // Raycast for pig LOS
        RaycastHit2D hit = Physics2D.Raycast(transform.position, m_currentDirection, Mathf.Infinity, m_layerMask);

        if ( hit.collider.gameObject.tag == "Player" && m_currentPigAlertState != AlertState.CaughtPlayer )
        {
            if ( hit.collider.gameObject.GetComponent<Player_Stealthed>().isLit )
            {
                //Debug.DrawRay(transform.position, m_currentDirection * hit.distance, Color.white);
                m_currentPigAlertState = AlertState.ChasePlayer;
                m_currentAlertStateTimer = 0.0f;
            }
        }

        if ( hit.collider.gameObject.tag == "Player" && m_currentPigAlertState == AlertState.ChasePlayer)
        {
            m_canSeePlayer = true;
        }
        else
        {
            m_canSeePlayer = false;
        }
	}
	
	//Called at every fixed framerate frame
	//Use when dealing with rigidBody Physics
	void  FixedUpdate()
	{

        if (m_currentPigAlertState == AlertState.NotAlert)
        {
            switch (m_pigBehavior)
            {
                case PatrolBehaviors.Stationary:
                {
                    // do nothing
                    break;
                }

                case PatrolBehaviors.Flip:
                {
                    FlipBehavior();
                    break;
                }

                case PatrolBehaviors.Patrol:
                {
                    // do nothing
                    break;
                }
            }
        }

        else
        {
            switch ( m_currentPigAlertState )
            {
                case AlertState.ChasePlayer:
                {
                    m_currentAlertStateTimer += Time.deltaTime;
                    if ( m_currentAlertStateTimer >=  m_secondsTilPlayerLost )
                    {
                        m_currentAlertStateTimer = 0.0f;
                        m_currentPigAlertState = AlertState.LostPlayer;
                        m_hasPlayedAlertSound = false;
                    }
                    break;
                }    

                case AlertState.LostPlayer:
                {
                    m_currentAlertStateTimer += Time.deltaTime;
                    if ( m_currentAlertStateTimer >= m_secondsTilBackToStart )
                    {
                        m_currentAlertStateTimer = 0.0f;
                        m_currentPigAlertState = AlertState.BackToStart;
                    }
                    break;
                }

                case AlertState.BackToStart:
                {
                    //if ( m_currentDirection.x * transform.position.x >= m_currentDirection.x * m_pigHomePoint.x )
//                     if (transform.position.x == m_pigHomePoint.x)
//                     {
// 
//                     }
                    break;
                }
            }
        }

        //--------------------------------------------------
        //					***Moving Code***
        //Set the rigidbody2D velocity so it can move
        //the object, if it's not dead
        rigidbody2D.velocity = m_movement;
	}
	

    //---------------------------------------------------------------------------
	//Flip the art facing and movement direction
	void Flip()
	{	
		//Reverse the facing of the AI
		m_isFacingRight = !m_isFacingRight;
		
		//Reverse the art facing
		Vector3 theScale = this.transform.localScale;
		theScale.x *= -1;
		this.transform.localScale = theScale;
		
		//REverse the movement direction
		m_currentDirection.x *= -1;
	}


    //---------------------------------------------------------------------------
    void FlipCurrentMovement()
    {
        if (m_currentMoveDirection == PigInitialDirection.Left)
        {
            m_currentMoveDirection = PigInitialDirection.Right;
        }
        else
        {
            m_currentMoveDirection = PigInitialDirection.Left;
        }
    }


    //---------------------------------------------------------------------------
    void FlipBehavior()
    {
        m_currentFlipTimerSeconds += Time.deltaTime;

        if ( m_currentFlipTimerSeconds >= m_flipWaitSeconds )
        {
            m_currentFlipTimerSeconds = 0.0f;
            FlipCurrentMovement();
            Flip();
        }
    }
	

    //---------------------------------------------------------------------------
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		//If AI collides with reverseObjectName then Flip():
		//		reverse art facing and move direction
 		if(otherCollider.gameObject.name == m_reverseObjectName && m_currentPigAlertState == AlertState.NotAlert)
		{
            FlipCurrentMovement();
            Flip();
        }
        if ( otherCollider.gameObject.layer == 11 )
        {
            FlipCurrentMovement();
            Flip();
        }

        if (otherCollider.gameObject.layer == 8 /* player */)
        {
            if (m_canSeePlayer)
            {
                m_currentPigAlertState = AlertState.CaughtPlayer;
            }
            else
            {
                if ( !otherCollider.gameObject.GetComponent<PlayerController>().isMovingUp &&
                    !otherCollider.gameObject.GetComponent<PlayerController>().isMovingDown )
                {
                    FlipCurrentMovement();
                    Flip();
                }
            }
        }
	}
	

    //---------------------------------------------------------------------------
	void OnCollisionEnter2D(Collision2D otherCollider)
	{
        if ( m_currentPigAlertState != AlertState.NotAlert )
		{
        	// player layer = 8, wall layer = 11
            if ( otherCollider.gameObject.layer == 11 /* wall */ )
            {
                FlipCurrentMovement();
                Flip();
            }
       
        }
	}


    //---------------------------------------------------------------------------
    public void Respawn()
    {
        transform.position = m_pigHomePoint;
        m_currentPigAlertState = AlertState.NotAlert;
        m_currentMoveDirection = m_initialMoveDirection;
        //m_isFacingRight = m_isFacingRightHolder;
        m_movement = new Vector2(0f, rigidbody2D.velocity.y);
        if (!m_isFacingRight || m_initialMoveDirection == PigInitialDirection.Left) Flip();

        m_anim.SetBool("isAlert", false);
        m_anim.SetBool("caughtPlayer", false);
        this.gameObject.layer = 19;
    }


    //---------------------------------------------------------------------------
    private void PlaySound(AudioClip soundToPlay)
    {
        m_audioSource.clip = soundToPlay;
        m_audioSource.Play();
    }

}
