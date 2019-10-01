using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // da sostituire con evento per animazione 

        if(collision.transform.CompareTag("Fantasma"))
        {
            GameManager.fantasma_count++;
            GameManager.zombie_count = 0;
            GameManager.armatura_count = 0;
            GameManager.UpdateText();            
            Destroy(collision.gameObject);            
        }

        if (collision.transform.CompareTag("Zombie"))
        {
            GameManager.zombie_count++;
            GameManager.fantasma_count = 0;
            GameManager.armatura_count = 0;
            GameManager.UpdateText();
            Destroy(collision.gameObject);
        }

        if (collision.transform.CompareTag("Armatura"))
        {
            GameManager.armatura_count++;
            GameManager.fantasma_count = 0;
            GameManager.zombie_count = 0;
            GameManager.UpdateText();
            Destroy(collision.gameObject);
        }
    }
}
