using UnityEngine;
using System.Collections;

public class LevelSelectScript : MonoBehaviour 
{
    public enum LevelSelected
    {
        LEVEL1,
        LEVEL2,
        LEVEL3,
        LEVEL4,
        EXIT,
    };

    public LevelSelected selected;

    public GUITexture Level1Selected;
    public GUITexture Level2Selected;
    public GUITexture Level3Selected;
    public GUITexture Level4Selected;
    public GUITexture StartLevel;
    public GUITexture BackButton;

    public string levelOneName;
    public string levelTwoName;
    public string levelThreeName;
    public string levelFourName;
    public Texture Level1Texture;
    public Texture Level2Texture;
    public Texture Level3Texture;
    public Texture Level4Texture;
    public GameObject SelectedLevel;
    public GUIText SelectedLevelName;
    private GUITexture selectedLevelTexture;
    private GUIText selectedLevelText;
    private GUIText selectedLevelTitle;
    private string textBaseString;
    private string selectedLevelKeyBase;
    private string highScoreKey = " High Score";
    private string maxScoreKey = " Max Score";
    private string highScoreNotFound = "-- / --";

    public GameObject pointer;

    public bool pressed;

    public float joystickInputDelay = 0.5f;

    private bool processedInput;
    private float startedInputTimer;

    public AudioSource audioSource;
    public AudioClip navigationSound;
    public AudioClip buttonPressSound;

    private bool moveUp;
    private bool moveDown;
    public bool level1Pointed = true;
    public bool level2Pointed;
    public bool level3Pointed;
    public bool level4Pointed;
    private bool startButtonPressed;
    private bool backButtonPressed;

	// Use this for initialization
	void Start () 
    {
        selectedLevelTexture = SelectedLevel.GetComponent<GUITexture>();
        selectedLevelText = SelectedLevel.GetComponent<GUIText>();
        textBaseString = SelectedLevel.GetComponent<GUIText>().text;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Time.unscaledTime - startedInputTimer >= joystickInputDelay)
        {
            startedInputTimer = Time.unscaledTime;
            processedInput = false;
        }

        CheckMouseInput();
        CheckKeyboardAndControllerInput();

	}

    void OnGUI()
    {
        
    }

    private void CheckMouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            pressed = (Level1Selected.HitTest(Input.mousePosition) || Level2Selected.HitTest(Input.mousePosition) || Level3Selected.HitTest(Input.mousePosition) || Level4Selected.HitTest(Input.mousePosition) || BackButton.HitTest(Input.mousePosition) || StartLevel.HitTest(Input.mousePosition));
            level1Pointed      = Level1Selected.HitTest(Input.mousePosition);
            level2Pointed      = Level2Selected.HitTest(Input.mousePosition);
            level3Pointed      = Level3Selected.HitTest(Input.mousePosition);
            level4Pointed      = Level4Selected.HitTest(Input.mousePosition);
            startButtonPressed = StartLevel.HitTest(Input.mousePosition);
            backButtonPressed = BackButton.HitTest(Input.mousePosition);
            ProcessButtonPress();
        }
    }

    private void CheckKeyboardAndControllerInput()
    {
        moveUp = (!processedInput && (Input.GetButtonDown("Move Up") || (Input.GetAxis("Vertical") > 0.5f)));

        moveDown = (!processedInput && (Input.GetButtonDown("Move Down") || (Input.GetAxis("Vertical") < -0.5f)));

        startButtonPressed = (Input.GetButtonDown("Fire2"));
        backButtonPressed = (Input.GetButtonDown("Fire3"));

        pressed = (startButtonPressed || backButtonPressed);

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

        ProcessButtonPress();

    }

    private void ProcessButtonPress()
    {
        //if(pressed)
        //{
            if(level1Pointed)
            {
                pointer.transform.position = new Vector3(pointer.transform.position.x, Level1Selected.transform.position.y, pointer.transform.position.z);
                selectedLevelTexture.texture = Level1Texture;
                SelectedLevelName.text = levelOneName;
                selectedLevelKeyBase = "RobbinRobin_Level1";

                if ( PlayerPrefs.HasKey( selectedLevelKeyBase + highScoreKey ) )
                {
                    selectedLevelText.text = textBaseString + " " + PlayerPrefs.GetInt(selectedLevelKeyBase + highScoreKey) + " / " + PlayerPrefs.GetInt(selectedLevelKeyBase + maxScoreKey);
                }
                else
                {
                    selectedLevelText.text = textBaseString + " " + highScoreNotFound;
                }
            }
            
            if (level2Pointed)
            {
                pointer.transform.position = new Vector3(pointer.transform.position.x, Level2Selected.transform.position.y, pointer.transform.position.z);
                selectedLevelTexture.texture = Level2Texture;
                SelectedLevelName.text = levelTwoName;

                selectedLevelKeyBase = "RobbinRobin_Level2";

                if (PlayerPrefs.HasKey(selectedLevelKeyBase + highScoreKey))
                {
                    selectedLevelText.text = textBaseString + " " + PlayerPrefs.GetInt(selectedLevelKeyBase + highScoreKey) + " / " + PlayerPrefs.GetInt(selectedLevelKeyBase + maxScoreKey);
                }
                else
                {
                    selectedLevelText.text = textBaseString + " " + highScoreNotFound;
                }
            }
            
            if (level3Pointed)
            {
                pointer.transform.position = new Vector3(pointer.transform.position.x, Level3Selected.transform.position.y, pointer.transform.position.z);
                selectedLevelTexture.texture = Level3Texture;
                selectedLevelKeyBase = "RobbinRobin_Level3";
                SelectedLevelName.text = levelThreeName;

                if (PlayerPrefs.HasKey(selectedLevelKeyBase + highScoreKey))
                {
                    selectedLevelText.text = textBaseString + " " + PlayerPrefs.GetInt(selectedLevelKeyBase + highScoreKey) + " / " + PlayerPrefs.GetInt(selectedLevelKeyBase + maxScoreKey);
                }
                else
                {
                    selectedLevelText.text = textBaseString + " " + highScoreNotFound;
                }
            }
            
            if (level4Pointed)
            {
                pointer.transform.position = new Vector3(pointer.transform.position.x, Level4Selected.transform.position.y, pointer.transform.position.z);
                selectedLevelTexture.texture = Level4Texture;
                selectedLevelKeyBase = "RobbinRobin_Level4";
                SelectedLevelName.text = levelFourName;

                if (PlayerPrefs.HasKey(selectedLevelKeyBase + highScoreKey))
                {
                    selectedLevelText.text = textBaseString + " " + PlayerPrefs.GetInt(selectedLevelKeyBase + highScoreKey) + " / " + PlayerPrefs.GetInt(selectedLevelKeyBase + maxScoreKey);
                }
                else
                {
                    selectedLevelText.text = textBaseString + " " + highScoreNotFound;
                }
            }

            if(startButtonPressed)
            {
                if(level1Pointed)
                {
                    Application.LoadLevel("RobbinRobin_Level1");
                }
                else if(level2Pointed)
                {
                    Application.LoadLevel("RobbinRobin_Level2");
                }
                else if (level3Pointed)
                {
                    Application.LoadLevel("RobbinRobin_Level3");
                }
                else if (level4Pointed)
                {
                    Application.LoadLevel("RobbinRobin_Level4");
                }
            }

            if(backButtonPressed)
            {
                Application.LoadLevel("RobbinRobin_MainMenu");
            }
        //}
    }

    private void MoveHighlighter(bool moveUp)
    {
        audioSource.clip = navigationSound;
        audioSource.Play();

        if (moveUp)
        {
            if(level1Pointed)
            {
                level1Pointed = !level1Pointed;
                level2Pointed = !level2Pointed;
                pointer.transform.position = new Vector3(pointer.transform.position.x, Level2Selected.transform.position.y, pointer.transform.position.z);
            }
            else if(level2Pointed)
            {
                level2Pointed = !level2Pointed;
                level3Pointed = !level3Pointed;
                pointer.transform.position = new Vector3(pointer.transform.position.x, Level3Selected.transform.position.y, pointer.transform.position.z);
            }
            else if (level3Pointed)
            {
                level3Pointed = !level3Pointed;
                level4Pointed = !level4Pointed;
                pointer.transform.position = new Vector3(pointer.transform.position.x, Level4Selected.transform.position.y, pointer.transform.position.z);
            }
            else if (level4Pointed)
            {
                level4Pointed = !level4Pointed;
                level1Pointed = !level1Pointed;
                pointer.transform.position = new Vector3(pointer.transform.position.x, Level1Selected.transform.position.y, pointer.transform.position.z);
            }
        }
        else
        {
            if (level1Pointed)
            {
                level1Pointed = !level1Pointed;
                level4Pointed = !level4Pointed;
                pointer.transform.position = new Vector3(pointer.transform.position.x, Level4Selected.transform.position.y, pointer.transform.position.z);
            }
            else if (level2Pointed)
            {
                level2Pointed = !level2Pointed;
                level1Pointed = !level1Pointed;
                pointer.transform.position = new Vector3(pointer.transform.position.x, Level1Selected.transform.position.y, pointer.transform.position.z);
            }
            else if (level3Pointed)
            {
                level3Pointed = !level3Pointed;
                level2Pointed = !level2Pointed;
                pointer.transform.position = new Vector3(pointer.transform.position.x, Level2Selected.transform.position.y, pointer.transform.position.z);
            }
            else if (level4Pointed)
            {
                level4Pointed = !level4Pointed;
                level3Pointed = !level3Pointed;
                pointer.transform.position = new Vector3(pointer.transform.position.x, Level3Selected.transform.position.y, pointer.transform.position.z);
            }
        }
    }
}
