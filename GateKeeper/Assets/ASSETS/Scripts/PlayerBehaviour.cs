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

        #region //---  DA SOSTITUIRE CON EVENTO DA ANIMAZIONE  ---//
        if (collision.transform.CompareTag("Fantasma"))
        {   
            GameManager.zombie_count = 0;
            GameManager.armatura_count = 0;            

            if (!GameManager.powerUptaken)
            {
                GameManager.fantasma_count++;
            }

            int scoreToAdd = collision.transform.GetComponent<EnemyBehaviour>().enemyScore;

            if (GameManager.fantasma_count == 1)
            {
                GameManager.score += scoreToAdd;
            }
            else if (GameManager.fantasma_count == 2)
            {
                GameManager.score += scoreToAdd * 2;
            }
            else if (GameManager.fantasma_count > 2)
            {
                GameManager.score += scoreToAdd * 5;
            }           

            GameManager.UpdateText();
            
            Destroy(collision.gameObject);  
        }

        if (collision.transform.CompareTag("Zombie"))
        {
            GameManager.fantasma_count = 0;
            GameManager.armatura_count = 0;
            
            if (!GameManager.powerUptaken)
            {
                GameManager.zombie_count++;
            }

            int scoreToAdd = collision.transform.GetComponent<EnemyBehaviour>().enemyScore;

            if (GameManager.zombie_count == 1)
            {
                GameManager.score += scoreToAdd;
            }
            else if (GameManager.zombie_count == 2)
            {
                GameManager.score += scoreToAdd * 2;
            }
            else if (GameManager.zombie_count > 2)
            {
                GameManager.score += scoreToAdd * 5;
            }

            GameManager.UpdateText();            

            Destroy(collision.gameObject);
        }

        if (collision.transform.CompareTag("Armatura"))
        {
            GameManager.fantasma_count = 0;
            GameManager.zombie_count = 0;

            if (!GameManager.powerUptaken)
            {
                GameManager.armatura_count++;
            }

            int scoreToAdd = collision.transform.GetComponent<EnemyBehaviour>().enemyScore;

            if (GameManager.armatura_count == 1)
            {
                GameManager.score += scoreToAdd;
            }
            else if (GameManager.armatura_count == 2)
            {
                GameManager.score += scoreToAdd * 2;
            }
            else if (GameManager.armatura_count > 2)
            {
                GameManager.score += scoreToAdd * 5;
            }

            GameManager.UpdateText();            

            Destroy(collision.gameObject);
        }
        #endregion

    }
}
