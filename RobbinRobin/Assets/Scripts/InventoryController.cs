//Inventory Controller
//
//Handles all inventory related functionality
//as well as sending information to the HUD
//
//by Michael McCoy
//with help from Morgan Wagnon
//--------------------------------------------
using UnityEngine;
using System.Collections;

public class InventoryController : MonoBehaviour 
{
	//Variable that points at the Player
	GameObject player;

	//Create an enuration to specify initial move
	//direction
	public enum whatToModify
	{
		Health,
		Lives,
		Collectable
	}

	//Variables to display Health and Lives
	public GUIText HealthGUIText;
	public GUIText LivesGUIText;

	//As the editor/inspector does not handle structs
	//brute force is the only way to handle inventory
 	//Inventory item 1 varaibles
    public string PickupNameToDisplay;
	public string Pickup1Name;
	public GUIText Pickup1GUIText;
	public float Pickup1InitialValue;
    public float Pickup1MaxValue;
	public whatToModify Pickup1AttrToModify;
	public float Pickup1IncreaseAmt;

	//Inventory item 2 varaibles
	public string Pickup2Name;
	public GUIText Pickup2GUIText;
	public float Pickup2InitialValue;
	public whatToModify Pickup2AttrToModify;
	public float Pickup2IncreaseAmt;

	//Inventory item 3 varaibles
	public string Pickup3Name;
	public GUIText Pickup3GUIText;
	public float Pickup3InitialValue;
	public whatToModify Pickup3AttrToModify;
	public float Pickup3IncreaseAmt;

	//Pointer to the Player Controller
	HealthController healthController;

	// Use this for initialization
	void Start () 
	{
		//Find the player and point this variable
		//to them
		player = GameObject.FindWithTag ("Player");

		//Initialize a pointer the the player controller script
		healthController = player.GetComponent<HealthController> ();

		//If there are saved inventory values, use them
		//This data gets saved by EndOfLevel.cs
		if (PlayerPrefs.GetFloat ("Inv1Value") != 0.0f)
			Pickup1InitialValue = PlayerPrefs.GetFloat ("Inv1Value");
		if (PlayerPrefs.GetFloat ("Inv2Value") != 0.0f)
			Pickup2InitialValue = PlayerPrefs.GetFloat ("Inv2Value");
		if (PlayerPrefs.GetFloat ("Inv3Value") != 0.0f)
			Pickup3InitialValue = PlayerPrefs.GetFloat ("Inv3Value");
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Send Health and Lives info to the screen
		if(HealthGUIText != null)
			HealthGUIText.text = "Health: " + (int) healthController.health;
		if(LivesGUIText != null)
			LivesGUIText.text = "Lives: " + (int) healthController.lives;

		//Send inventory info to the screen through GUIText Obects
		if(Pickup1GUIText != null)
			Pickup1GUIText.text = PickupNameToDisplay + (int) Pickup1InitialValue + " / " + (int) Pickup1MaxValue;
		if(Pickup2GUIText != null)
			Pickup2GUIText.text = Pickup2Name + ": " + (int) Pickup2InitialValue;
		if(Pickup3GUIText != null)
			Pickup3GUIText.text = Pickup3Name + ": " + (int) Pickup3InitialValue;
	}

	//Called on collision with triggers
	void OnTriggerEnter2D(Collider2D otherCollider)
	{
		//Increase inventory/player stats if is pickup and has not been awarded yet
		if(otherCollider.gameObject.name == Pickup1Name)
		{
			if(!otherCollider.gameObject.GetComponent<PickupController>().awarded)
			{
				//Increase value
				//Pickup1InitialValue++;
				//If should increase Health do so
				if(Pickup1AttrToModify == whatToModify.Health) 
					healthController.health += Pickup1IncreaseAmt;
				//If should increase Lives do so
				if(Pickup1AttrToModify == whatToModify.Lives) 
					healthController.lives += Pickup1IncreaseAmt;
                //If a regular pickup of some kind, modify it based on the user's input
                if ((Pickup1AttrToModify != whatToModify.Health) && (Pickup1AttrToModify != whatToModify.Lives))
                    Pickup1InitialValue += Pickup1IncreaseAmt;

				//Debug.Log("Health = " + healthController.health);
			}
		}
		//Increase inventory/player stats if is pickup and has not been awarded yet
		if(otherCollider.gameObject.name == Pickup2Name)
		{
			if(!otherCollider.gameObject.GetComponent<PickupController>().awarded)
			{
				//Increase value
				//Pickup2InitialValue++;
				//If should increase Health do so
				if(Pickup2AttrToModify == whatToModify.Health) 
					healthController.health += Pickup2IncreaseAmt;
				//If should increase Lives do so
				if(Pickup2AttrToModify == whatToModify.Lives) 
					healthController.lives += Pickup2IncreaseAmt;
                //If a regular pickup of some kind, modify it based on the user's input
                if ((Pickup2AttrToModify != whatToModify.Health) && (Pickup2AttrToModify != whatToModify.Lives))
                    Pickup2InitialValue += Pickup2IncreaseAmt;
			}
		}
		//Increase inventory/player stats if is pickup and has not been awarded yet
		if(otherCollider.gameObject.name == Pickup3Name)
		{
			if(!otherCollider.gameObject.GetComponent<PickupController>().awarded)
			{
				//Increase value
				//Pickup3InitialValue++;
				//If should increase Health do so
				if(Pickup3AttrToModify == whatToModify.Health) 
					healthController.health += Pickup3IncreaseAmt;
				//If should increase Lives do so
				if(Pickup3AttrToModify == whatToModify.Lives) 
					healthController.lives += Pickup3IncreaseAmt;
                //If a regular pickup of some kind, modify it based on the user's input
                if ((Pickup3AttrToModify != whatToModify.Health) && (Pickup3AttrToModify != whatToModify.Lives))
                    Pickup3InitialValue += Pickup3IncreaseAmt;
			}
		}
		//If otherCollider is a pickup, tell it to destroy itself
		if(otherCollider.gameObject.name == Pickup1Name ||
		   otherCollider.gameObject.name == Pickup2Name ||
		   otherCollider.gameObject.name == Pickup3Name)
		{
			otherCollider.gameObject.GetComponent<PickupController>().awarded = true;
			otherCollider.gameObject.GetComponent<PickupController>().KillThySelf();
            SendMessage("PlaySound", "Food");
		}
	}

	//Script that actually plays sounds
	public void Play(AudioClip sound)
	{
		//If sound exists
		if(sound != null)
			// As it is not 3D audio clip, position doesn't matter.
			AudioSource.PlayClipAtPoint(sound, transform.position);

	}
}
