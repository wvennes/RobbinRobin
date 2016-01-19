using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {

    public string m_sceneToLoad = "RobbinRobin_MainMenu";
    public float m_fadeSpeed = 1f;
    public float m_buttonPulseSpeed = 1f;
    public GameObject m_blackScreenObject;
    public GUITexture m_titleTexture;
    public GUITexture m_pressStartTexture;
    public GUIText m_pressStartText;
    private GUITexture m_fadeRenderer;
    private bool m_isFadingIn;
    private bool m_isFadingOut;
    private bool m_isPulseFadingIn;
    private bool m_isNextScreen;
    //private float m_maxScale = 0.25f;

	//---------------------------------------------------------------------------
	void Start ()
    {
        m_fadeRenderer = m_blackScreenObject.GetComponent<GUITexture>();
        //m_titleTexture.color = Color.clear;
        m_pressStartTexture.color = Color.clear;
        m_pressStartText.color = Color.clear;
        m_isFadingIn = true;
//         m_pressStartTexture.enabled = false;
//         m_pressStartText.enabled = false;
        m_isPulseFadingIn = true;
        m_isNextScreen = false;
	}

	
    //---------------------------------------------------------------------------
	void Update () 
    {
	    if ( !m_isFadingIn && Input.GetButtonDown("Fire2") )
        {
            m_isNextScreen = true;
        }
	}


    //---------------------------------------------------------------------------
    void FixedUpdate()
    {
        // fade in the screen
        if ( m_isFadingIn && !m_isNextScreen )
        {
            FadeInBackground();
        }
        else if ( !m_isFadingIn && m_isNextScreen )
        {
            FadeOutToMainMenu();
        }
        else if ( !m_isFadingIn && !m_isNextScreen )
        {
            // pulse the button
            PulseButton();
        }
    }


    //---------------------------------------------------------------------------
    void PulseButton()
    {
        if ( m_isPulseFadingIn )
        {
            m_pressStartTexture.color = Color.Lerp(m_pressStartTexture.color, Color.gray, m_buttonPulseSpeed * Time.deltaTime);
            m_pressStartText.color = Color.Lerp(m_pressStartText.color, Color.black, m_buttonPulseSpeed * Time.deltaTime);

            if (m_pressStartTexture.color.a >= 0.9f)
            {
                m_pressStartTexture.color = Color.gray;
                m_pressStartText.color = Color.black;
                m_isPulseFadingIn = false;
            }
        }
        else
        {
            m_pressStartTexture.color = Color.Lerp(m_pressStartTexture.color, Color.clear, m_buttonPulseSpeed * Time.deltaTime);
            m_pressStartText.color = Color.Lerp(m_pressStartText.color, Color.clear, m_buttonPulseSpeed * Time.deltaTime);

            if (m_pressStartTexture.color.a <= 0.5f)
            {
//                 m_pressStartTexture.color = Color.gray;
//                 m_pressStartText.color = Color.black;
                m_isPulseFadingIn = true;
            }
        }
    }


    //---------------------------------------------------------------------------
    void FadeInBackground()
    {
        m_fadeRenderer.color = Color.Lerp(m_fadeRenderer.color, Color.clear, m_fadeSpeed * Time.deltaTime);

        if (m_fadeRenderer.color.a <= 0.05f)
        {
            m_fadeRenderer.color = Color.clear;
            m_isFadingIn = false;
            //m_isPulseFadingIn = true;
            //m_pressStartTexture.enabled = true;
        }
    }


    //---------------------------------------------------------------------------
    void FadeOutToMainMenu()
    {
        m_titleTexture.color = Color.Lerp(m_titleTexture.color, Color.clear, m_fadeSpeed * Time.deltaTime);
        m_pressStartTexture.color = Color.Lerp(m_pressStartTexture.color, Color.clear, m_fadeSpeed * Time.deltaTime);
        m_pressStartText.color = Color.Lerp(m_pressStartText.color, Color.clear, m_fadeSpeed * Time.deltaTime);

        if (m_titleTexture.color.a <= 0.05f)
        {
            m_titleTexture.color = Color.clear;
            m_pressStartTexture.color = Color.clear;
            m_pressStartText.color = Color.clear;
            LoadNextScreen();
        }
    }


    //---------------------------------------------------------------------------
    void LoadNextScreen()
    {
        Application.LoadLevel(m_sceneToLoad);
    }
}
