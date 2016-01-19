using UnityEngine;
using System.Collections;

public class PigIcons : MonoBehaviour {


    public GameObject alertIcon;
    private GameObject lostIcon;
    public GameObject[] lostMessagePool;
    private AIControllerPatrolPig controllerPatrolPig;
    bool hasChangedTexture = false;
    bool m_hasFlipped = false;
    public Vector3 m_localScale;
    //public AIControllerPatrolPig.AlertState alertState;


	// Use this for initialization
	void Start () 
    {
        if ( alertIcon != null ) alertIcon.GetComponent<SpriteRenderer>().enabled = false;
        lostIcon = lostMessagePool[0];
        if ( lostIcon != null ) lostIcon.GetComponent<SpriteRenderer>().enabled = false;
        controllerPatrolPig = this.GetComponent<AIControllerPatrolPig>();
        m_localScale = lostIcon.transform.localScale;

//         foreach ( GameObject currentObject in lostMessagePool )
//         {
//             currentObject.GetComponent<SpriteRenderer>().enabled = false;
//         }
        for ( int i = 0; i < lostMessagePool.Length; ++i )
        {
            lostMessagePool[i].GetComponent<SpriteRenderer>().enabled = false;
        }
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (controllerPatrolPig.m_currentPigAlertState == AIControllerPatrolPig.AlertState.NotAlert)
        {
            alertIcon.GetComponent<SpriteRenderer>().enabled = false;
            lostIcon.GetComponent<SpriteRenderer>().enabled = false;
            hasChangedTexture = false;
            m_hasFlipped = false;
        }
        else if (controllerPatrolPig.m_currentPigAlertState == AIControllerPatrolPig.AlertState.ChasePlayer)
        {
            alertIcon.GetComponent<SpriteRenderer>().enabled = true;
            lostIcon.GetComponent<SpriteRenderer>().enabled = false;
            hasChangedTexture = false;
            m_hasFlipped = false;
        }
        else if (controllerPatrolPig.m_currentPigAlertState == AIControllerPatrolPig.AlertState.LostPlayer)
        {
            //lostIcon.renderer.material.mainTexture = lostMessagePool[Random.Range(0, lostMessagePool.Length)];
            if (!hasChangedTexture)
            {
                lostIcon = lostMessagePool[Random.Range(0, lostMessagePool.Length)];
                lostIcon.GetComponent<SpriteRenderer>().enabled = true;
                hasChangedTexture = true;
            }
            alertIcon.GetComponent<SpriteRenderer>().enabled = false;

            //Handles the reversing of the facing
            //Vector3 theScale = this.transform.localScale;
            Vector3 parentScale = this.GetComponentInParent<Transform>().localScale;



            if (parentScale.x < 0f && !m_hasFlipped)
            {
                lostIcon.transform.localScale = new Vector3(m_localScale.x * -1f, m_localScale.y, m_localScale.z);
                /*theScale.x *= -1;*/
                m_hasFlipped = true;
            }
            else if ( parentScale.x > 0f && !m_hasFlipped )
            {
                lostIcon.transform.localScale = new Vector3(m_localScale.x, m_localScale.y, m_localScale.z);
                m_hasFlipped = true;
            }
        }
        else if (controllerPatrolPig.m_currentPigAlertState == AIControllerPatrolPig.AlertState.BackToStart)
        {
            alertIcon.GetComponent<SpriteRenderer>().enabled = false;
            lostIcon.GetComponent<SpriteRenderer>().enabled = false;
            hasChangedTexture = false;
            m_hasFlipped = false;
        }
        else
        {
            alertIcon.GetComponent<SpriteRenderer>().enabled = false;
            lostIcon.GetComponent<SpriteRenderer>().enabled = false;
            hasChangedTexture = false;
            m_hasFlipped = false;
        }
	}
}
