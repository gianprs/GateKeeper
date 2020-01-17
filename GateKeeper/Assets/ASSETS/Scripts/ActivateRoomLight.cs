using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class ActivateRoomLight : MonoBehaviour
{
    public Animator blackAnim;

    Collider2D myCollider;

    bool activateLight;
    
    void Start()
    {
        

        myCollider = GetComponentInChildren<Collider2D>();

        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !activateLight)
        {

            blackAnim.SetBool("dissolve", true);
            myCollider.enabled = false;
            activateLight = true;
        }
    }
}
