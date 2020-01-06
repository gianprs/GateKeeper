using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashBehaviour : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            Destroy(gameObject);
        }
    }
}
