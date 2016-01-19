using UnityEngine;
using System.Collections;

public class LevenCompleteScreen : MonoBehaviour 
{
    public GameController gameController;
    private bool triggerEntered = false;
	// Use this for initialization
	void Start () 
    {
        gameController = GameObject.FindGameObjectWithTag("PauseScreen").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () 
    {

	}

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player" && !triggerEntered)
        {
            triggerEntered = true;
            gameController.endOfLevel = true;
            gameController.TogglePause(false);
        }
    }
}
