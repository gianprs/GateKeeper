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

    public static int enemiesCount;
    public static bool powerUptaken;

    public static float score;

    public PlayerBehaviour _playerBehaviour;

    public Text enemiesKilledCount;
    public Text scoreText;
    public float powerUpTime = 5f;

    int enemiesCountSprite;

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
        enemiesCount = 0;
        score = 0;
        powerUptaken = false;

        instanceGM.enemiesKilledCount.text = ("no power");
        instanceGM.scoreText.text = ("Score: " + score);        
    }

    
    void Update()
    {
        instanceGM.scoreText.text = "Score: " + score;
        // se uccido tre nemici uguali di fila sblocco il powerUp
        if (enemiesCount == 3 && !powerUptaken)
        {            
            print("powerup");            
            powerUptaken = true;
            UpdateText();
        } 
        if (enemiesCountSprite == 3 && !powerUptaken)
        {
            print("cancella");
            ResetSlotSprite();
        }
        
    }

    public static void UpdateText()
    {
        
        // in base al nemico che colpisco azzero il count degli altri

        if (powerUptaken)
        {
            instanceGM.enemiesKilledCount.text = ("POWER UP ATTIVO");
            instanceGM.StartCoroutine(instanceGM.powerUpCountdown());
        } 
        else
        {
            instanceGM.enemiesCountSprite++;

            if (zombie_count == 0 && armatura_count == 0)
            {
                enemiesCount = fantasma_count;
            }
            else if (fantasma_count == 0 && armatura_count == 0)
            {
                enemiesCount = zombie_count;
            }
            else if (fantasma_count == 0 && zombie_count == 0)
            {
                enemiesCount = armatura_count;
            }

            // aggiorno la UI
            //instanceGM.enemiesKilledCount.text = ("Nemici uccisi: " + enemiesCount);
        }

    }

    void ResetSlotSprite()
    {
        enemiesCountSprite = 0;

        // resetto le sprite nella slot
        _playerBehaviour.slot_01.sprite = null;
        _playerBehaviour.slot_02.sprite = null;
        _playerBehaviour.slot_03.sprite = null;
    }

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

        ResetSlotSprite();

        // annullo il powerUp
        powerUptaken = false;

        // riporto il counter dei nemici a zero
        fantasma_count = 0;
        zombie_count = 0;
        armatura_count = 0;

        // aggiorno la UI
        enemiesKilledCount.text = ("no power");
        UpdateText();
    }
}
