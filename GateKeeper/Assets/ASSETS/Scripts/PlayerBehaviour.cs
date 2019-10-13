using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{   

    public Image slot_01;
    public Image slot_02;
    public Image slot_03;

    public Sprite fantasmaSprite;
    public Sprite zombieSprite;
    public Sprite armaturaSprite;

    int playerLife;

    void Start()
    {
        playerLife = 6;
    }

    
    void Update()
    {
        
    }

    public void PlayerDamaged()
    {
        playerLife--;
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
                // aggiungo il punteggio del nemico
                GameManager.score += scoreToAdd;

                // modifico la sprite della slot
                // se la prima è libera occupo quella se no a discesa
                if (slot_01.sprite == null)
                {
                    slot_01.sprite = fantasmaSprite;
                } 
                else if (slot_02.sprite == null)
                {
                    slot_02.sprite = fantasmaSprite;
                } 
                else if (slot_03.sprite == null)
                {
                    slot_03.sprite = fantasmaSprite;
                }
                
            }
            else if (GameManager.fantasma_count == 2)
            {
                // aggiungo il punteggio del nemico
                GameManager.score += scoreToAdd * 2;

                // modifico la sprite della slot
                // se la seconda è libera occupo quella se no vado sulla terza
                if (slot_02.sprite == null)
                {
                    slot_02.sprite = fantasmaSprite;
                }
                else if (slot_03.sprite == null)
                {
                    slot_03.sprite = fantasmaSprite;
                }

            }
            else if (GameManager.fantasma_count > 2)
            {
                // aggiungo il punteggio del nemico
                GameManager.score += scoreToAdd * 5;

                // modifico la sprite della slot
                // solo la terza perchè se arrivo qui vuol dire che le altre 2 sono full
                slot_03.sprite = fantasmaSprite;
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
                // aggiungo il punteggio del nemico
                GameManager.score += scoreToAdd;

                // modifico la sprite della slot
                // se la prima è libera occupo quella se no a discesa
                if (slot_01.sprite == null)
                {
                    slot_01.sprite = zombieSprite;
                }
                else if (slot_02.sprite == null)
                {
                    slot_02.sprite = zombieSprite;
                }
                else if (slot_03.sprite == null)
                {
                    slot_03.sprite = zombieSprite;
                }
            }
            else if (GameManager.zombie_count == 2)
            {
                // aggiungo il punteggio del nemico
                GameManager.score += scoreToAdd * 2;

                // modifico la sprite della slot
                // se la seconda è libera occupo quella se no vado sulla terza
                if (slot_02.sprite == null)
                {
                    slot_02.sprite = zombieSprite;
                }
                else if (slot_03.sprite == null)
                {
                    slot_03.sprite = zombieSprite;
                }
            }
            else if (GameManager.zombie_count > 2)
            {
                // aggiungo il punteggio del nemico
                GameManager.score += scoreToAdd * 5;

                // modifico la sprite della slot
                // solo la terza perchè se arrivo qui vuol dire che le altre 2 sono full
                slot_03.sprite = zombieSprite;
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
                // aggiungo il punteggio del nemico
                GameManager.score += scoreToAdd;

                // modifico la sprite della slot
                // se la prima è libera occupo quella se no a discesa
                if (slot_01.sprite == null)
                {
                    slot_01.sprite = armaturaSprite;
                }
                else if (slot_02.sprite == null)
                {
                    slot_02.sprite = armaturaSprite;
                }
                else if (slot_03.sprite == null)
                {
                    slot_03.sprite = armaturaSprite;
                }
            }
            else if (GameManager.armatura_count == 2)
            {
                // aggiungo il punteggio del nemico
                GameManager.score += scoreToAdd * 2;

                // modifico la sprite della slot
                // se la seconda è libera occupo quella se no vado sulla terza
                if (slot_02.sprite == null)
                {
                    slot_02.sprite = armaturaSprite;
                }
                else if (slot_03.sprite == null)
                {
                    slot_03.sprite = armaturaSprite;
                }
            }
            else if (GameManager.armatura_count > 2)
            {
                // aggiungo il punteggio del nemico
                GameManager.score += scoreToAdd * 5;

                // modifico la sprite della slot
                // solo la terza perchè se arrivo qui vuol dire che le altre 2 sono full
                slot_03.sprite = armaturaSprite;
            }

            GameManager.UpdateText();            

            Destroy(collision.gameObject);
        }
        #endregion

    }
}
