using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashBehaviour : MonoBehaviour
{
    

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fantasma") 
            || collision.CompareTag("Zombie") 
            || collision.CompareTag("Armatura") 
            || collision.CompareTag("Bat") 
            || collision.CompareTag("WallCollider"))
        {
            Destroy(gameObject);
        }
    }
}
