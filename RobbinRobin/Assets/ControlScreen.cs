using UnityEngine;
using System.Collections;

public class ControlScreen : MonoBehaviour {

    public string m_sceneToLoad;

	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if ( Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire3") )
        {
            LoadNextScreen();
        }
	}


    //---------------------------------------------------------------------------
    void LoadNextScreen()
    {
        Application.LoadLevel(m_sceneToLoad);
    }
}
