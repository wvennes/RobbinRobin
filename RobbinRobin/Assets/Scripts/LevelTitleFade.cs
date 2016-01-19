using UnityEngine;
using System.Collections;

public class LevelTitleFade : MonoBehaviour
{

    public string m_sceneToLoad = "RobbinRobin_Level1";
    public float m_fadeSpeed = 1f;
    public GUIText m_levelTitle;

    private bool m_isFadingIn;
    private bool m_isFadingOut;

    //---------------------------------------------------------------------------
    void Start()
    {
        m_isFadingIn = true;
        m_isFadingOut = false;
        m_levelTitle.color = Color.clear;
    }

    //---------------------------------------------------------------------------
    void Update()
    {

    }


    //---------------------------------------------------------------------------
    void FixedUpdate()
    {
        if (m_isFadingIn)
        {
            FadeInText();
        }
        else if (m_isFadingOut)
        {
            FadeOutText();
        }
    }


    //---------------------------------------------------------------------------
    void FadeInText()
    {
        m_levelTitle.color = Color.Lerp(m_levelTitle.color, Color.white, m_fadeSpeed * Time.deltaTime);

        if (m_levelTitle.color.a >= 0.9f)
        {
            m_levelTitle.color = Color.white;
            m_isFadingIn = false;
            m_isFadingOut = true;
        }
    }


    //---------------------------------------------------------------------------
    void FadeOutText()
    {
        m_levelTitle.color = Color.Lerp(m_levelTitle.color, Color.clear, m_fadeSpeed * Time.deltaTime);

        if (m_levelTitle.color.a <= 0.1f)
        {
            m_levelTitle.color = Color.clear;
            m_isFadingOut = false;
            
            LoadNextScreen();
        }
    }


    //---------------------------------------------------------------------------
    void LoadNextScreen()
    {
        Application.LoadLevel(m_sceneToLoad);
    }
}
