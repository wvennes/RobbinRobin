using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

    public string m_sceneToLoad = "RobbinRobin_MainMenu";
    public float m_fadeSpeed = 1f;
    public GUITexture m_guildhallSplash;
    public GUITexture m_teamLogoSplash;
    public GameObject m_blackScreen;

    private GUITexture m_fadeRenderer;
    private bool m_isFadingIn;
    private bool m_isFadingOut;
    private bool m_hasShownBothScreens;

	//---------------------------------------------------------------------------
	void Start ()
    {
        m_isFadingIn = true;
        m_isFadingOut = false;
        m_hasShownBothScreens = false;
        m_fadeRenderer = m_blackScreen.GetComponent<GUITexture>();
        m_guildhallSplash.enabled = true;
        m_teamLogoSplash.enabled = false;
	}
	
    //---------------------------------------------------------------------------
	void Update () 
    {
	
	}


    //---------------------------------------------------------------------------
    void FixedUpdate()
    {
        if ( m_isFadingIn )
        {
            FadeInBackground();
        }
        else if ( m_isFadingOut )
        {
            FadeOutBackground();
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
            m_isFadingOut = true;
        }
    }


    //---------------------------------------------------------------------------
    void FadeOutBackground()
    {
        m_fadeRenderer.color = Color.Lerp(m_fadeRenderer.color, Color.black, m_fadeSpeed * Time.deltaTime);

        if (m_fadeRenderer.color.a >= 0.9f)
        {
            m_fadeRenderer.color = Color.black;
            m_guildhallSplash.enabled = false;
            m_teamLogoSplash.enabled = true;
            m_isFadingOut = false;
            m_isFadingIn = true;

            if ( m_hasShownBothScreens )
            {
                LoadNextScreen();
            }

            m_hasShownBothScreens = true;
        }
    }


    //---------------------------------------------------------------------------
    void LoadNextScreen()
    {
        Application.LoadLevel( m_sceneToLoad );
    }
}
