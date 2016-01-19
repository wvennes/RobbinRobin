//InvisibleObject
//
//Turns the Enabled flag on and off in the objects
//SpriteRenderer
//
//by James Bowling; modified by Michael McCoy
using UnityEngine;
using System.Collections;

public class InvisibleObject : MonoBehaviour 
{

	//Start active/enabled
	public bool startEnabled = false;

	//Pointer to the SpriteRender of the target object
	private SpriteRenderer mySprite;

	// Use this for initialization
	void Start ()
	{
		//Initializes the pointer to the SpriteRenderer
		mySprite = gameObject.GetComponent<SpriteRenderer> ();

        //Set the initial value of the SpriteRenderer
		//and the Collider2D component
		if (!startEnabled)
        {
            mySprite.enabled = false;
            this.collider2D.enabled = false;
        }
        else
        {
            mySprite.enabled = true;
            this.collider2D.enabled = true;
        }
	}
	
	// Update is called once per frame
	void Update ()
	{
		//If triggered and the SpriteRenderer is not enabled
		//enable the SpriteRenderer and Collider2D
		if (startEnabled && mySprite.enabled != true)
		{
			mySprite.enabled = true;
			this.collider2D.enabled = true;
		}

		//If turned off and the SpriteRenderer is enabled
		//disable the SpriteRenderer and Collider2D
		if (!startEnabled && mySprite.enabled == true)
		{
			mySprite.enabled = false;
			this.collider2D.enabled = false;
		}
	}
}
