using UnityEngine;
using System.Collections;

public class GameOptions : MonoBehaviour 
{
    private const float maxSFX   = 1.0f;
    private const float maxMusic = 0.5f;
    private const float sfxModifier = 0.2f;
    private const float musicModifier = 0.1f;

    [Range(0.0f, maxSFX)]
    public float sfxVolume = maxSFX;
    private float prevSoundVolume;

    public string[] MenuLevels;
    public string[] StoryLevels;
    public string[] GameLevels;

    [Range(0.0f, maxMusic)]
    public float musicVolume = maxMusic;
    private float prevMusicVolume;

    public int loadedLevel;
    public Font font;
    public Color color;

    public Rect SliderArea;
    public Rect MainArea;

    public float levelLoadDelay = 0.2f;

    private bool processedInput = false;
    private float startedInputTimer;

    public bool moveRight;
    public bool moveLeft;

    public bool moveUp;
    public bool moveDown;

    public float joystickInputDelay = 0.015f;
    public bool sfxHighlighted = true;
    public float verticalSpace = 3;

    public GameObject pointerObject;

    public AudioSource sfxSource;
    public AudioSource musicSource;
    public AudioSource indicatorSource;
    public AudioClip navigationSound;
    public AudioClip selectionSound;
    public AudioClip MenuMusic;
    public AudioClip StoryMusic;
    public AudioClip InGameMusic;

    public float hSpace = 5f;
    public float vSpace = 5f;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        
        sfxVolume = PlayerPrefs.GetFloat("SoundEffects", maxSFX);
        musicVolume = PlayerPrefs.GetFloat("MusicEffects", maxMusic);

        loadedLevel = Application.loadedLevel;

        StartCoroutine("LoadFirstLevel");
    }

    void OnEnable()
    {
        StartCoroutine("ApplyVolumes");
        loadedLevel = Application.loadedLevel;
    }

	// Use this for initialization
	void Start () 
    {
        SliderArea = new Rect(350.0f, 200.0f, 500.0f, 200.0f);
        MainArea = new Rect(450, 280f, 1920f, 1080f);

        prevMusicVolume = musicVolume;
        prevSoundVolume = sfxVolume;

        loadedLevel = Application.loadedLevel;

	}
	
	// Update is called once per frame
	void Update () 
    {
        ApplyVolumes();

        if(loadedLevel != Application.loadedLevel)
        {
            loadedLevel = Application.loadedLevel;

            PlayMusic();

            if (Application.loadedLevelName == "RobbinRobin_Options")
            {
                prevSoundVolume = sfxVolume;
                prevMusicVolume = musicVolume;

                pointerObject = GameObject.Find("PointerObject");
                pointerObject.transform.position = new Vector3(pointerObject.transform.position.x, 0.54f, pointerObject.transform.position.z);

                //sfxSource = GameObject.Find("SoundObject").GetComponent<AudioSource>();
            }
        }

        if(Application.loadedLevelName != "RobbinRobin_Options")
        {
            joystickInputDelay = 0.015f;
            if (sfxVolume != prevSoundVolume)
            {
                prevSoundVolume = sfxVolume;
                StartCoroutine("ApplyVolumes");
            }

            if (musicVolume != prevMusicVolume)
            {
                prevMusicVolume = musicVolume;
                StartCoroutine("ApplyVolumes");
            }
        }
        else
        {
            joystickInputDelay = 0.15f;
            if (processedInput && Time.unscaledTime - startedInputTimer >= joystickInputDelay)
            {
                startedInputTimer = Time.unscaledTime;
                processedInput = false;
            }

            moveRight = (!processedInput &&  (Input.GetAxis("Horizontal") > 0.5f));

            moveLeft = (!processedInput && (Input.GetAxis("Horizontal") < -0.5f));

            moveUp = (!processedInput && ( (Input.GetButtonDown("Move Up") || (Input.GetAxis("Vertical") > 0.5f)) ) );

            moveDown = (!processedInput && ( (Input.GetButtonDown("Move Down") || (Input.GetAxis("Vertical") < -0.5f)) ) );

            musicSource.volume = musicVolume;
           
            sfxSource.volume = sfxVolume;
           

            if (moveRight)
            {
                if(sfxHighlighted)
                {
                    indicatorSource.clip = selectionSound;
                    indicatorSource.volume = sfxVolume;
                    indicatorSource.Play();
                }
                else
                {
                    indicatorSource.clip = selectionSound;
                    indicatorSource.volume = musicVolume;
                    indicatorSource.Play();
                }

                processedInput = true;
                if(sfxHighlighted)
                {
                    sfxVolume += sfxModifier;
                    if(sfxVolume > maxSFX)
                    {
                        sfxVolume = maxSFX;
                    }
                }
                else
                {
                    musicVolume += musicModifier;
                    if (musicVolume > maxMusic)
                    {
                        musicVolume = maxMusic;
                    }
                }
                
                startedInputTimer = Time.unscaledTime;
            }

            if (moveLeft)
            {
                if (sfxHighlighted)
                {
                    indicatorSource.clip = selectionSound;
                    indicatorSource.volume = sfxVolume;
                    indicatorSource.Play();
                }
                else
                {
                    indicatorSource.clip = selectionSound;
                    indicatorSource.volume = musicVolume;
                    indicatorSource.Play();
                }

                processedInput = true;
                if(sfxHighlighted)
                {
                    sfxVolume -= sfxModifier;
                    if (sfxVolume < sfxModifier)
                    {
                        sfxVolume = 0.0f;
                    }
                }
                else
                {
                    musicVolume -= musicModifier;
                    if (musicVolume < musicModifier || musicVolume > maxMusic)
                    {
                        musicVolume = 0.0f;
                    }
                }

                startedInputTimer = Time.unscaledTime;
            }

            if (moveUp)
            {
                indicatorSource.clip = selectionSound;
                indicatorSource.volume = sfxVolume;
                indicatorSource.Play();

                processedInput = true;
                sfxHighlighted = !sfxHighlighted;
                if(sfxHighlighted)
                {
                    pointerObject.transform.position = new Vector3(pointerObject.transform.position.x, 0.54f, pointerObject.transform.position.z);
                }
                else
                {
                    pointerObject.transform.position = new Vector3(pointerObject.transform.position.x, 0.47f, pointerObject.transform.position.z);
                }

                startedInputTimer = Time.unscaledTime;
            }

            if (moveDown)
            {
                indicatorSource.clip = selectionSound;
                indicatorSource.volume = sfxVolume;
                indicatorSource.Play();

                processedInput = true;
                sfxHighlighted = !sfxHighlighted;
                if (sfxHighlighted)
                {
                    pointerObject.transform.position = new Vector3(pointerObject.transform.position.x, 0.54f, pointerObject.transform.position.z);
                }
                else
                {
                    pointerObject.transform.position = new Vector3(pointerObject.transform.position.x, 0.47f, pointerObject.transform.position.z);
                }

                startedInputTimer = Time.unscaledTime;
            }

            if ( /*( DoneButton.HitTest(Input.mousePosition) && (Input.GetMouseButtonDown(0)) ) || */Input.GetButtonDown("Fire2") )
            {
                ProcessDone();
            }
            if ( /*( ResetButton.HitTest(Input.mousePosition) && (Input.GetMouseButtonDown(0)) ) || */Input.GetButtonDown("Fire3") )
            {
                ProcessReset();
            }

        }
	}

    void OnDrawGizmos()
    {
        Mathf.Clamp(sfxVolume, 0f, maxSFX);
        Mathf.Clamp(musicVolume, 0f, maxMusic);
    }

    void ApplyVolumes()
    {
        AudioSource[] sources = GameObject.FindObjectsOfType<AudioSource>();

        if(sources.Length > 0)
        {
            foreach (AudioSource s in sources)
            {
                if(s.gameObject == this.gameObject)
                {
                    s.volume = musicVolume;
//                     if(!s.isPlaying)
//                     {
//                         s.Play();
//                     }
                }
                else
                {
                    s.volume = sfxVolume;
                }
            }
        }
    }

    IEnumerator LoadFirstLevel()
    {
        yield return new WaitForSeconds(levelLoadDelay);

        Application.LoadLevel("RobbinRobin_TitleScreen");
    }

    void OnGUI()
    {
        if (Application.loadedLevelName == "RobbinRobin_Options")
        {
            GUILayout.BeginArea(MainArea);
            
            GUILayout.BeginVertical();
            GUILayout.Space(vSpace);
            
            GUILayout.BeginHorizontal();
            GUILayout.Space(hSpace);

            GUILayout.BeginArea(SliderArea);
            GUILayout.BeginVertical();
            
            GUI.skin.label.font = font;
            Color oldColor = GUI.color;

            GUILayout.BeginHorizontal();
            GUI.color = color;
            GUILayout.Label("SFX Volume   ");
            GUI.color = oldColor;
            sfxVolume = GUILayout.HorizontalSlider(sfxVolume, 0.0f, 1.0f);
            GUILayout.EndHorizontal();
            
            GUILayout.Space(verticalSpace);

            GUILayout.BeginHorizontal();
            GUI.color = color;
            GUILayout.Label("Music Volume");
            GUI.color = oldColor;
            musicVolume = GUILayout.HorizontalSlider(musicVolume, 0.0f, 0.5f);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.EndArea();

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }
    }

    private void ProcessDone()
    {
        PlayerPrefs.SetFloat("SoundEffects", sfxVolume);
        PlayerPrefs.SetFloat("MusicEffects", musicVolume);

        StartCoroutine("LoadMainMenu");
    }

    private void ProcessReset()
    {
        PlayerPrefs.SetFloat("SoundEffects", sfxVolume);
        PlayerPrefs.SetFloat("MusicEffects", musicVolume);

        StartCoroutine("LoadMainMenu");
    }

    IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(levelLoadDelay);

        Application.LoadLevel("RobbinRobin_MainMenu");
    }

    private void PlayMusic()
    {
        for (int index = 0; index < GameLevels.Length; index++)
        {
            if (Application.loadedLevelName == GameLevels[index])
            {
                musicSource.clip = InGameMusic;
                if (!musicSource.isPlaying)
                {
                    musicSource.volume = musicVolume;
                    musicSource.Play();
                    musicSource.loop = true;
                }
                return;
            }
        }

        for (int index = 0; index < MenuLevels.Length; index++)
        {
            if (Application.loadedLevelName == MenuLevels[index])
            {
                musicSource.clip = MenuMusic;
                if (!musicSource.isPlaying)
                {
                    musicSource.volume = musicVolume;
                    musicSource.Play();
                    musicSource.loop = true;
                }
                return;
            }
        }

        for (int index = 0; index < StoryLevels.Length; index++)
        {
            if (Application.loadedLevelName == StoryLevels[index])
            {
                musicSource.clip = StoryMusic;
                if (!musicSource.isPlaying)
                {
                    musicSource.volume = musicVolume;
                    musicSource.Play();
                    musicSource.loop = true;
                }
                return;
            }
        }
    }

}
