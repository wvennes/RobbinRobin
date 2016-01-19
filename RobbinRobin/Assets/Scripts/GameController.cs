using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    private bool m_isPaused = false;
    private bool m_isFire2 = false;
    private bool m_justResumed = false;
    
    public AudioSource audioSource;
    public AudioClip navigationSound;
    public AudioClip buttonPressSound;

    public bool texturesEnabled = false;

    public GUITexture PauseMenu;
    public GUITexture ResumeButton;
    public GUITexture RestartButton;
    public GUITexture MainMenuButton;
    public GUITexture HighlightedButtonPointer;
    //private Vector3 HighlightedButtonPointerDefaultPosition;

    public GUIText TextResumeButton;
    public GUIText TextRestartButton;
    public GUIText TextMainMenuButton;

    public GUITexture EndOfLevelScreen;
    public GUITexture EOLTurkeyTexture;
    public GUITexture EOLCheeseTexture;
    public GUITexture EOLCakeTexture;
    public GUIText EOLFoodCollectedText;

    public bool resumeHighlighted = true;
    public bool restartHighlighted = false;
    public bool mainMenuHighlighted = false;

    public Color highlightedColor;
    public Color unHighlightedColor;
    
    public float restartLevelDelay = 0.15f;
    public float joystickInputDelay = 0.5f;

    private bool processedInput = false;
    private float startedInputTimer = 0.0f;
    public bool endOfLevel = false;
    private bool m_isFire3;

    private InventoryController invController;
    private GameObject playerObject;

    // Use this for initialization
    void Start()
    {
        playerObject = GameObject.Find("Player_Robin_Prefab");
        highlightedColor.a = 128;
        unHighlightedColor.a = 128;
        invController = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryController>();
        EOLFoodCollectedText.text = "Level Completed! \n You Got "+invController.Pickup1InitialValue+" / "+ invController.Pickup1MaxValue+" Food";
    }

    // Update is called once per frame
    void Update()
    {
        m_isFire2 = Input.GetButtonDown("Fire2");
        m_isFire3 = Input.GetButtonDown("Fire3");

        if(m_isPaused)
        {
            if (Time.unscaledTime - startedInputTimer >= joystickInputDelay)
            {
                startedInputTimer = Time.unscaledTime;
                processedInput = false;
            }
        }

        if (!m_justResumed && Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
       
        if (m_justResumed)
        {
            m_justResumed = false;
        }
    }

    void PauseGame()
    {
        m_isPaused = true;
        Time.timeScale = 0f;

        playerObject.GetComponent<PlayerController>().enabled = false;

        TutorialHUD[] huds = GameObject.FindObjectsOfType<TutorialHUD>();
        foreach (TutorialHUD hud in huds)
        {
            hud.enabled = false;
        }

        startedInputTimer = Time.unscaledTime;

        if (texturesEnabled)
        {
            if(!endOfLevel)
            {
                PauseMenu.enabled = true;
                ResumeButton.enabled = true;
                RestartButton.enabled = true;
                MainMenuButton.enabled = true;
                TextResumeButton.enabled = true;
                TextRestartButton.enabled = true;
                TextMainMenuButton.enabled = true;
                HighlightedButtonPointer.enabled = true;

                resumeHighlighted = true;
                restartHighlighted = false;
                mainMenuHighlighted = false;
            }
            else
            {
                EOLFoodCollectedText.text = "Level Completed! \n You Got " + invController.Pickup1InitialValue + " / " + invController.Pickup1MaxValue + " Food";
                EndOfLevelScreen.enabled = true;
                EOLTurkeyTexture.enabled = true; 
                EOLCheeseTexture.enabled = true; 
                EOLCakeTexture.enabled = true; 
                EOLFoodCollectedText.enabled = true;

                string currentScoreKey = Application.loadedLevelName + " High Score";
                string maxScoreKey = Application.loadedLevelName + " Max Score";
                if ( PlayerPrefs.HasKey( currentScoreKey ) )
                {
                    if (invController.Pickup1InitialValue > PlayerPrefs.GetInt(currentScoreKey))
                    {
                        PlayerPrefs.SetInt(currentScoreKey, (int) invController.Pickup1InitialValue);
                    }
                }
                else
                {
                    PlayerPrefs.SetInt(currentScoreKey, (int) invController.Pickup1InitialValue);
                }

                PlayerPrefs.SetInt(maxScoreKey, (int)invController.Pickup1MaxValue);
            }
        }
    }

    void ResumeGame()
    {
        Time.timeScale = 1f;
        m_isPaused = false;
        m_justResumed = true;

        playerObject.GetComponent<PlayerController>().enabled = true;

        TutorialHUD[] huds = GameObject.FindObjectsOfType<TutorialHUD>();
        foreach ( TutorialHUD hud in huds )
        {
            hud.enabled = true;
        }

        if (texturesEnabled)
        {
            if(!endOfLevel)
            {
                PauseMenu.enabled = false;
                ResumeButton.enabled = false;
                RestartButton.enabled = false;
                MainMenuButton.enabled = false;
                TextResumeButton.enabled = false;
                TextRestartButton.enabled = false;
                TextMainMenuButton.enabled = false;
                HighlightedButtonPointer.enabled = false;
                EndOfLevelScreen.enabled = false;
                EOLTurkeyTexture.enabled = false;
                EOLCheeseTexture.enabled = false;
                EOLCakeTexture.enabled = false;
                EOLFoodCollectedText.enabled = false;

                resumeHighlighted = false;
                restartHighlighted = false;
                mainMenuHighlighted = false;
            }
        }
    }

    public void TogglePause(bool playSound = true)
    {
        if(playSound)
        {
            audioSource.clip = buttonPressSound;
            audioSource.Play();
        }

        processedInput = true;

        if (m_isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    void OnGUI()
    {

        if (m_isPaused)
        {
            if (!texturesEnabled)
            {
                // Make a background box
                GUI.Box(new Rect(10, 10, 100, 90), "Pause Menu");

                // Make the first button. If it is pressed, Resumes the game by calling TogglePause
                if (GUI.Button(new Rect(20, 40, 80, 20), "Resume Game") || Input.GetButtonDown("Pause"))
                {
                    TogglePause();
                }

                // Make the second button.
                if (GUILayout.Button(/*new Rect(20, 70, 80, 20), */"Quit To Main Menu"))
                {
                    //Application.LoadLevel(2);
                }
            }

            else
            {
                //CheckMouseInput();
                CheckKeyboardAndControllerInput();

                if(!endOfLevel)
                {
                    if (resumeHighlighted)
                    {
                        if (ResumeButton.color != highlightedColor)
                        {
                            ResumeButton.color = highlightedColor;
                        }
                        if (HighlightedButtonPointer.transform.position.y != ResumeButton.transform.position.y)
                        {
                            HighlightedButtonPointer.transform.position = new Vector3(HighlightedButtonPointer.transform.position.x, ResumeButton.transform.position.y, HighlightedButtonPointer.transform.position.z);
                        }
                    }
                    else
                    {
                        if (ResumeButton.color != unHighlightedColor)
                        {
                            ResumeButton.color = unHighlightedColor;
                        }
                    }

                    if (restartHighlighted)
                    {
                        if (RestartButton.color != highlightedColor)
                        {
                            RestartButton.color = highlightedColor;
                        }
                        if (HighlightedButtonPointer.transform.position.y != RestartButton.transform.position.y)
                        {
                            HighlightedButtonPointer.transform.position = new Vector3(HighlightedButtonPointer.transform.position.x, RestartButton.transform.position.y, HighlightedButtonPointer.transform.position.z);
                        }
                    }
                    else
                    {
                        if (RestartButton.color != unHighlightedColor)
                        {
                            RestartButton.color = unHighlightedColor;
                        }
                    }

                    if (mainMenuHighlighted)
                    {
                        if (MainMenuButton.color != highlightedColor)
                        {
                            MainMenuButton.color = highlightedColor;
                        }
                        if (HighlightedButtonPointer.transform.position.y != MainMenuButton.transform.position.y)
                        {
                            HighlightedButtonPointer.transform.position = new Vector3(HighlightedButtonPointer.transform.position.x, MainMenuButton.transform.position.y, HighlightedButtonPointer.transform.position.z);
                        }
                    }
                    else
                    {
                        if (MainMenuButton.color != unHighlightedColor)
                        {
                            MainMenuButton.color = unHighlightedColor;
                        }
                    }
                }
                else
                {

                }
            }
        }

    }

    private void CheckMouseInput()
    {
        if (Input.GetMouseButton(0))
        {
            if (ResumeButton.enabled && ResumeButton.HitTest(Input.mousePosition))
            {
                TogglePause();
            }
            else if (RestartButton.enabled && RestartButton.HitTest(Input.mousePosition))
            {
                TogglePause();
                StartCoroutine("RestartLevel");
            }
            else if (MainMenuButton.enabled && MainMenuButton.HitTest(Input.mousePosition))
            {
                TogglePause();
                Application.Quit();
            }
        }
    }

    private void CheckKeyboardAndControllerInput()
    {
        if(!endOfLevel)
        {
            if (!processedInput && (Input.GetButtonDown("Move Up") || (Input.GetAxis("Vertical") > 0.5f)))
            {
                processedInput = true;
                MoveHighlighter(true);
                startedInputTimer = Time.unscaledTime;
            }
            if (!processedInput && (Input.GetButtonDown("Move Down") || (Input.GetAxis("Vertical") < -0.5f)))
            {
                processedInput = true;
                MoveHighlighter(false);
                startedInputTimer = Time.unscaledTime;
            }
            if (/*Input.GetButtonDown("Fire2") */m_isFire2)
            {
                if (resumeHighlighted)
                {
                    TogglePause();
                }
                else if (restartHighlighted)
                {
                    TogglePause();
                    StartCoroutine("RestartLevel");
                }
                else if (mainMenuHighlighted)
                {
                    TogglePause();
                    StartCoroutine("LoadMainMenu");
                }
            }
        }
        else
        {
            if (m_isFire2 || m_isFire3)
            {
                if (endOfLevel)
                {
                    endOfLevel = false;
                    //Application.LoadLevel(Application.loadedLevel);
                    TogglePause();
                }
            }
        }
    }

    private void MoveHighlighter(bool moveUp)
    {
        audioSource.clip = navigationSound;
        audioSource.Play();

        if(moveUp)
        {
            if(resumeHighlighted)
            {
                resumeHighlighted = false;
                restartHighlighted = false;
                mainMenuHighlighted = true;
            }
            else if(restartHighlighted)
            {
                resumeHighlighted = true;
                restartHighlighted = false;
                mainMenuHighlighted = false;
            }
            else if(mainMenuHighlighted)
            {
                resumeHighlighted = false;
                restartHighlighted = true;
                mainMenuHighlighted = false;
            }
        }
        else
        {
            if (resumeHighlighted)
            {
                resumeHighlighted = false;
                restartHighlighted = true;
                mainMenuHighlighted = false;
            }
            else if (restartHighlighted)
            {
                resumeHighlighted = false;
                restartHighlighted = false;
                mainMenuHighlighted = true;
            }
            else if (mainMenuHighlighted)
            {
                resumeHighlighted = true;
                restartHighlighted = false;
                mainMenuHighlighted = false;
            }
        }
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(restartLevelDelay);

        Application.LoadLevel(Application.loadedLevel);
    }

    IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(restartLevelDelay);

        Application.LoadLevel("RobbinRobin_MainMenu");
    }
}
