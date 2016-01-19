using UnityEngine;
using System.Collections;

public class Portcullis : MonoBehaviour {

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



    //Checks if the platform is waiting at its end point
    bool waiting = false;

    public bool isRaising;
    public bool isFinished;
    public float distanceToMove;
    public bool startsRaised;

    //This function runs only once when the object that has this script assigned to it is loaded into the scene
    void Start()
    {
        //The variable StartPosition (which is of Vector3 type), is set to the current position of the object that has this script on it
        //startPosition = transform.position;

        //This new Vector3 checks the centering of the object, this addresses an issue with the movement to the stop position being based on
        //the original position of the empty game object that was created around it
        Vector3 centerPosition = new Vector3((gameObject.transform.position.x + gameObject.GetComponent<BoxCollider2D>().center.x),
                                            (gameObject.transform.position.y + gameObject.GetComponent<BoxCollider2D>().center.y), gameObject.transform.position.z);

        startPosition = centerPosition;


        //endLocation = this.transform;
        endLocation.position = new Vector3(endLocation.position.x, endLocation.position.y + distanceToMove, endLocation.position.z);
        //Debug.Log("Start: " + startPosition.x + " " + startPosition.y + "        " + "End: " + endLocation.position);

        if(startsRaised)
        {

            isRaising = !isRaising;
            //Play(soundToPlay);
            isFinished = false;

            

            //if (anim != null) anim.SetTrigger("trigger");


        }

    }

    //Called every fixed framerate, because framerate in Update() can vary, making movements jerky
    void FixedUpdate()
    {
        //Debug.Log("isRaising: " + isRaising);
        //Debug.Log("isFinished: " + isFinished);


        //If the platform is raising up...
        if (isRaising && !isFinished)
        {

            //Debug.Log("raising");



            //Create a variable called "difference" of type "Vector 2" and set it equal to the current position of the platform
            //minus the position of the EndLocation variable which is set in the editor
            Vector2 difference = transform.position - endLocation.position;


            //If the platform has reached its end location, stop
            if (difference.sqrMagnitude < 0.001f)
            {
                //Debug.Log("finished raising");
                //this.collider2D.enabled = false;
                isFinished = true;
                

            }

            //If the platform has not reached its end location, continue to move
            else
            {
                isFinished = false;

                //Create a variable called "difference" of type "Vector 2" and set it equal to the position of the EndLocation variable which is set in the editor
                //minus the current position of the platform
                Vector2 direction = (endLocation.position - transform.position);

                //Debug.Log(direction);

                //Shrink the length of the Vector in "direction" to one
                direction.Normalize();

                //Move the platform equal to the Vector "direction" multiplied by the speed set by the designer
                rigidbody2D.velocity = direction * speed;


                this.collider2D.enabled = false;
            }

        }

        //If the platform is lowering down...
        else if (!isRaising && !isFinished)
        {
            //Debug.Log("lowering");


            //Create a variable called "difference" of type "Vector 2" and set it euqal to the current position of the platform
            //minus the original starting position of the platform; in other words, how far it is away from start
            Vector2 difference = transform.position - startPosition;

            //If the platform has gotten back to the beginning point and stopped
            if (difference.sqrMagnitude < 0.001f)
            {
                //Debug.Log("finished lowering");
                this.collider2D.enabled = true;
                isFinished = true;
            }

            //If the platform is heading back to the start position but has not gotten there yet, keep moving
            else
            {
                isFinished = false;
                

                //Determine how far away the platform is from its starting position
                Vector2 direction = (startPosition - transform.position);


                //Debug.Log(direction);


                //Shrink the length of the Vector in "direction" to one
                direction.Normalize();

                //Move the platform equal to the Vector "direction" multiplied by the speed set by the designer
                rigidbody2D.velocity = direction * speed;
            }
        }
        else
        {
            isFinished = true;
            rigidbody2D.velocity = new Vector2(0, 0);
        }



        /*
        //If waiting is false and activated is true, so the platform is not waiting at the end of its path, and it is activated
        if (!waiting && startEnabled)
        {
            


        }

        //If it is not moving forward OR backward, stop it
        else
        {
            rigidbody2D.velocity = new Vector2(0, 0);
        }
        */




    }

}
