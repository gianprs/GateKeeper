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

    public Transform player, princess;

    public Text enemiesKilledCount, scoreText, gameOverText, finalScoreText;
    public Image firstHeart, secondHeart, thirdHeart;
    public Sprite fullheart, halfHeart, emptyHeart;
    public Animator blackScreenFade;

    [Header("Help text messages")]
    public float timeMessage;
    public GameObject up, down, dx, dx_up, dx_down, sx, sx_up, sx_down;

    [Header("Power Up Time")]
    public float powerUpTimeGhost;
    public float powerUpTimeArmor;
    public float powerUpTimeVampire;
    public float powerUpTimeZombie;    

    float powerUpTime, timeMessageMemory;
    int enemiesCountSprite;
    bool addScore, showMessage;


    public bool nameSelection;
    bool input, inputV1;
    int l1, t1;

    List<string> letter = new List<string>() { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

    public Text[] playerLettersText, p1_selection;
    public Color standardColor, highlightedColor;


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

        timeMessageMemory = timeMessage;
        up.SetActive(false);
        down.SetActive(false);
        dx.SetActive(false);
        dx_down.SetActive(false);
        dx_up.SetActive(false);
        sx.SetActive(false);
        sx_down.SetActive(false);
        sx_up.SetActive(false);

        blackScreenFade.gameObject.SetActive(true);
        blackScreenFade.SetBool("Fade", true);

        playerLettersText[0].color = highlightedColor;
        playerLettersText[1].color = standardColor;
        playerLettersText[2].color = standardColor;

        for (int i = 0; i < playerLettersText.Length; i++)
        {
            playerLettersText[i].gameObject.SetActive(false);
        }
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

            if (!showMessage)
            {
                timeMessage -= Time.deltaTime;
                if (timeMessage <= 0)
                {
                    StartCoroutine(CheckPrincessPosition());
                    showMessage = true;
                }
            }

            
        } 
        else
        {
            enemiesKilledCount.enabled = false;
            scoreText.enabled = false;
            gameOverText.enabled = true;
            finalScoreText.enabled = true;
            finalScoreText.text = ("Your score is " + score);

            if (nameSelection)
            {
                for (int i = 0; i < playerLettersText.Length; i++)
                {
                    playerLettersText[i].gameObject.SetActive(true);
                }

                if (!inputV1 && Input.GetAxis("Vertical") < -0.5f || Input.GetKeyDown(KeyCode.Y))
                {
                    inputV1 = true;
                    l1++;
                    if (l1 >= letter.Count) l1 = 0;
                    playerLettersText[t1].text = letter[l1];
                }
                if (!inputV1 && Input.GetAxis("Vertical") > 0.5f || Input.GetKeyDown(KeyCode.H))
                {
                    inputV1 = true;
                    l1--;
                    if (l1 < 0) l1 = letter.Count - 1;
                    playerLettersText[t1].text = letter[l1];
                }

                if (!input && Input.GetAxis("Horizontal") > 0.5f || Input.GetKeyDown(KeyCode.J))
                {
                    input = true;
                    playerLettersText[t1].color = standardColor;
                    t1++;
                    if (t1 >= playerLettersText.Length) t1 = 0;
                    l1 = letter.IndexOf(playerLettersText[t1].text);
                    playerLettersText[t1].color = highlightedColor;
                }
                if (!input && Input.GetAxis("Horizontal") < -0.5f || Input.GetKeyDown(KeyCode.G))
                {
                    input = true;
                    playerLettersText[t1].color = standardColor;
                    t1--;
                    if (t1 < 0) t1 = playerLettersText.Length - 1;
                    l1 = letter.IndexOf(playerLettersText[t1].text);
                    playerLettersText[t1].color = highlightedColor;
                }

                if (Input.GetAxis("Vertical") < 0.5f && Input.GetAxis("Vertical") > -0.5f)
                {
                    inputV1 = false;
                }

                if (!input && Input.GetAxis("Horizontal") > 0.5f || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    input = true;

                }
                else if (!input && Input.GetAxis("Horizontal") < -0.5f || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    input = true;
                }
                else if (Input.GetAxis("Horizontal") < 0.5f && Input.GetAxis("Horizontal") > -0.5f || (Input.GetKeyUp(KeyCode.RightArrow) && Input.GetKeyDown(KeyCode.LeftArrow)))
                {
                    input = false;
                }

                // gestisco input che mi va a confermare il nome del player e poi setto lo score

                //if(!addScore)
                //{
                //    // QUI AGGIUNGO IL PUNTEGGIO
                //    // scelta nome 3 lettere

                //    addScore = true;
                //    nameSelection = false;
                //}
            }
            else
            {
                for (int i = 0; i < playerLettersText.Length; i++)
                {
                    playerLettersText[i].gameObject.SetActive(false);
                }
            }
        }
    }

    void PowerUp()
    {
        instanceGM.enemiesKilledCount.text = ("POWER UP ATTIVO");
        enemiesCountSprite = 0;
        instanceGM.StartCoroutine(instanceGM.powerUpCountdown());
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

    IEnumerator CheckPrincessPosition()
    {
        Vector2 vectorDir = player.position - princess.position;

        float angle = Vector2.SignedAngle(new Vector2(1, 0), player.InverseTransformPoint(princess.position).normalized);

        if (angle < 22.5f && angle > -22.5f)
        {
            // dx
            up.SetActive(false);
            down.SetActive(false);
            dx.SetActive(true);
            dx_down.SetActive(false);
            dx_up.SetActive(false);
            sx.SetActive(false);
            sx_down.SetActive(false);
            sx_up.SetActive(false);
        }
        else if (angle > 22.5f && angle < 67.5f)
        {
            // dx_up
            up.SetActive(false);
            down.SetActive(false);
            dx.SetActive(false);
            dx_down.SetActive(false);
            dx_up.SetActive(true);
            sx.SetActive(false);
            sx_down.SetActive(false);
            sx_up.SetActive(false);
        }
        else if (angle > 67.5f && angle < 112.5f)
        {
            // up
            up.SetActive(true);
            down.SetActive(false);
            dx.SetActive(false);
            dx_down.SetActive(false);
            dx_up.SetActive(false);
            sx.SetActive(false);
            sx_down.SetActive(false);
            sx_up.SetActive(false);
        }
        else if (angle > 112.5f && angle < 157.5f)
        {
            // sx_up
            up.SetActive(false);
            down.SetActive(false);
            dx.SetActive(false);
            dx_down.SetActive(false);
            dx_up.SetActive(false);
            sx.SetActive(false);
            sx_down.SetActive(false);
            sx_up.SetActive(true);
        }
        else if (angle > -157.5f && angle < -112.5f)
        {
            // sx_down
            up.SetActive(false);
            down.SetActive(false);
            dx.SetActive(false);
            dx_down.SetActive(false);
            dx_up.SetActive(false);
            sx.SetActive(false);
            sx_down.SetActive(true);
            sx_up.SetActive(false);
        }
        else if (angle > -112.5f && angle < -67.5f)
        {
            // down
            up.SetActive(false);
            down.SetActive(true);
            dx.SetActive(false);
            dx_down.SetActive(false);
            dx_up.SetActive(false);
            sx.SetActive(false);
            sx_down.SetActive(false);
            sx_up.SetActive(false);
        }
        else if (angle > -67.5f && angle < -22.5f)
        {
            // dx_down
            up.SetActive(false);
            down.SetActive(false);
            dx.SetActive(false);
            dx_down.SetActive(true);
            dx_up.SetActive(false);
            sx.SetActive(false);
            sx_down.SetActive(false);
            sx_up.SetActive(false);
        }
        else
        {
            // sx
            up.SetActive(false);
            down.SetActive(false);
            dx.SetActive(false);
            dx_down.SetActive(false);
            dx_up.SetActive(false);
            sx.SetActive(true);
            sx_down.SetActive(false);
            sx_up.SetActive(false);
        }

        yield return new WaitForSeconds(2);

        timeMessage = timeMessageMemory;
        showMessage = false;

        up.SetActive(false);
        down.SetActive(false);
        dx.SetActive(false);
        dx_down.SetActive(false);
        dx_up.SetActive(false);
        sx.SetActive(false);
        sx_down.SetActive(false);
        sx_up.SetActive(false);
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
    
}
