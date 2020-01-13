using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBehaviour : MonoBehaviour
{
    
    public AudioClip[] attackSound;

    public static PlayerBehaviour instancePB;
    public Animator playerAC;

    public Image slot_01;
    public Image slot_02;
    public Image slot_03;

    public Sprite fantasmaSprite;
    public Sprite zombieSprite;
    public Sprite armaturaSprite;
    public Sprite batSprite;

    public int playerLife;
    public Color flashColor;
    public Color ghostColor;
    public Color normalColor;
    public float flashDuration;
    public int numberOfFlashesDamaged = 4;

    public float attackRate = 2; 
    
    public float knockTime;
    public float playerImpulseForce;

    //[HideInInspector]
    public bool powerUpFantasma, powerUpZombie, powerUpArmatura, powerUpBat, playerDead;

    Rigidbody2D myRB;
    SpriteRenderer playerSprite;
    AudioSource myAudio;
    Collider2D myCollider;
    PlayerMovement playerMovement;
    private float tempPlayerSpeed;
    private int playerHitDamage = 1;
    private float nextAttackTime = 0;


    public GameObject slashPrefab;
    public float slashSpeed = 5;

    public Transform attackPos_Front, attackPos_Dx, attackPos_Back, attackPos_Sx;

    //singleton
    void Awake()
    {
        if(instancePB == null)
        {
            instancePB = this;
        } 
        else if (instancePB != null)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        playerDead = false;
        playerLife = 6;
        myCollider = GetComponent<Collider2D>();
        myRB = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();

        playerMovement = GetComponent<PlayerMovement>();
        tempPlayerSpeed = playerMovement.playerSpeed;

        myAudio = GetComponent<AudioSource>();
    }
    
    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !playerDead)
            {
                Attack();
                nextAttackTime = Time.time + 1 / attackRate;
            }
        }

        if (powerUpFantasma)
        {
            // invisibilità
            //StartCoroutine(PowerUpGhost());
            playerSprite.color = Color.Lerp(normalColor, ghostColor, Mathf.PingPong(Time.time, 1));
        }
        if(powerUpZombie)
        {
            // super forza me più lento
            playerHitDamage = 2;
            playerMovement.playerSpeed -= 0.5f;
        }
        if(powerUpArmatura)
        {
            // super difesa

        }
        if(powerUpBat)
        {
            // attacco distanza ma meno frequente
            attackRate = 1;
        }
    }

    public void PlayerDamaged()
    {
        playerLife--;

        if(playerLife > 0)
        {
            playerAC.SetTrigger("takeDamage");
            StartCoroutine(FlashPlayerDamaged());
        } 
        if (playerLife == 0 && !playerDead)
        {
            playerAC.SetTrigger("death");
            playerDead = true;
            myRB.velocity = Vector2.zero;

            slot_01.enabled = false;
            slot_02.enabled = false;
            slot_03.enabled = false;
        }        

        

        GameManager.instanceGM.UpdateHeart();
    }

    void Attack()
    {
        playerAC.SetTrigger("attack");

        myAudio.clip = attackSound[Random.Range(0, attackSound.Length)];
        myAudio.Play();

        //suono
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Fantasma") || collision.CompareTag("Zombie") || collision.CompareTag("Armatura") || collision.CompareTag("Bat"))
        {
            EnemyHit(collision);

            EnemyBehaviour enemyBehaviour = collision.GetComponent<EnemyBehaviour>();
            enemyBehaviour.activeAI = false;
        }

        if (collision.CompareTag("FakeWall"))
        {
            FakeWallBehaviour fakeWB = collision.GetComponentInChildren<FakeWallBehaviour>();
            fakeWB.wallLife -= 1;
        }       
    }

    public void EnemyHit(Collider2D collision)
    {
        Rigidbody2D enemyRB = collision.GetComponent<Rigidbody2D>();
        if (enemyRB != null)
        {
            enemyRB.isKinematic = false;
            Vector2 difference = (enemyRB.transform.position - transform.position);
            difference = difference.normalized * playerImpulseForce;
            enemyRB.AddForce(difference, ForceMode2D.Impulse);
            StartCoroutine(KnockEnemy(enemyRB, collision));
        }
    }

    IEnumerator KnockEnemy(Rigidbody2D enemyRB, Collider2D collision)
    {
        yield return new WaitForSeconds(knockTime);
        if (enemyRB != null)
        {            
            enemyRB.velocity = Vector2.zero;
            enemyRB.isKinematic = true;

            EnemyBehaviour enemyBehaviour = enemyRB.gameObject.GetComponent<EnemyBehaviour>();
            enemyBehaviour.enemyLife -= playerHitDamage;
            enemyBehaviour.activeAI = true;

            if (enemyBehaviour.enemyLife <= 0)
            {
                if (collision.CompareTag("Fantasma"))
                {
                    GameManager.zombie_count = 0;
                    GameManager.armatura_count = 0;
                    GameManager.bat_count = 0;

                    if (!GameManager.powerUptaken)
                    {
                        GameManager.fantasma_count++;
                    }

                    int scoreToAdd = collision.GetComponent<EnemyBehaviour>().enemyScore;

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
                }

                if (collision.CompareTag("Zombie"))
                {
                    GameManager.fantasma_count = 0;
                    GameManager.armatura_count = 0;
                    GameManager.bat_count = 0;

                    if (!GameManager.powerUptaken)
                    {
                        GameManager.zombie_count++;
                    }

                    int scoreToAdd = collision.GetComponent<EnemyBehaviour>().enemyScore;

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
                }

                if (collision.CompareTag("Armatura"))
                {
                    GameManager.fantasma_count = 0;
                    GameManager.zombie_count = 0;
                    GameManager.bat_count = 0;

                    if (!GameManager.powerUptaken)
                    {
                        GameManager.armatura_count++;
                    }

                    int scoreToAdd = collision.GetComponent<EnemyBehaviour>().enemyScore;

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
                }

                if (collision.CompareTag("Bat"))
                {
                    GameManager.fantasma_count = 0;
                    GameManager.zombie_count = 0;
                    GameManager.armatura_count = 0;

                    if (!GameManager.powerUptaken)
                    {
                        GameManager.bat_count++;
                    }

                    int scoreToAdd = collision.GetComponent<EnemyBehaviour>().enemyScore;

                    if (GameManager.bat_count == 1)
                    {
                        // aggiungo il punteggio del nemico
                        GameManager.score += scoreToAdd;

                        // modifico la sprite della slot
                        // se la prima è libera occupo quella se no a discesa
                        if (slot_01.sprite == null)
                        {
                            slot_01.sprite = batSprite;
                        }
                        else if (slot_02.sprite == null)
                        {
                            slot_02.sprite = batSprite;
                        }
                        else if (slot_03.sprite == null)
                        {
                            slot_03.sprite = batSprite;
                        }
                    }
                    else if (GameManager.bat_count == 2)
                    {
                        // aggiungo il punteggio del nemico
                        GameManager.score += scoreToAdd * 2;

                        // modifico la sprite della slot
                        // se la seconda è libera occupo quella se no vado sulla terza
                        if (slot_02.sprite == null)
                        {
                            slot_02.sprite = batSprite;
                        }
                        else if (slot_03.sprite == null)
                        {
                            slot_03.sprite = batSprite;
                        }
                    }
                    else if (GameManager.bat_count > 2)
                    {
                        // aggiungo il punteggio del nemico
                        GameManager.score += scoreToAdd * 5;

                        // modifico la sprite della slot
                        // solo la terza perchè se arrivo qui vuol dire che le altre 2 sono full
                        slot_03.sprite = batSprite;
                    }

                    GameManager.UpdateText();
                }

                enemyBehaviour.EnemyDeath();
            }
        }
    }

    IEnumerator FlashPlayerDamaged()
    {
        int temp = 0;
        myCollider.enabled = false;
        while(temp < numberOfFlashesDamaged)
        {
            playerSprite.color = flashColor;
            yield return new WaitForSeconds(flashDuration);
            playerSprite.color = normalColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        myCollider.enabled = true;

        playerMovement.enabled = true;
        myRB.velocity = Vector2.zero;
    }

    public void ResetPlayerPower()
    {
        // reset poteri
        playerHitDamage = 1;
        playerMovement.playerSpeed = tempPlayerSpeed;

        attackRate = 2;
    }

    #region Bat power up attack direction
    public void AttackBat_DX()
    {
        if (powerUpBat)
        {
            GameObject cloneSlash = Instantiate(slashPrefab, attackPos_Dx.position, transform.rotation);
            SlashBehaviour cloneSlashBehaviour = cloneSlash.GetComponent<SlashBehaviour>();
            cloneSlashBehaviour.vampireSlash = true;
            Rigidbody2D slashRB = cloneSlash.GetComponent<Rigidbody2D>();
            Animator slashAnim = cloneSlash.GetComponent<Animator>();
            slashAnim.SetBool("dx_front", true);
            slashRB.AddForce(Vector2.right * slashSpeed, ForceMode2D.Impulse);
        }
    }

    public void AttackBat_SX()
    {
        if (powerUpBat)
        {
            GameObject cloneSlash = Instantiate(slashPrefab, attackPos_Sx.position, transform.rotation);
            SlashBehaviour cloneSlashBehaviour = cloneSlash.GetComponent<SlashBehaviour>();
            cloneSlashBehaviour.vampireSlash = true;
            Rigidbody2D slashRB = cloneSlash.GetComponent<Rigidbody2D>();
            Animator slashAnim = cloneSlash.GetComponent<Animator>();
            slashAnim.SetBool("sx_back", true);
            slashRB.AddForce(-Vector2.right * slashSpeed, ForceMode2D.Impulse);
        }
    }

    public void AttackBat_UP()
    {
        if (powerUpBat)
        {
            GameObject cloneSlash = Instantiate(slashPrefab, attackPos_Back.position, transform.rotation);
            SlashBehaviour cloneSlashBehaviour = cloneSlash.GetComponent<SlashBehaviour>();
            cloneSlashBehaviour.vampireSlash = true;
            Rigidbody2D slashRB = cloneSlash.GetComponent<Rigidbody2D>();
            Animator slashAnim = cloneSlash.GetComponent<Animator>();
            slashAnim.SetBool("sx_back", true);
            slashRB.AddForce(Vector2.up * slashSpeed, ForceMode2D.Impulse);
        }
    }

    public void AttackBat_DOWN()
    {
        if (powerUpBat)
        {
            GameObject cloneSlash = Instantiate(slashPrefab, attackPos_Front.position, transform.rotation);
            SlashBehaviour cloneSlashBehaviour = cloneSlash.GetComponent<SlashBehaviour>();
            cloneSlashBehaviour.vampireSlash = true;
            Rigidbody2D slashRB = cloneSlash.GetComponent<Rigidbody2D>();
            Animator slashAnim = cloneSlash.GetComponent<Animator>();
            slashAnim.SetBool("dx_front", true);
            slashRB.AddForce(-Vector2.up * slashSpeed, ForceMode2D.Impulse);
        }
    }
    #endregion

    IEnumerator PowerUpGhost()
    {
        print("ghost");
        int temp = 0;
        //myCollider.enabled = false;
        while (temp < numberOfFlashesDamaged)
        {
            playerSprite.color = ghostColor;
            yield return new WaitForSeconds(flashDuration);
            playerSprite.color = normalColor;
            yield return new WaitForSeconds(flashDuration);
            temp++;
        }
        //myCollider.enabled = true;
    }
}
