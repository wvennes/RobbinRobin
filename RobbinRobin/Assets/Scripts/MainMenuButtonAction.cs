using UnityEngine;
using System.Collections;

public class MainMenuButtonAction : MonoBehaviour 
{

    public GUITexture button;

    public bool pressed;

    public GUITexture pointerImage;

    public ButtonListAndState highlightedButton;
    public ButtonListAndState previousButton;
    public ButtonListAndState nextButton;

    public GUITexture highlightedButtonTexture;
    public GUITexture previousButtonTexture;
    public GUITexture nextButtonTexture;

    public float joystickInputDelay = 0.2f;

    private bool processedInput;
    private float startedInputTimer;

    public AudioSource audioSource;
    public AudioClip navigationSound;
    public AudioClip buttonPressSound;

    private bool moveUp;
    private bool moveDown;

	// Use this for initialization
	void Start () 
    {
        button = highlightedButtonTexture;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Time.unscaledTime - startedInputTimer >= joystickInputDelay || (Input.GetButtonUp("Move Up") || Input.GetButtonUp("Move Down") ) )
        {
            startedInputTimer = Time.unscaledTime;
            processedInput = false;
        }

        CheckMouseInput();
        CheckKeyboardAndControllerInput();
	}

    void OnGUI()
    {

        if (pressed)
        {
            ProcessButtonPress();
        }

    }

    private void CheckMouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            pressed = (highlightedButton.isHighlighted && button.HitTest(Input.mousePosition));
        }
    }

    private void CheckKeyboardAndControllerInput()
    {
        moveUp = (!processedInput && (Input.GetButton("Move Up") || (Input.GetAxis("Vertical") > 0.5f)));
        
        moveDown = (!processedInput && (Input.GetButton("Move Down") || (Input.GetAxis("Vertical") < -0.5f)));
        
        if(Input.GetButtonDown("Fire2"))
        {
            int x = 0;
        }
        pressed = (Input.GetButtonDown("Fire2"));

        if (moveUp) 
        {
            processedInput = true;
            MoveHighlighter(true);
            startedInputTimer = Time.unscaledTime;
        }
        if (moveDown)
        {
            processedInput = true;
            MoveHighlighter(false);
            startedInputTimer = Time.unscaledTime;
        }
    }

    public void ProcessButtonPress()
    {
        audioSource.clip = buttonPressSound;
        audioSource.Play();
        switch(highlightedButton.actionToPerform)
        {
            case ButtonListAndState.ButtonActions.STARTGAME: StartGame();
                break;
            case ButtonListAndState.ButtonActions.LEVELSELECT: LevelSelect();
                break;
            case ButtonListAndState.ButtonActions.OPTIONS: Options();
                break;
            case ButtonListAndState.ButtonActions.CONTROLS: Controls();
                break;
            case ButtonListAndState.ButtonActions.CREDITS: Credits();
                break;
            case ButtonListAndState.ButtonActions.EXIT: ExitGame();
                break;
        }
    }

    private void StartGame()
    {
        Application.LoadLevel("RobbinRobin_IntroScene");
    }

    private void Options()
    {
        Application.LoadLevel("RobbinRobin_Options");
    }

    private void LevelSelect()
    {
        Application.LoadLevel("RobbinRobin_LevelSelect");
    }

    private void Controls()
    {
        Application.LoadLevel("RobbinRobin_Controls");
    }

    private void Credits()
    {
        Application.LoadLevel("RobbinRobin_Credits");
    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void MoveHighlighter(bool moveUp)
    {
        audioSource.clip = navigationSound;
        audioSource.Play();

        if (moveUp)
        {
            if (highlightedButton.isHighlighted)
            {
                highlightedButton.isHighlighted = false;
                previousButton.isHighlighted = true;

                ButtonListAndState temp = highlightedButton;
                nextButton = highlightedButton;
                highlightedButton = previousButton;
                previousButton = highlightedButton.previousButton;

                button = temp.previousButtonTexture;
                nextButtonTexture = highlightedButtonTexture;
                highlightedButtonTexture = previousButtonTexture;
                previousButtonTexture = previousButton.previousButtonTexture;

                pointerImage.transform.position = new Vector3(pointerImage.transform.position.x, highlightedButton.transform.position.y, pointerImage.transform.position.z);
            }
        }
        else
        {
            if (highlightedButton.isHighlighted)
            {
                highlightedButton.isHighlighted = false;
                nextButton.isHighlighted = true;

                ButtonListAndState temp = highlightedButton;
                previousButton = highlightedButton;
                highlightedButton = nextButton;
                nextButton = highlightedButton.nextButton;

                button = temp.nextButtonTexture;
                previousButtonTexture = highlightedButtonTexture;
                highlightedButtonTexture = nextButtonTexture;
                nextButtonTexture = nextButton.nextButtonTexture;

                pointerImage.transform.position = new Vector3(pointerImage.transform.position.x, highlightedButton.transform.position.y, pointerImage.transform.position.z);
            }
        }
    }
}
