using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum enemyType {Ghost, Vampire, Zombie, Armor}

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Player reference")]
    Transform playerPosition;
    public float chaseRadius;
    public float attackRadius;

    [Header("Enemy stats")]
    public enemyType enemyClass;
    public int enemyLife = 2;
    public int enemyScore = 100;
    public int enemyDamage = 1;
    public float enemyAttackRatio;
    public float enemyImpulseForce;
    public float timeForRespawning;
    public GameObject particleDeath;

    [Header("Vampire attack")]
    public GameObject vampireSlash;
    public float slashSpeed;

    [Header("Colliders")]
    public GameObject frontCollider;
    public GameObject backCollider;
    public GameObject dxCollider;
    public GameObject sxCollider;

    AIPath enemyAI;
    Patrol enemyPatrol;
    Animator enemyAC;
    AudioSource enemyAS;    
    Rigidbody2D enemyRB;
    SpriteRenderer enemySprite;
    Collider2D enemyMainCollider;

    [HideInInspector]
    public bool attacking;
    [HideInInspector]
    public bool activeAI = true;

    private bool enemyDead;
    private float tempAIspeed, tempAttackRadius;
    private float xValue, yValue;    
    private float thresholdX = 0.55f;
    private float thresholdY = 0.55f;
    private int enemyLifeMemory;
    private Vector3 enemyStartPosition; 

    void Start()
    {
        playerPosition = GameObject.FindGameObjectWithTag("Player").transform;

        enemyLifeMemory = enemyLife;
        enemyStartPosition = transform.position;

        if (enemyClass != enemyType.Ghost)
        {
            enemyAI = GetComponent<AIPath>();
            enemyPatrol = GetComponent<Patrol>();

            tempAIspeed = enemyAI.maxSpeed;
        }        

        enemyAC = GetComponent<Animator>();
        enemyAS = GetComponent<AudioSource>();        
        enemyRB = GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();
        enemyMainCollider = GetComponent<Collider2D>();

        activeAI = true;
        tempAttackRadius = attackRadius;
    }    

    void Update()
    {
        if (!enemyDead)
        {
            if (enemyClass != enemyType.Ghost)
            {
                enemyAI.enabled = activeAI;

                xValue = enemyAI.velocity.normalized.x;
                yValue = enemyAI.velocity.normalized.y;

                if (xValue == 0 && yValue == 0)
                {
                    if (enemyClass != enemyType.Vampire)
                    {
                        enemyAC.SetBool("walk", false);
                    }
                }
                else
                {
                    if (enemyClass != enemyType.Vampire)
                    {
                        enemyAC.SetBool("walk", true);
                    }

                    enemyAC.SetFloat("hor", xValue);
                    enemyAC.SetFloat("ver", yValue);
                }

                if (frontCollider != null && backCollider != null && dxCollider != null && sxCollider != null)
                {
                    if (xValue > thresholdX)
                    {
                        // DX
                        frontCollider.SetActive(false);
                        backCollider.SetActive(false);
                        dxCollider.SetActive(true);
                        sxCollider.SetActive(false);
                    }
                    else if (xValue < -thresholdX)
                    {
                        // SX
                        frontCollider.SetActive(false);
                        backCollider.SetActive(false);
                        dxCollider.SetActive(false);
                        sxCollider.SetActive(true);
                    }
                    else if (yValue > thresholdY)
                    {
                        // BACK
                        frontCollider.SetActive(false);
                        backCollider.SetActive(true);
                        dxCollider.SetActive(false);
                        sxCollider.SetActive(false);
                    }
                    else if (yValue < -thresholdY)
                    {
                        // FRONT
                        frontCollider.SetActive(true);
                        backCollider.SetActive(false);
                        dxCollider.SetActive(false);
                        sxCollider.SetActive(false);
                    }
                }
            }

            if (enemyClass == enemyType.Vampire)
            {
                
            }
        }
        
    }

    void FixedUpdate()
    {
        if (enemyClass != enemyType.Ghost && enemyClass != enemyType.Vampire && !enemyDead)
        {
            CheckDistance();
        }            
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") )
        {            
            if (enemyClass != enemyType.Vampire)
            {
                PlayerHit(collision);
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (enemyClass == enemyType.Vampire && !enemyDead)
            {
                if (!attacking)
                {
                    enemyAC.SetTrigger("attack");
                    StartCoroutine(EnemyAttack());

                    enemyAI.maxSpeed = 0;
                    attackRadius = 0;

                    if (vampireSlash != null)
                    {
                        GameObject cloneSlash = Instantiate(vampireSlash, transform.position, transform.rotation);
                        SlashBehaviour cloneSlashBehaviour = cloneSlash.GetComponent<SlashBehaviour>();
                        cloneSlashBehaviour.vampireSlash = true;
                        Rigidbody2D cloneRB = cloneSlash.GetComponent<Rigidbody2D>();
                        Vector2 direction = playerPosition.position - transform.position;
                        cloneRB.AddForce(direction * slashSpeed, ForceMode2D.Impulse);
                        Animator slashAnim = cloneSlash.GetComponent<Animator>();

                        if (direction.y > 0)
                        {
                            slashAnim.SetBool("dx_front", true);
                        }
                        else if (direction.y < 0)
                        {
                            slashAnim.SetBool("sx_back", true);
                        }
                    }
                }
            }            
        }            
    }

    public void PlayerHit(Collider2D collision)
    {
        PlaySound(); // TEMP -> va utilizzata nell'animazione
        PlayerMovement _playerMovement = collision.GetComponent<PlayerMovement>();
        _playerMovement.enabled = false;
        Rigidbody2D _playerRB = collision.GetComponent<Rigidbody2D>();
        PlayerBehaviour.instancePB.PlayerDamaged(enemyDamage);
        Vector2 difference = (_playerRB.transform.position - transform.position);
        difference = difference.normalized * enemyImpulseForce;
        _playerRB.AddForce(difference, ForceMode2D.Impulse);
    }

    void CheckDistance()
    {
        if (Vector3.Distance(playerPosition.position,transform.position) <= chaseRadius && !PlayerBehaviour.instancePB.playerDead)
        {           
            enemyPatrol.enabled = false;

            if(Vector3.Distance(playerPosition.position, transform.position) > attackRadius)
            {
                if (enemyAI.destination != null)
                {
                    enemyAI.destination = playerPosition.position;
                }
                if (enemyClass == enemyType.Vampire)
                {                    
                    attackRadius = tempAttackRadius;
                }
            }
            else if (Vector3.Distance(playerPosition.position, transform.position) <= attackRadius)
            {
                if (!attacking)
                {
                    enemyAC.SetTrigger("attack");
                    StartCoroutine(EnemyAttack());                    
                }
            }
        }        
        else if (Vector3.Distance(playerPosition.position, transform.position) > chaseRadius || PlayerBehaviour.instancePB.playerDead)
        {
            enemyPatrol.enabled = true;
        }
    }    

    public void EnemyDeath()
    {
        // TODO animazione morte
        enemyDead = true;

        GameObject cloneParticle = Instantiate(particleDeath, transform.position, transform.rotation);

        StartCoroutine(RespawnEnemy());
    }

    #region Eventi da animazione
    // chiamo questa funzione da evento nell'animazione
    public void PlaySound()
    {        
        enemyAS.Play();
    }
    public void StopAI_Attack()
    {
        enemyAI.maxSpeed = 0;
    }
    public void MoveAI_Attack()
    {
        enemyAI.maxSpeed = tempAIspeed;
    }
    #endregion


    IEnumerator EnemyAttack()
    {        
        attacking = true;
        
        yield return new WaitForSeconds(enemyAttackRatio);
        attacking = false;
        if (enemyClass == enemyType.Vampire)
        {
            enemyAI.maxSpeed = tempAIspeed;
            //attackRadius = tempAttackRadius;
        }

    }

    IEnumerator RespawnEnemy()
    {
        if (enemyClass != enemyType.Ghost && enemyClass != enemyType.Vampire)
        {
            enemyAI.enabled = false;
            enemyPatrol.enabled = false;

            enemyAC.SetBool("walk", false);
        }

        enemyAC.enabled = false;
        enemyAS.enabled = false;
        enemySprite.enabled = false;
        enemyMainCollider.enabled = false;

        yield return new WaitForSeconds(timeForRespawning);

        enemyDead = false;

        enemyAC.enabled = true;
        enemyAS.enabled = true;
        enemySprite.enabled = true;
        enemyMainCollider.enabled = true;

        if (enemyClass != enemyType.Ghost)
        {
            enemyAI.enabled = true;
            enemyPatrol.enabled = true;

            enemyAC.Play("Idle");
        }
        enemyLife = enemyLifeMemory;
        transform.position = enemyStartPosition;
    }

    
}
