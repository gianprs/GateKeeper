using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.LWRP;

public class ActivateRoomLight : MonoBehaviour
{
    Light2D _light;

    public Collider2D myCollider;

    bool activateLight;
    
    void Start()
    {
        _light = GetComponent<Light2D>();

        //myCollider = GetComponentInChildren<Collider2D>();

        _light.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !activateLight)
        {
            print("player");

            _light.enabled = true;
            
            myCollider.enabled = false;

            activateLight = true;
        }
    }
}
