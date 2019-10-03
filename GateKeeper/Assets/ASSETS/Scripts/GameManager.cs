﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instanceGM = null;
    public static int fantasma_count = 0;
    public static int zombie_count = 0;
    public static int armatura_count = 0;    

    public static int enemyCount;
    public static bool powerUptaken;
    
    public Text enemiesKilledCount;
    public Text scoreText;
    public float powerUpTime = 5f;

    public static float score;
    //bool powerUpAlreadyTaken = false;

    void Awake()
    {
        if (instanceGM == null)
        {
            instanceGM = this;
        }   
        else if (instanceGM != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        fantasma_count = 0;
        zombie_count = 0;
        armatura_count = 0;
        enemyCount = 0;
        score = 0;
        powerUptaken = false;

        instanceGM.enemiesKilledCount.text = ("Nemici uccisi: " + enemyCount);
        instanceGM.scoreText.text = ("Score: " + score);
    }

    
    void Update()
    {
        instanceGM.scoreText.text = "Score: " + score;
        // se uccido tre nemici uguali di fila sblocco il powerUp
        if (enemyCount == 3 && !powerUptaken)
        {            
            print("powerup");            
            powerUptaken = true;
            UpdateText();
        }
        
    }

    public static void UpdateText()
    {
        
        // in base al nemico che colpisco azzero il count degli altri

        if (powerUptaken)
        {
            instanceGM.enemiesKilledCount.text = ("POWER UP ATTIVO");
            instanceGM.StartCoroutine(instanceGM.powerUpCountdown());
            //instanceGM.UpdateScore();
        } 
        else
        {
            if (zombie_count == 0 && armatura_count == 0)
            {
                enemyCount = fantasma_count;
            }
            else if (fantasma_count == 0 && armatura_count == 0)
            {
                enemyCount = zombie_count;
            }
            else if (fantasma_count == 0 && zombie_count == 0)
            {
                enemyCount = armatura_count;
            }

            // aggiorno la UI
            //instanceGM.UpdateScore();
            instanceGM.enemiesKilledCount.text = ("Nemici uccisi: " + enemyCount);
        }

        //instanceGM.scoreText.text = "Score: " + score;
        //instanceGM.UpdateScore();

    }


    /*
    public void UpdateScore ()
    {

        #region GESTIONE PUNTEGGI PER NEMICI IN SERIE
        
        if (fantasma_count == 1)
        {
            instanceGM.score += 100;
        }
        else if (fantasma_count == 2)
        {
            instanceGM.score += 200;
        }
        else if (fantasma_count == 3)
        {
            instanceGM.score += 500;
            print("dio");
        }
        else if (fantasma_count > 3)
        {
            instanceGM.score += 500;
            print("cane");
        }

        if (zombie_count == 1)
        {
            instanceGM.score += 100;
        }
        else if (zombie_count == 2)
        {
            instanceGM.score += 200;
        }
        else if (zombie_count == 3)
        {
            instanceGM.score += 500;
        }
        else if (zombie_count > 3)
        {
            instanceGM.score += 500;
        }

        if (armatura_count == 1)
        {
            instanceGM.score += 100;
        }
        else if (armatura_count == 2)
        {
            instanceGM.score += 200;
        }
        else if (armatura_count == 3)
        {
            instanceGM.score += 500;
        }
        else if (armatura_count > 3)
        {
            instanceGM.score += 500;
        }
        
        
        instanceGM.scoreText.text = "Score: " + score;

    }
    */
    
        
    IEnumerator powerUpCountdown()
    {
        if (fantasma_count == 3)
        {
            // power up del fantasma
            print("fantasma");
        }
        else if (zombie_count == 3)
        {
            // power up zombie
            print("zombie");
        }
        else if (armatura_count == 3)
        {
            // power up armatura
            print("armatura");
        }

        yield return new WaitForSeconds(powerUpTime);
        powerUptaken = false;
        fantasma_count = 0;
        zombie_count = 0;
        armatura_count = 0;
        UpdateText();
    }
}
