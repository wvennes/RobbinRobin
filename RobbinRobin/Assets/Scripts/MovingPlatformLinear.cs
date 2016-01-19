//Moving Platform Linear
//
//Allows a platform to move from start to
//user defined endpoint and then reverses
//direction
//
//RigidBody2D (Needed settings)
//		Fixed Angle = true
//		Is Kinematic = true
//		Interpolate = Interpolate
//
//by Eric Douglas C20 Programmer
//with modification by Morgan Wagnon and James Bowling
//--------------------------------------------
using UnityEngine;
using System.Collections;

public class MovingPlatformLinear : MonoBehaviour 
{
    //Boolean that checks whether the platform is activated; if it is not activated, it will not move
	public bool startEnabled = true;

    //A position in the world called "EndLocation" that is able to be assigned in the editor
	public Transform endLocation;

    //Create a variable called "StartPosition" of type Vector3; this can hold the positions of objects
	Vector3 startPosition;

    //How fast the platforms move
	public float speed;

    //How long to wait at each end of the platform's path
	public float secondsToWait;

    //Checks if the platform is moving forward or backward
	bool forward = true;

    //Checks if the platform is waiting at its end point
	bool waiting = false;

	//This function runs only once when the object that has this script assigned to it is loaded into the scene
	void Start () 
	{
        //The variable StartPosition (which is of Vector3 type), is set to the current position of the object that has this script on it
		//startPosition = transform.position;

		//This new Vector3 checks the centering of the object, this addresses an issue with the movement to the stop position being based on
		//the original position of the empty game object that was created around it
		Vector3 centerPosition = new Vector3((gameObject.transform.position.x + gameObject.GetComponent<BoxCollider2D> ().center.x),
		                                    (gameObject.transform.position.y + gameObject.GetComponent<BoxCollider2D> ().center.y),gameObject.transform.position.z);

		startPosition = centerPosition;

	}
	
    //Called every fixed framerate, because framerate in Update() can vary, making movements jerky
	void FixedUpdate()
	{
        //If waiting is false and activated is true, so the platform is not waiting at the end of its path, and it is activated
		if (!waiting && startEnabled)
		{
            //If the platform is moving forward...
			if (forward) 
            {
                //Create a variable called "difference" of type "Vector 2" and set it equal to the current position of the platform
                //minus the position of the EndLocation variable which is set in the editor
				Vector2 difference = transform.position - endLocation.position;

                //If the platform has reached its end location, stop
				if (difference.sqrMagnitude < 0.001f)
				{
                    //The platform is no longer going forward, and will go backward after its waiting period is finished
					forward = false;

                    //Run the function that makes the platform hold its position before reversing direction
					StartCoroutine ("StopWaiting");
				}

                //If the platform has not reached its end location, continue to move
				else 
                {
                    //Create a variable called "difference" of type "Vector 2" and set it equal to the position of the EndLocation variable which is set in the editor
                    //minus the current position of the platform
					Vector2 direction = (endLocation.position - transform.position);

                    //Shrink the length of the Vector in "direction" to one
					direction.Normalize();

                    //Move the platform equal to the Vector "direction" multiplied by the speed set by the designer
					rigidbody2D.velocity =  direction * speed;
				}
				
			}

            //If the platform is moving backward...
			else 
            {
				//Create a variable called "difference" of type "Vector 2" and set it euqal to the current position of the platform
                //minus the original starting position of the platform; in other words, how far it is away from start
				Vector2 difference = transform.position - startPosition;

                //If the platform has gotten back to the beginning point and stopped
				if (difference.sqrMagnitude < 0.001f)
				{
                    //The platform has returned to its start position and is now going to move in the original direction after its waiting period is finished
					forward = true;

                    //Run the function that makes the platform hold its position before reversing direction
					StartCoroutine ("StopWaiting");
				}

                //If the platform is heading back to the start position but has not gotten there yet, keep moving
				else 
                {	
                    //Determine how far away the platform is from its starting position
					Vector2 direction = (startPosition - transform.position);

                    //Shrink the length of the Vector in "direction" to one
					direction.Normalize();

                    //Move the platform equal to the Vector "direction" multiplied by the speed set by the designer
					rigidbody2D.velocity =  direction * speed;
				}
			}
		}

        //If it is not moving forward OR backward, stop it
		else 
        {
			rigidbody2D.velocity = new Vector2(0, 0);
		}		
	}
	
    //When the moving platform collides with something...
	void OnCollisionStay2D(Collision2D collision) 
	{
		//Modify the players X velocity as long as he's colliding
		//with a platform
		if (collision.gameObject.tag == "Player")
		{
			collision.gameObject.GetComponent<PlayerController> ().
				platformSpeedX = gameObject.rigidbody2D.velocity.x;
		}
	}

	//When the moving platform is no longer colliding with an object
	void OnCollisionExit2D(Collision2D collision)
	{
		//Remove the platforms velocity from the player when he's
		//no longer colliding with it
		if (collision.gameObject.tag == "Player")
		{
			collision.gameObject.GetComponent<PlayerController> ().
				platformSpeedX = 0.0f;
		}
	}

    //Runs every time the platform reaches the end of its path and needs to wait for a certain amount of time before reversing direction
	public IEnumerator StopWaiting() 
	{
        //Tell the FixedUpdate() not to run any movement on the platform because it is waiting
		waiting = true;

        //Pauses the coroutine for the given amount of seconds in "SecondsToWait"
		yield return new WaitForSeconds (secondsToWait);

        //After the platform waits for the correct amount of time, tell FixedUpdate() that the platform is no longer waiting
		waiting = false;
	}
}
