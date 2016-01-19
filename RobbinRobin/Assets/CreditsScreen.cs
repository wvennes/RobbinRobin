using UnityEngine;
using System.Collections;

public class CreditsScreen : MonoBehaviour
{

    public string m_sceneToLoad;
    public GUIText[] m_credits;
    private int m_currentTextNumber = 0;
    private GUIText m_currentGUIText;
    private bool m_hasReachedEnd = false;

    // Use this for initialization
    void Start()
    {
        foreach ( GUIText current in m_credits )
        {
            current.enabled = false;
        }

        m_currentGUIText = m_credits[m_currentTextNumber];
        m_currentGUIText.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_hasReachedEnd || Input.GetButtonDown("Fire3"))
        {
            LoadNextScreen();
        }
        else if ( Input.GetButtonDown("Fire2") )
        {
            LoadNextText();
        }
    }


    //---------------------------------------------------------------------------
    void LoadNextText()
    {
        ++m_currentTextNumber;

        if ( m_currentTextNumber >= m_credits.Length )
        {
            m_hasReachedEnd = true;
        }
        else
        {
            m_currentGUIText.enabled = false;
            m_currentGUIText = m_credits[m_currentTextNumber];
            m_currentGUIText.enabled = true;
        }
    }


    //---------------------------------------------------------------------------
    void LoadNextScreen()
    {
        Application.LoadLevel(m_sceneToLoad);
    }
}
