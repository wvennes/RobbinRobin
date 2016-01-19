//Camera Controller
//
//Simple controller that forcesthe camera
//to follow the player
//
//by Michael McCoy with code from tutorials
//-------------------------------------------------
using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	//Variable that points at the player
	private GameObject player;

	//Variable that points to the PlayerController
	private PlayerController playerController;

	//Always lock to players Y position
	public bool alwaysFollowOnY = true;

	//How long it takes to zero to players location
	//		Larger numbers are slower
	public int followLag = 40;

	//X and Y camera offsets
	public float xOffset = 0.0f;
	public float yOffset = 1.0f;

	// Use this for initialization
	void Start () 
	{
		//Find the player and point this variable
		//to them
		player = GameObject.FindWithTag ("Player");

		//Get a pointer to the players PlayerController
		playerController = player.GetComponent<PlayerController>();

		//Set the lower limit on follow lag amount
		if (followLag < 1)followLag = 1;
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Calculate the distance on the x and y between camera and player + offsets
		float xDistance = this.transform.position.x - (player.transform.position.x + xOffset);
		float yDistance = this.transform.position.y - (player.transform.position.y + yOffset);

		//Move the camera to the players x,y coordinates
		if(playerController.CameraFollow)
		{
			//Move the camera toward the players x,y coordinates
			this.transform.position = new Vector3 (this.transform.position.x - xDistance / followLag,
			                                       this.transform.position.y - yDistance / followLag,
			                                       this.transform.position.z);			
		}
		else if (alwaysFollowOnY)
		{
			//Move the camera toward the players x,y coordinates
			this.transform.position = new Vector3 (this.transform.position.x,
			                                       this.transform.position.y - yDistance / followLag,
			                                       this.transform.position.z);	

		}
	}
}
