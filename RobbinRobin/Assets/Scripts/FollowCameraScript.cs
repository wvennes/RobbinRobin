//Background Follow Camera
//
//Forces the attached object to follow the camera on x,y
//
//by Michael McCoy
//--------------------------------------------
using UnityEngine;
using System.Collections;

public class FollowCameraScript : MonoBehaviour 
{
	//Pointer to the Camera
	public GameObject cameraObject;

	//Pointer to Player
	GameObject player;

	//Follow Player?
	public bool followPlayerNotCamera = false;

	//Follow on the Y axis?
	public bool followOnY = false;

	//Follow on the X axis?
	public bool followOnX = true;

	//The desired position of the object
	private Vector3 desiredPosition;

	// Use this for initialization
	void Start () 
	{
		//Find the player and point this variable
		//to them
		player = GameObject.FindWithTag ("Player");
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Set the default position for the object
		desiredPosition = this.transform.position;

		//If not following player
		if(!followPlayerNotCamera)
		{
			//If the camera object exists
			if(cameraObject != null)
			{
				//If follow X
				if(followOnX)
					desiredPosition.x = cameraObject.transform.position.x;

				//If follow Y
				if(followOnY)
					desiredPosition.y = cameraObject.transform.position.y;

				//Change this objects position to  the desired position
				this.transform.position = desiredPosition;
			}
			else //Send error message
			{
				//Debug.Log ("Follow Camera Script: Cannot find camera object!");
			}
		}
		else //Follow the player
		{
			//If the player exits
			if(player != null)
			{
				//If follow X
				if(followOnX)
					desiredPosition.x = player.transform.position.x;
				
				//If follow Y
				if(followOnY)
					desiredPosition.y = player.transform.position.y;

				//Change this objects position to the desired position
				this.transform.position = desiredPosition;
			}
			else //Send error message
			{
				//Debug.Log ("Follow Camera Script: Cannot find player object!");
			}
		}
	}
}
