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
    public static int bat_count = 0;

    public static int enemiesCount;
    public static bool powerUptaken;

    public static float score;

    public PlayerBehaviour _playerBehaviour;

    public Text enemiesKilledCount, scoreText, gameOverText, finalScoreText;
    public Image firstHeart, secondHeart, thirdHeart;
    public Sprite fullheart, halfHeart, emptyHeart;

    [Header("Power Up Time")]
    public float powerUpTimeGhost;
    public float powerUpTimeArmor;
    public float powerUpTimeVampire;
    public float powerUpTimeZombie;

    float powerUpTime;
    int enemiesCountSprite;
    bool addScore;

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
        bat_count = 0;
        enemiesCount = 0;
        score = 0;
        powerUptaken = false;

        firstHeart.sprite = fullheart;
        secondHeart.sprite = fullheart;
        thirdHeart.sprite = fullheart;

        enemiesKilledCount.text = ("no power");
        scoreText.text = ("Score: " + score);
        gameOverText.enabled = false;
        finalScoreText.enabled = false;
    }

    
    void Update()
    {
        instanceGM.scoreText.text = "Score: " + score;

        if(!PlayerBehaviour.instancePB.playerDead)
        {
            // se uccido tre nemici uguali di fila sblocco il powerUp
            if (enemiesCount == 3 && !powerUptaken)
            {
                powerUptaken = true;
                PowerUp();
            }

            // se uccido tre nemici diversi di fila sblocco il powerUp
            if (enemiesCountSprite == 3 && !powerUptaken)
            {
                StartCoroutine(wrongEnemiesCombination());
            }
        } 
        else
        {
            enemiesKilledCount.enabled = false;
            scoreText.enabled = false;
            gameOverText.enabled = true;
            finalScoreText.enabled = true;
            finalScoreText.text = ("Your score is " + score);
            if(!addScore)
            {
                // QUI AGGIUNGO IL PUNTEGGIO
                // scelta nome 3 lettere

                addScore = true;
            }
        }
        
    }

    void PowerUp()
    {
        instanceGM.enemiesKilledCount.text = ("POWER UP ATTIVO");
        enemiesCountSprite = 0;
        instanceGM.StartCoroutine(instanceGM.powerUpCountdown());
    }

    public static void UpdateText()
    {
        if (!powerUptaken)
        {
            if (zombie_count == 0 && armatura_count == 0 && bat_count == 0)
            {
                enemiesCount = fantasma_count;
            }
            else if (fantasma_count == 0 && armatura_count == 0 && bat_count == 0)
            {
                enemiesCount = zombie_count;
            }
            else if (fantasma_count == 0 && zombie_count == 0 && bat_count == 0)
            {
                enemiesCount = armatura_count;
            }
            else if (fantasma_count == 0 && zombie_count == 0 && armatura_count == 0)
            {
                enemiesCount = bat_count;
            }
            

            if(enemiesCount != 0)            
            instanceGM.enemiesCountSprite++;
        }
    }

    void ResetSlotSprite()
    {
        enemiesCountSprite = 0;

        // riporto il counter dei nemici a zero
        fantasma_count = 0;
        zombie_count = 0;
        armatura_count = 0;
        bat_count = 0;

        // resetto le sprite nella slot
        _playerBehaviour.slot_01.sprite = null;
        _playerBehaviour.slot_02.sprite = null;
        _playerBehaviour.slot_03.sprite = null;
    }

    IEnumerator powerUpCountdown()
    {
        if (fantasma_count == 3)
        {
            _playerBehaviour.powerUpFantasma = true;
            powerUpTime = powerUpTimeGhost;
        }
        else if (zombie_count == 3)
        {
            _playerBehaviour.powerUpZombie = true;
            powerUpTime = powerUpTimeZombie;
        }
        else if (armatura_count == 3)
        {
            _playerBehaviour.powerUpArmatura = true;
            powerUpTime = powerUpTimeArmor;
        } 
        else if (bat_count == 3)
        {
            _playerBehaviour.powerUpBat = true;
            powerUpTime = powerUpTimeVampire;
        }

        yield return new WaitForSeconds(powerUpTime);

        ResetSlotSprite();

        // annullo il powerUp
        powerUptaken = false;

        _playerBehaviour.powerUpFantasma = false;
        _playerBehaviour.powerUpZombie = false;
        _playerBehaviour.powerUpArmatura = false;
        _playerBehaviour.powerUpBat = false;

        _playerBehaviour.ResetPlayerPower();



        // aggiorno la UI
        enemiesKilledCount.text = ("no power");
        UpdateText();
    }

    IEnumerator wrongEnemiesCombination()
    {
        instanceGM.enemiesKilledCount.text = ("WRONG COMBINATION");
        yield return new WaitForSeconds(1);
        ResetSlotSprite();
        instanceGM.enemiesKilledCount.text = ("no power");

    }

    // VITA PLAYER
    public void UpdateHeart()
    {
        if (PlayerBehaviour.instancePB.playerLife == 6)
        {
            firstHeart.sprite = fullheart;
        }
        else if (PlayerBehaviour.instancePB.playerLife == 5)
        {
            firstHeart.sprite = halfHeart;
        }
        else if (PlayerBehaviour.instancePB.playerLife == 4)
        {
            firstHeart.sprite = emptyHeart;
        }
        else if (PlayerBehaviour.instancePB.playerLife == 3)
        {
            secondHeart.sprite = halfHeart;
        }
        else if (PlayerBehaviour.instancePB.playerLife == 2)
        {
            secondHeart.sprite = emptyHeart;
        }
        else if (PlayerBehaviour.instancePB.playerLife == 1)
        {
            thirdHeart.sprite = halfHeart;
        }
        else if (PlayerBehaviour.instancePB.playerLife == 0)
        {
            thirdHeart.sprite = emptyHeart;
        }
    }
}
