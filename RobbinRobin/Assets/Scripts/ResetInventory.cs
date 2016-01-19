//Reset Inventory
//
//Resets player inventory as it is persistent
//across runs of the game
//
//by Michael McCoy
//--------------------------------------------
using UnityEngine;
using System.Collections;

public class ResetInventory : MonoBehaviour 
{
	// Performed before Start() functions
	void OnEnable () 
	{
		//Debug.Log ("Reseting Player Prefs");

		//Resets all PlayerPrefs as they are
		//persistent across runs
		PlayerPrefs.SetFloat("Health",0.0f);
		PlayerPrefs.SetFloat("Lives",0.0f);
		PlayerPrefs.SetFloat("Inv1Value",0.0f);
		PlayerPrefs.SetFloat("Inv2Value",0.0f);
		PlayerPrefs.SetFloat("Inv3Value",0.0f);
	}
}
