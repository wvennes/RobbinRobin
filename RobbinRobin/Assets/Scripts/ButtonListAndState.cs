using UnityEngine;
using System.Collections;

public class ButtonListAndState : MonoBehaviour 
{
    public enum ButtonActions
    {
        STARTGAME,
        LEVELSELECT,
        OPTIONS,
        CONTROLS,
        CREDITS,
        EXIT,
    };


    public bool isHighlighted;

    public ButtonListAndState previousButton;
    public ButtonListAndState nextButton;

    public GUITexture previousButtonTexture;
    public GUITexture nextButtonTexture;
   
    public ButtonActions actionToPerform;
	
    // Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}

}
