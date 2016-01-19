using UnityEngine;
using System.Collections;

public class LevelComplete : MonoBehaviour 
{
    public bool levelComplete = false;
    public bool levelWon = false;
    public InventoryController invControllerScript;

    public float debugFloat;
	// Use this for initialization
	void Start () 
    {
        invControllerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryController>();
        debugFloat = invControllerScript.Pickup1InitialValue;
	}
	
	// Update is called once per frame
	void Update () 
    {
        debugFloat = invControllerScript.Pickup1InitialValue;
	}

    //Called when another collider hits this one
    void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if(otherCollider.gameObject.tag == "Player")
        {
            levelComplete = true;
            levelWon = true;
        }
    }

    //Called when another collider hits this one
    void OnCollisionEnter2D(Collision2D otherCollider)
    {
        if (otherCollider.gameObject.tag == "Player")
        {
            levelComplete = true;
            levelWon = true;
        }
    }
}
