//End of Level Controller
//
//Saves player data and loads the next level
//
//		Must be placed on a trigger
//		Make sure blackTextureObject covers
//		viewable world in game
//
//by Morgan Wagnon and modified
//by Michael McCoy
//--------------------------------------------

using UnityEngine;
using System.Collections;

public class EndOfLevel : MonoBehaviour
{   
	//Set this number lower if you want a slower fade, and higher if you want a faster fade
	public float fadeSpeed = 1f;

	//This is a reference to the object with just a sprite renderer and transform that is parented to the camera
	public GameObject blackTextureObject;

	//Reset Player Health on next level?
	public bool resetHealth = true;

	//Name of the next level
	public string nextLevelName = "none";

	//Exit to desktop flag
	public bool exitToDesktop = false;
	
	//Creates a variable that holds a reference to the sprite renderer of the plain black texture
	private SpriteRenderer faderender;
	
	//Checks if the script should be running the fade IN function
	private bool isFadingIn = true;
	
	//Checks if the script should be running the fade OUT function
	private bool isFadingOut = false;

	//The desired position and scale of the blackTextureObject
	private Vector3 desiredPosition, desiredScale;

	//Variable that points at the player
	private GameObject player;

	//Pointer to Camera object
	Vector3 cameraPosition;

	//This runs whenever the object that this script is attached to is loaded into the scene
	void Start()
	{
		//Find the sprite renderer component on the blackTextureObject that the designer assigns in the editor
		//and put a short-cut to the renderer with the name "faderender"
		faderender = blackTextureObject.GetComponent<SpriteRenderer>();

		//Make sure the faderender exists
		if(faderender != null)
		{
			//Make sure that the color of the renderer of the texture is completely black
			faderender.color = Color.black;
		}
		else //Give error message
		{
			//Debug.Log("End of Level Script: Cannot find blackTextureObject!");
		}

		//Set the desired scale for the blackTextureObject which is coving the entire screen
		desiredScale.x = (float) (8000 / faderender.sprite.texture.width);
		desiredScale.y = (float) (8000 / faderender.sprite.texture.height);
		desiredScale.z = 1.0f;
		
		//Set the black/fading texture scale to the desired scale
		blackTextureObject.transform.localScale = desiredScale;

		//Find the player and point this variable at them
		player = GameObject.FindWithTag ("Player");
	}
	
	//This runs after a fixed framerate time
	void FixedUpdate()
	{
		//Make sure the faderender exists
		if(faderender != null)
		{
			//If we are fading in...
			if (isFadingIn == true) 
			{
				if(faderender != null)
					//Run the FadeIn function
					FadeIn ();

			}
			
			//If we are fading out...
			if (isFadingOut == true)
			{
				//Rune the FadeOut function
				FadeOut ();
			}	
		}
	}

	//When the end of level trigger is entered
    void OnTriggerEnter2D(Collider2D collider)
	{
		//If the thing that triggered it is the player and the player isn't currently dead
        if (collider.gameObject.tag == "Player" && (collider.GetComponent<HealthController>().health >= 1.0f))
        {
			if(faderender != null)
			{
				//If fading in has not completed before the player runs through the end of level trigger...
				if (isFadingIn == true)
				{
					//Set fade color to transparent
					faderender.color = Color.clear;

					//Set fading in to false
					//This prevents fading in and fading out from happening at the same time
					isFadingIn = false;
				}
				//Moves the Black Fading Texture to the cameras position and rescales it
				MoveToCamera();

				//Set Player to invulnerable
				player.GetComponent<HealthController>().isInvulnerable = true;

				//Run the FadeOut function starting next frame
				isFadingOut = true;
			}
			else
			{
				//Make sure designer entered a level name
				if(nextLevelName != "none")
				{
					//Load the next level
					Application.LoadLevel(nextLevelName);
				}
				else
				{
					//Debug.Log("End of Level Script: No level to load!");
				}
			}
        }
    }

	//Only runs when isFadingIn is true
	void FadeIn()
	{
		//Take the current color of the texture and set it equal to the difference between the current color and completely clear, 
		//and change the current color by that much difference over the fadeSpeed amount of time per frame
		faderender.color = Color.Lerp (faderender.color, Color.clear, fadeSpeed * Time.deltaTime);

		//Moves the Black Fading Texture to the cameras position and rescales it
		MoveToCamera();

		//When the color gets close enough, go ahead and set it to clear so we can save processor power
		if (faderender.color.a <= 0.1f) 
		{
			//Set fade color to transparent
			faderender.color = Color.clear;
			
			//We're done fading in
			isFadingIn = false;
		}
		
	}
	
	//Only runs when isFadingIn is true
	void FadeOut() 
	{
		//Take the current color of the texture and set it equal to the difference between the current color and completely black, 
		//and change the current color by that much difference over the fadeSpeed amount of time per frame
		faderender.color = Color.Lerp(faderender.color, Color.black, fadeSpeed * Time.deltaTime);
		
		//When the color gets close enough, go ahead and set it to black so we can save processor power
		if (faderender.color.a >= 0.9f) 
		{
			//Set fade color to black
			faderender.color = Color.black;

			//We're done fading out
			isFadingOut = false;

			//Save the players lives and health
			if(!resetHealth) PlayerPrefs.SetFloat("Health",player.GetComponent<HealthController>().health);
			PlayerPrefs.SetFloat("Lives",player.GetComponent<HealthController>().lives);
			
			//If the player has inventory keep it
			if(player.GetComponent<InventoryController>() != null)
			{
				PlayerPrefs.SetFloat("Inv1Value",player.GetComponent<InventoryController>().Pickup1InitialValue);
				PlayerPrefs.SetFloat("Inv2Value",player.GetComponent<InventoryController>().Pickup2InitialValue);
				PlayerPrefs.SetFloat("Inv3Value",player.GetComponent<InventoryController>().Pickup3InitialValue);
			}

			//Check the exitToDesktop flag
			if(exitToDesktop) //Exit to the desktop
			{
				//Debug.Log("Exiting to Desktop");
				Application.Quit();
			}
			else
			{
				//Make sure designer entered a level name
				if(nextLevelName != "none")
				{
					//Load the next level
					Application.LoadLevel(nextLevelName);
				}
				else
				{
					//Debug.Log("End of Level Script: No level to load!");
				}
			}
			//Stop fading out process
			isFadingOut = false;
		}
	}

	//Moves the Black Fading Texture to the cameras position and rescales it
	void  MoveToCamera()
	{
		//Make sure MainCamera exits
		if(GameObject.FindWithTag ("MainCamera") != null)
		{
			cameraPosition = GameObject.FindWithTag ("MainCamera").transform.position;
			
			//Set the desired position for the blackTextureObject which is the exact position of the camera
			//and a Z 5 units in front of the cameras Z
			desiredPosition = new Vector3(cameraPosition.x,
			                              cameraPosition.y,
			                              cameraPosition.z + 5.0f);
			
			//Move the black/fading texture to the desired position
			blackTextureObject.transform.position = desiredPosition;
		}
		else //Give error message
		{
			//Debug.Log("End of Level Script: Cannot find MainCamera game object");
		}
	}
}