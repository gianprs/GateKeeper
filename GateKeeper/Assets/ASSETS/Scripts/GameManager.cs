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

    public Text enemiesKilledCount;
    public Text scoreText;
    public float powerUpTime = 5f;

    int enemiesCountSprite;

    public Image firstHeart, secondHeart, thirdHeart;
    public Sprite fullheart, halfHeart, emptyHeart;


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

        instanceGM.enemiesKilledCount.text = ("no power");
        instanceGM.scoreText.text = ("Score: " + score);        
    }

    
    void Update()
    {
        instanceGM.scoreText.text = "Score: " + score;

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
            // power up del fantasma
            print("fantasma");
            _playerBehaviour.powerUpFantasma = true;
        }
        else if (zombie_count == 3)
        {
            // power up zombie
            print("zombie");

            _playerBehaviour.powerUpZombie = true;
        }
        else if (armatura_count == 3)
        {
            // power up armatura
            print("armatura");

            _playerBehaviour.powerUpArmatura = true;
        } 
        else if (bat_count == 3)
        {
            // power up pipistrello
            print("pipistrello");

            _playerBehaviour.powerUpBat = true;
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
