using UnityEngine;
using System.Collections;

public class StoryScreen : MonoBehaviour 
{
    public string m_sceneToLoad;
    public GUIText[] m_dialogue;
    int m_currentDialogue = 0;
    public float m_fadeSpeed = 1f;
    public float m_textFadeSpeed = 1f;
    public GameObject m_blackTextureObject;
    public GameObject m_textBackground;
    private GUITexture m_fadeRenderer;
    private GUITexture m_textBackgroundRenderer;
    private GUIText m_textRenderer;
    private bool m_isFadingIn = true;
    private bool m_isFadingOut = false;
    private bool m_isTextBackgroundFadingIn = false;
    private bool m_isTextBackgroundFadingOut = false;
    private bool m_isTextFadingIn = false;
    private bool m_isTextFadingOut = false;
    //private Color m_textBackgroundColor;
    private Vector3 m_fadeScale;
    private Vector3 m_fadePos;

	// Use this for initialization
	void Start () 
    {
	    for ( int i = 0; i < m_dialogue.Length; ++i )
        {
            m_dialogue[i].enabled = false;
        }

        //m_dialogue[m_currentDialogue].enabled = true;

        m_fadeRenderer = m_blackTextureObject.GetComponent<GUITexture>();
        m_textRenderer = m_dialogue[m_currentDialogue].GetComponent<GUIText>();
        m_textBackgroundRenderer = m_textBackground.GetComponent<GUITexture>();
        //m_textBackgroundColor = m_textBackgroundRenderer.color;

//         m_fadeScale.x = (float)(8000 / m_fadeRenderer.sprite.texture.width);
//         m_fadeScale.y = (float)(8000 / m_fadeRenderer.sprite.texture.height);
//         m_fadeScale.z = 1.0f;

        //m_blackTextureObject.transform.localScale = m_fadeScale;
        m_textBackgroundRenderer.color = Color.clear;
        m_textBackgroundRenderer.enabled = true;
        m_fadeRenderer.color = Color.black;
        m_fadeRenderer.enabled = true;
        m_textRenderer.color = Color.clear;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if ( !m_isFadingIn && !m_isFadingOut && !m_isTextFadingIn && !m_isTextFadingOut && !m_isTextBackgroundFadingIn && !m_isTextBackgroundFadingOut )
        {
            if (Input.GetButtonDown("Fire2"))
            {
                //++m_currentDialogue;
                m_isTextFadingOut = true;
            }

            if (m_currentDialogue >= m_dialogue.Length || Input.GetButtonDown("Fire3"))
            {
                m_isTextBackgroundFadingOut = true;
                //Application.LoadLevel(m_sceneToLoad);
            }
//             else
//             {
// //                 m_dialogue[m_currentDialogue - 1].enabled = false;
// //                 m_dialogue[m_currentDialogue].enabled = true;
//             }
        }
	}


    //---------------------------------------------------------------------------
    void FixedUpdate()
    {
        if (m_isFadingIn)
        {
            FadeInBackground();
        }
        else if (m_isFadingOut)
        {
            FadeOutBackground();
        }
        else if ( m_isTextBackgroundFadingIn )
        {
            FadeInTextBackground();
        }
        else if ( m_isTextBackgroundFadingOut )
        {
            //FadeOutText();
            if ( m_textRenderer.enabled )
            {
                FadeOutText();
            }
            FadeOutTextBackground();
        }
        else if ( m_isTextFadingIn )
        {
            FadeInText();
        }
        else if ( m_isTextFadingOut && m_currentDialogue < m_dialogue.Length )
        {
            FadeOutText();
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
            m_isTextBackgroundFadingIn = true;
        }
    }


    //---------------------------------------------------------------------------
    void FadeInTextBackground()
    {
        m_textBackgroundRenderer.color = Color.Lerp(m_textBackgroundRenderer.color, Color.gray, m_textFadeSpeed * Time.deltaTime);

        if (m_textBackgroundRenderer.color.a >= 0.9f)
        {
            m_textBackgroundRenderer.color = Color.gray;
            m_isTextBackgroundFadingIn = false;
            m_isTextFadingIn = true;

            m_textRenderer = m_dialogue[m_currentDialogue].GetComponent<GUIText>();
            m_dialogue[m_currentDialogue].enabled = true;
            //m_dialogue[m_currentDialogue].enabled = false;
        }
    }


    //---------------------------------------------------------------------------
    void FadeOutTextBackground()
    {
        m_textBackgroundRenderer.color = Color.Lerp(m_textBackgroundRenderer.color, Color.clear, m_textFadeSpeed * Time.deltaTime);

        if (m_textBackgroundRenderer.color.a <= 0.1f)
        {
            m_textBackgroundRenderer.color = Color.clear;
            m_isTextBackgroundFadingOut = false;
            m_isFadingOut = true;
            //m_dialogue[m_currentDialogue].enabled = false;
        }
    }


    //---------------------------------------------------------------------------
    void FadeInText()
    {
        m_textRenderer.color = Color.Lerp(m_textRenderer.color, Color.black, m_textFadeSpeed * Time.deltaTime);

        if ( m_textRenderer.color.a >= 0.9f )
        {
            m_textRenderer.color = Color.black;
            m_isTextFadingIn = false;
            //m_dialogue[m_currentDialogue].enabled = false;
        }
    }


    //---------------------------------------------------------------------------
    void FadeOutBackground()
    {
        m_fadeRenderer.color = Color.Lerp(m_fadeRenderer.color, Color.black, m_fadeSpeed * Time.deltaTime);
        
        if ( m_fadeRenderer.color.a >= 0.9f )
        {
            m_fadeRenderer.color = Color.black;
            m_isFadingOut = false;

            LoadNextLevel();
        }
    }


    //---------------------------------------------------------------------------
    void FadeOutText()
    {
        m_textRenderer.color = Color.Lerp(m_textRenderer.color, Color.clear, m_textFadeSpeed * Time.deltaTime);

        if ( m_textRenderer.color.a <= 0.1f )
        {
            m_textRenderer.color = Color.clear;
            m_dialogue[m_currentDialogue].enabled = false;
            m_isTextFadingOut = false;
            ++m_currentDialogue;

            if ( m_currentDialogue >= m_dialogue.Length )
            {
                m_isTextBackgroundFadingOut = true;
            }
            else
            {
                m_textRenderer = m_dialogue[m_currentDialogue].GetComponent<GUIText>();
                m_textRenderer.color = Color.clear;
                m_dialogue[m_currentDialogue].enabled = true;
                m_isTextFadingIn = true;
            }
        }
    }


    //---------------------------------------------------------------------------
    void LoadNextLevel()
    {
        Application.LoadLevel(m_sceneToLoad);
    }
}
