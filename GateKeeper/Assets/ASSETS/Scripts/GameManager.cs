using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instanceGM = null;
    public static int fantasma_count = 0;
    public static int zombie_count = 0;
    public static int armatura_count = 0;
    public static Text staticEnemiesKilledCount;
    public static Text staticScoreText;

    static int enemyCount;
    static bool powerUptaken;
    
    public Text enemiesKilledCount;
    public Text scoreText;

    float score;

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

        staticEnemiesKilledCount = enemiesKilledCount;
        staticScoreText = scoreText;
        staticEnemiesKilledCount.text = ("Nemici uccisi: " + enemyCount);
        staticScoreText.text = ("Score: " + enemyCount);
    }

    
    void Update()
    {
        // se uccido tre nemici uguali di fila sblocco il powerUp
        if (enemyCount == 3)
        {
            powerUptaken = true;
            print("powerup");
        }
    }



    public static void UpdateText()
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

        // in base al nemico che colpisco azzero il count degli altri
        if (!powerUptaken)
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

            staticEnemiesKilledCount.text = ("Nemici uccisi: " + enemyCount);
        }

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
        } 
        else if (fantasma_count > 3)
        {
            instanceGM.score += 500;
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
        else if(armatura_count > 3)
        {
            instanceGM.score += 500;
        }

        staticScoreText.text = "Score: " + instanceGM.score;

        #endregion
    }
}
