//TutorialHUD
//
//Turns the Enabled flag on and off for HUD elements
//
//By Nathan Bowden
//Based on the InvisibleObject by James Bowling; modified by Michael McCoy





using UnityEngine;
using System.Collections;

public class TutorialHUD : MonoBehaviour {


    //Time when can trigger again
//     private float timeCanRetriggerEnter = 0.0f;
//     private float timeCanRetriggerExit = 0.0f;
    private bool isWindowOpen = false;
    private bool hasClosed = false;

    //private 

    //Pointer to the guitext and guitexture objects
    public GUIText notifyGUIText;
    public GUITexture notifyGUITexture;

    public GUIText tutorialGUIText1;
    public GUITexture tutorialGUITexture1;
    public GUITexture tutorialGUIPortrait1;

    public GUIText tutorialGUIText2;
    public GUITexture tutorialGUITexture2;
    public GUITexture tutorialGUIPortrait2;

    public GUIText tutorialGUIText3;
    public GUITexture tutorialGUITexture3;
    public GUITexture tutorialGUIPortrait3;

    //variable to track when player is able to toggle tutorial screen
    private bool canTrigger;

    private int currentScreen = 0;

    public AudioClip m_popupSound;
    private AudioSource m_audioSource;





	// Use this for initialization
	void Start () 
    {

        //Set the initial value of the GUIText and GUITexture to disabled
        notifyGUIText.enabled = false;
        notifyGUITexture.enabled = false;

        if ( tutorialGUIText1 != null ) tutorialGUIText1.enabled = false;
        if (tutorialGUITexture1 != null) tutorialGUITexture1.enabled = false;
        if (tutorialGUIPortrait1 != null) tutorialGUIPortrait1.enabled = false;

        if (tutorialGUIText2 != null) tutorialGUIText2.enabled = false;
        if (tutorialGUITexture2 != null) tutorialGUITexture2.enabled = false;
        if (tutorialGUIPortrait2 != null) tutorialGUIPortrait2.enabled = false;

        if (tutorialGUIText3 != null) tutorialGUIText3.enabled = false;
        if (tutorialGUITexture3 != null) tutorialGUITexture3.enabled = false;
        if (tutorialGUIPortrait3 != null) tutorialGUIPortrait3.enabled = false;

	}
	
	// Update is called once per frame
	void Update () 
    {
        



        //the player is in the trigger and presses the right mouse button
        if (Input.GetButtonDown("Fire2") && canTrigger)
        {
            //toggle the tutorial GUIText and GUITexture
//             tutorialGUIText.enabled = !tutorialGUIText.enabled;
//             tutorialGUITexture.enabled = !tutorialGUITexture.enabled;

            switch ( currentScreen )
            {
//                 case 0:
//                 {
//                     tutorialGUIText1.enabled = false;
//                     tutorialGUITexture1.enabled = false;
//                     tutorialGUIPortrait1.enabled = false;
// 
//                     tutorialGUIText2.enabled = false;
//                     tutorialGUITexture2.enabled = false;
//                     tutorialGUIPortrait2.enabled = false;
// 
//                     tutorialGUIText3.enabled = false;
//                     tutorialGUITexture3.enabled = false;
//                     tutorialGUIPortrait3.enabled = false;
//                     isWindowOpen = false;
//                     break;
//                 }
                case 0:
                {
                    ++currentScreen;
                    if (tutorialGUIText1 != null) tutorialGUIText1.enabled = true;
                    if (tutorialGUITexture1 != null) tutorialGUITexture1.enabled = true;
                    if (tutorialGUIPortrait1 != null) tutorialGUIPortrait1.enabled = true;
                    isWindowOpen = true;
                    Play(m_popupSound);
                    break;
                }
                case 1:
                {
                    ++currentScreen;
                    if (tutorialGUIText1 != null) tutorialGUIText1.enabled = false;
                    if (tutorialGUITexture1 != null) tutorialGUITexture1.enabled = false;
                    if (tutorialGUIPortrait1 != null) tutorialGUIPortrait1.enabled = false;
                    Play(m_popupSound);

                    if (tutorialGUITexture2 != null)
                    {
                        tutorialGUIPortrait2.enabled = true;
                        tutorialGUIText2.enabled = true;
                        tutorialGUITexture2.enabled = true;
                    }
                    else
                    {
                        canTrigger = false;
                        hasClosed = true;
                        notifyGUIText.enabled = false;
                        notifyGUITexture.enabled = false;
                        isWindowOpen = false;
                        currentScreen = 0;
                    }
                    break;
                }
                case 2:
                {
                    ++currentScreen;
                    tutorialGUIPortrait2.enabled = false;
                    tutorialGUIText2.enabled = false;
                    tutorialGUITexture2.enabled = false;
                    Play(m_popupSound);

                    if (tutorialGUITexture3 != null)
                    {
                        tutorialGUIPortrait3.enabled = true;
                        tutorialGUIText3.enabled = true;
                        tutorialGUITexture3.enabled = true;
                    }
                    else
                    {
                        hasClosed = true;
                        canTrigger = false;
                        notifyGUIText.enabled = false;
                        notifyGUITexture.enabled = false;
                        isWindowOpen = false;
                        currentScreen = 0;
                    }
                    break;
                }
                case 3:
                {
                    hasClosed = true;
                    tutorialGUIPortrait3.enabled = false;
                    tutorialGUIText3.enabled = false;
                    tutorialGUITexture3.enabled = false;
                    isWindowOpen = false;
                    currentScreen = 0;
                    Play(m_popupSound);
                    canTrigger = false;
                    notifyGUIText.enabled = false;
                    notifyGUITexture.enabled = false;
                    break;
                }
            }

            //Time.timeScale = 0f;

            if ( isWindowOpen )
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
        
        if ( Input.GetButtonDown("Fire3") && canTrigger )
        {
            if (tutorialGUIText1 != null) tutorialGUIText1.enabled = false;
            if (tutorialGUITexture1 != null) tutorialGUITexture1.enabled = false;
            if (tutorialGUIPortrait1 != null) tutorialGUIPortrait1.enabled = false;

            if (tutorialGUIText2 != null) tutorialGUIText2.enabled = false;
            if (tutorialGUITexture2 != null) tutorialGUITexture2.enabled = false;
            if (tutorialGUIPortrait2 != null) tutorialGUIPortrait2.enabled = false;

            if (tutorialGUIText3 != null) tutorialGUIText3.enabled = false;
            if (tutorialGUITexture3 != null) tutorialGUITexture3.enabled = false;
            if (tutorialGUIPortrait3 != null) tutorialGUIPortrait3.enabled = false;

            hasClosed = true;
            isWindowOpen = false;
            currentScreen = 0;
            canTrigger = false;
            notifyGUIText.enabled = false;
            notifyGUITexture.enabled = false;
            Time.timeScale = 1f;
        }
	
	}


//     void OnGUI()
//     {
//         if ( isWindowOpen )
//         {
//             GUI.Box(new Rect(Screen.width * 0.5f, Screen.height * 0.5f, 500f, 500f), tutorialGUIText.text);
//         }
//     }

    
	//Run when the player collides with this trigger
    void OnTriggerEnter2D(Collider2D collider)
    {
        if ((collider.gameObject.tag == "Player") /*&& Time.time >= timeCanRetriggerEnter*/)
        {
            //when the player enters the collider, toggle the notification window on
            notifyGUIText.enabled = true;
            notifyGUITexture.enabled = true;
            canTrigger = true;
            hasClosed = false;
        }

        //Reset retrigger time
        /*timeCanRetriggerEnter = Time.time + 1.0f;*/

    }


    //---------------------------------------------------------------------------
    void OnTriggerStay2D(Collider2D collider)
    {
        if ((collider.gameObject.tag == "Player") /*&& Time.time >= timeCanRetriggerEnter*/ && !hasClosed)
        {
            //when the player enters the collider, toggle the notification window on
            notifyGUIText.enabled = true;
            notifyGUITexture.enabled = true;
            canTrigger = true;
            hasClosed = false;
        }
        else if (collider.gameObject.tag == "Player" && hasClosed)
        {
            notifyGUIText.enabled = false;
            notifyGUIText.enabled = false;
        }

        //Reset retrigger time
        /*timeCanRetriggerEnter = Time.time + 1.0f;*/
    }



    	//Activate when object exits collider
    void OnTriggerExit2D(Collider2D collider)
    {
        if ((collider.gameObject.tag == "Player") /*&& Time.time >= timeCanRetriggerExit*/)
        {
            //when the player exits the collider, toggle the notification window off
            notifyGUIText.enabled = false;
            notifyGUITexture.enabled = false;
//             tutorialGUIText.enabled = false;
//             tutorialGUITexture.enabled = false;
            canTrigger = false;
        }


        //Reset retrigger time
        /*timeCanRetriggerExit = Time.time + 1.0f;*/

    }

    //---------------------------------------------------------------------------
    public void Play(AudioClip sound)
    {
        //If sound exists
        if (sound != null)
            // As it is not 3D audio clip, position doesn't matter.
            AudioSource.PlayClipAtPoint(sound, transform.position);

    }
}




