/*MainMenu from DemoGirl
 * C22 TGP1 Game
 * by Evan Kohn
 * 
 * Commented by Morgan Wagnon for Guildhall Academy
 * 
*/
using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    //Drag and drop the texture of the menu background
	public Texture backgroundTexture;

	public Texture button1Texture;
	public Texture button2Texture;
	public Texture button3Texture;
	public Texture button4Texture;

    //Set up a GUIStyle variable to hold our font choices
	public GUIStyle style1;
	public GUIStyle style2;
	public GUIStyle style3;
	public GUIStyle style4;

	public Vector2 button1Scale = new Vector2(1f, 1f);
	public Vector2 button2Scale = new Vector2(1f, 1f);
	public Vector2 button3Scale = new Vector2(1f, 1f);
	public Vector2 button4Scale = new Vector2(1f, 1f);

	
    //How much to offset the buttons from the left
    //Zero would be the left side of the screen, while One would be the right side of the screen
	public float guiOffsetPercentageX = 0.75f;

    //How much to offset the first button from the top of the screen
    //Zero would be the top of the screen, while One would be the bottom of the screen
	public float guiOffsetY1 = 0.5f;

    //How much to offset the second button from the top of the screen
    //Zero would be the top of the screen, while One would be the bottom of the screen
	public float guiOffsetY2 = 0.6f;

    //How much to offset the third button from the top of the screen
    //Zero would be the top of the screen, while One would be the bottom of the screen
	public float guiOffsetY3 = 0.7f;

    //How much to offset the fourth button from the top of the screen
    //Zero would be the top of the screen, while One would be the bottom of the screen
	public float guiOffsetY4 = 0.8f;
	
    //Array that holds the names of all of the menu buttons
	string[] menu_ids = {"start", "controls", "credits", "quit"};

    //If using a controller, this variable keeps track of which option the player currently has selected, and starts at 0- the play button
	int current_option = 0; 
	
    //How long to wait in seconds before the player is able to change the current button selection with the controller
	public float optionChangeDelay = .25f;

    //Tracks how long it has been since the player changed the current button selected
	private float optionChangeTimer;
	
    //Set up the variable "mousePos" as a vector to hold where the mouse is moving
	private Vector2 mousePos = new Vector2(0,0);

    //By default, we assume that the mouse is not active yet
	private bool mouseActive = false;

    //Is this the first time the script is running?
	private bool onstart = true;
	
    //Type in the name of the first level to be played when the player presses start
	public string levelOne;

    //Picture to display when the player presses controls
	public Texture controlsTexture;

    //Picture to display when the player presses credits
	public Texture creditsTexture;

    //Am I currently showing the controls picture?
	private bool showingControls;

    //Am I currently showing the credits picture?
	private bool showingCredits;

    void Start()
    {

    }
	
    //Called each time something is first rendered on the screen- so can be called many times very quickly at the start!
	void OnGUI()
	{
		if( Input.GetButtonDown("Accept") )
			//Debug.Log("Pressed Accept");
		if( Input.GetButtonDown("Cancel") )
			//Debug.Log("Pressed Cancel");

        //If the variable "onstart" is true...
		if (onstart)
		{
            //Set the variable "mousePos" to be equal to the current position of the mouse cursor on screen
			mousePos = Event.current.mousePosition;

            //Set the variable "onstart" to false, so that this function will not run again
			onstart = false;

            //Delete all pickup and life options from the last time this game was played
			PlayerPrefs.DeleteAll();

            //Set the font size of style1 to be equal to the screen width multiplied by .028
            style1.fontSize = (int)(Screen.width * .028);
		}
		
		// Display background texture
		GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), backgroundTexture);
		
		// Display first button in the array
		GUI.SetNextControlName(menu_ids[0]);

        //If the script is at the "start" item in the menu_ids array, render a button that is a new rectangle using
        //Rect(X position of left side of button, Y position of top side of button, width of button, height of button)
        //and put the word "Start" on it, in the GUI style of "style1"
		bool start;
		if( button1Texture )
		{
			start = (GUI.Button(new Rect(Screen.width * guiOffsetPercentageX, Screen.height * guiOffsetY1, button4Texture.width * button1Scale.x, button1Texture.height * button1Scale.y), button1Texture, style1));
		}
		else
		{
			start = (GUI.Button(new Rect(Screen.width * guiOffsetPercentageX, Screen.height * guiOffsetY1, Screen.width * .1f, Screen.height * .1f), "Start", style1));
		}

        //If the start button on screen is clicked with a mouse OR you are using a controller and the current option selected is "start" AND you push the A button on the controller
		if (start ||/*OR*/ ((menu_ids[current_option] == "start") &&/*AND*/ Input.GetButtonDown("Accept")))
		{
            //Load level one that the designer named in the editor
			Application.LoadLevel(levelOne);
		}

        //Display second button in the array
		GUI.SetNextControlName(menu_ids[1]);

        //If the script is at the "controls" item in the menu_ids array, render a button that is a new rectangle using
        //Rect(X position of left side of button, Y position of top side of button, width of button, height of button)
        //and put the word "Controls" on it, in the GUI style of "style1"
		bool controls;// =(GUI.Button(new Rect(Screen.width * guiOffsetPercentageX, Screen.height * guiOffsetY2, Screen.width * .1f, Screen.height * .1f), "Controls", style1));
		if( button2Texture )
		{
			controls =(GUI.Button(new Rect(Screen.width * guiOffsetPercentageX, Screen.height * guiOffsetY2, button2Texture.width * button2Scale.x, button2Texture.height * button2Scale.y), button2Texture, style2));
		}
		else
		{
			controls =(GUI.Button(new Rect(Screen.width * guiOffsetPercentageX, Screen.height * guiOffsetY2, Screen.width * .1f, Screen.height * .1f), "Controls", style1));
		}
		
		
        //If the controls button on screen is clicked with a mouse OR you are using a controller and the current option selected is "controls" AND you push the A button on the controller
		if (controls ||/*OR*/ ((menu_ids[current_option] == "controls") &&/*AND*/ Input.GetButtonDown("Accept")))
		{
            //Set the variable "showingControls" to true
			showingControls = true;
		}
		
        //Display the third button in the array
		GUI.SetNextControlName(menu_ids[2]);

        //If the script is at the "credits" item in the menu_ids array, render a button that is a new rectangle using
        //Rect(X position of left side of button, Y position of top side of button, width of button, height of button)
        //and put the word "Credits" on it, in the GUI style of "style1"
		bool credits;// = (GUI.Button(new Rect(Screen.width * guiOffsetPercentageX, Screen.height * guiOffsetY3, Screen.width * .1f, Screen.height * .1f), "Credits", style1));
		if( button3Texture )
		{
			credits = (GUI.Button(new Rect(Screen.width * guiOffsetPercentageX, Screen.height * guiOffsetY3, button3Texture.width * button3Scale.x, button3Texture.height * button3Scale.y), button3Texture, style3));
		}
		else
		{
			credits = (GUI.Button(new Rect(Screen.width * guiOffsetPercentageX, Screen.height * guiOffsetY3, Screen.width * .1f, Screen.height * .1f), "Credits", style1));
		}
		
		//If the credits button is clicked on screen with a mouse OR you are using a controller and the current option selected is "credits" AND you push the A button on the controller
		if (credits ||/*OR*/ ((menu_ids[current_option] == "credits") &&/*AND*/ Input.GetButtonDown("Accept")))
		{
            //Set the variable "showingCredits" to true
			showingCredits = true;
		}

        //Display the fourth button in the array
		GUI.SetNextControlName(menu_ids[3]);

        //If the script is at the "quit" item in the menu_ids array, render a button that is a new rectangle using
        //Rect(X position of left side of button, Y position of top side of button, width of button, height of button)
        //and put the word "Quit" on it, in the GUI style of "style1"
		bool quit;// = (GUI.Button(new Rect(Screen.width * guiOffsetPercentageX, Screen.height * guiOffsetY4, Screen.width * .1f, Screen.height * .1f), "Quit", style1));
		if( button4Texture )
		{
			quit = (GUI.Button(new Rect(Screen.width * guiOffsetPercentageX, Screen.height * guiOffsetY4, button4Texture.width * button4Scale.x, button4Texture.height * button4Scale.y), button4Texture, style4));
		}
		else
		{
			quit = (GUI.Button(new Rect(Screen.width * guiOffsetPercentageX, Screen.height * guiOffsetY4, Screen.width * .1f, Screen.height * .1f), "Quit", style1));
		}

		//If the quit button is clicked on screen with a mouse OR you are using a controller and the current option selected is "quit" AND you push the A button on the controller
		if (quit ||/*OR*/ ((menu_ids[current_option] == "quit") &&/*AND*/ Input.GetButtonDown("Accept")))
		{
            //Quit the game
			Application.Quit();	
		}

        //This if statement keeps track of the mouse position on the screen and tracks whether it is actively moving or not
        //If the variable "mousePos" is not equal to the current position...
		if (mousePos != Event.current.mousePosition)
		{
            //Set the variable "mousePos" to be equal to the current position
			mousePos = Event.current.mousePosition;

            //The player is using a mouse instead of a controller
			mouseActive = true;
		}
		
        //If the mouse is not active, in other words, if the player is using a controller
        if (!mouseActive)
        {
            //Set the focus control to the current option selected by the controller
            GUI.FocusControl(menu_ids[current_option]);
        }

            //Otherwise, if the player is using a mouse
            else
            {
                //Set the focus control to nothing
                GUI.FocusControl("");
            }
			
        //If the variable "showingControls" is true...
        if (showingControls)
        {
            //Draw the "controlsTexture" set in the editor on the screen such that it is positioned in the top left corner of the screen
            //and covers the entire screen.
            GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), controlsTexture);
        }

            //If the variable "showingCredits" is true...
            else if (showingCredits)
            {
                //Draw the "controlsTexture" set in the editor on the screen such that it is positioned in the top left corner of the screen
                //and covers the entire screen.
                GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), creditsTexture);
            }
	}
	
    //This function is called once per frame, so fires often
	void Update()
	{
        //If the variable "showingControls" is not true AND the variable "showingCredits" is not true...
		if (!showingControls &&/*AND*/ !showingCredits)
		{
            //Subtract the fixed time that the last frame took from the variable "optionChangeTimer"
            //This creates a little bit of delay between switching the currently selected option when using a controller
			optionChangeTimer -= Time.deltaTime;
		
            //Get from the player's controller or mouse, their input, where their cursor is on the Y axis (up and down)
			float inputY = Input.GetAxis ("Vertical");

            //If the variable "optionChangeTimer" is less than zero AND the player is pushing the controller stick down...
            if (optionChangeTimer < 0f &&/*AND*/ inputY < -.5f)
			{
                //Increase the variable "current_option" by one
				current_option++;

                //Set the variable "optionChangeTimer" to be equal to the variable "optionChangeDelay"
				optionChangeTimer = optionChangeDelay;

                //The player is using a controller, not a mouse
				mouseActive = false;
			}

                //Otherwise, if the variable "optionChangeTimer" is less than 0 AND the player is pushing the controller stick up...
                else if (optionChangeTimer < 0f &&/*AND*/ inputY > .5f)
			    {
                    //Decrease the variable "current_option" by one
				    current_option--;

                    //Set the variable "optionChangeTimer" to be equal to the variable "optionChangeDelay"
				    optionChangeTimer = optionChangeDelay;

                    //The player is using a controller, not a mouse
				    mouseActive = false;
			    }

                //Otherwise, if the up and down input from the controller (in either direction) is less than 0.5..
                    //This means they are not pushing the joystick very far in either direction
			    else if (Mathf.Abs(inputY) < .5f)
			    {
                    //Set the variable "optionChangeTimer" to zero, so that if they push the joystick up or down enough in the next frame, the currently selected option will change
				    optionChangeTimer = 0f;
			    }
			
            //If the player is on the "quit" button and pushes down, highlight the "play" button at the top of the list
			if (current_option > 3)
			{
				current_option = 0;
			}

                //If the player is on the "play" button and pushes up, highlight the "quit" button at the bottom of the list
			    else if (current_option < 0)
			    {
				    current_option = 3;
			    }
		}
		
        //If the controls are being displayed on the screean AND either the player player presses B on the controller OR escape on the keyboard...
        if (showingControls &&/*AND*/ (Input.GetButtonDown("Cancel")))
        {
            //Turn off the controls picture
            showingControls = false;
        }

            //Otherwise, iIf the controls are being displayed on the screean AND either the player player presses B on the controller OR escape on the keyboard...
		else if (showingCredits &&/*AND*/ (Input.GetButtonDown("Cancel")))
            {
                //Turn off the credits picture
                showingCredits = false;
            }
		
	}
	
}
