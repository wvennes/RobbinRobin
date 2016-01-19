using UnityEngine;
using System.Collections;

/*
 * Code by: Nathan Bowden
 * 10/9/2014
 * 
 * 
 */





public class Pig_Vision : MonoBehaviour {
	//Target object that we want to determine if the pig sees.
	public GameObject targetObject;

	public bool isAlert = false;






	//Run when the player collides with this trigger
	void OnTriggerEnter2D(Collider2D collider)
	{
		
		if (collider.gameObject.tag == "Player") 
		{
			//Get Player_Stealthed
			Player_Stealthed playerStealthed = targetObject.GetComponent<Player_Stealthed>();
			
			
			if(playerStealthed.isLit == true)
			{
				//the pig sees the player 
				//go into alert!
				isAlert = true;

			}


		}
	}

	//Activate when object exits collider
	void OnTriggerExit2D(Collider2D collider)
	{
		
		
		
		if (collider.gameObject.tag == "Player") 
		{
			//wait 3 seconds and switch out of alert mode


			isAlert = false;
		}
	}





}
