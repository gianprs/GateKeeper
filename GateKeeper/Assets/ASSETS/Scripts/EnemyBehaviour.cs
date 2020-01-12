using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public enum enemyType {Ghost, Vampire, Zombie, Armor}

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Player reference")]
    public Transform playerPosition;
    public float chaseRadius;
    public float attackRadius;

    [Header("Enemy stats")]
    public enemyType enemyClass;
    public int enemyLife = 2;
    public int enemyScore = 100;
    public float enemyAttackRatio;
    public float enemyImpulseForce;

    [Header("Vampire attack")]
    public GameObject vampireSlash;
    public float slashSpeed;

    AIPath enemyAI;
    Animator enemyAC;
    AudioSource enemyAS;
    Patrol enemyPatrol;
    Rigidbody2D enemyRB;

    [HideInInspector]
    public bool attacking;
    [HideInInspector]
    public bool activeAI = true;

    private float tempAIspeed;

    

    void Start()
    {
        if(enemyClass != enemyType.Ghost)
        {
            enemyAI = GetComponent<AIPath>();
            enemyPatrol = GetComponent<Patrol>();

            tempAIspeed = enemyAI.maxSpeed;
        }        

        enemyAC = GetComponent<Animator>();
        enemyAS = GetComponent<AudioSource>();        
        enemyRB = GetComponent<Rigidbody2D>();

        activeAI = true;       
    }

    float xValue, yValue;

    void Update()
    {
        if (enemyClass != enemyType.Ghost)
        {
            enemyAI.enabled = activeAI;

            xValue = enemyAI.velocity.normalized.x;
            yValue = enemyAI.velocity.normalized.y;

            if(xValue == 0 && yValue == 0)
            {
                if (enemyClass != enemyType.Vampire)
                {
                    enemyAC.SetBool("walk", true);
                }
            } 
            else
            {
                if(enemyClass != enemyType.Vampire)
                {
                    enemyAC.SetBool("walk", true);
                }
                
                enemyAC.SetFloat("hor", xValue);
                enemyAC.SetFloat("ver", yValue);
            }
        }
            
    }

    void FixedUpdate()
    {
        if (enemyClass != enemyType.Ghost)
        {
            CheckDistance();
        }            
    }

    //void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(enemyClass == enemyType.Ghost)
    //    {
    //        if (collision.gameObject.CompareTag("PlayerBody"))
    //        {
    //            print("playerColpito");
    //        }
    //    }
    //}

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHit(collision);
        }
    }

    public void PlayerHit(Collider2D collision)
    {
        PlaySound(); // TEMP -> va utilizzata nell'animazione
        PlayerMovement _playerMovement = collision.GetComponent<PlayerMovement>();
        _playerMovement.enabled = false;
        Rigidbody2D _playerRB = collision.GetComponent<Rigidbody2D>();
        PlayerBehaviour.instancePB.PlayerDamaged();
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
            }
            else if (Vector3.Distance(playerPosition.position, transform.position) <= attackRadius)
            {
                if (!attacking)
                {
                    enemyAC.SetTrigger("attack");
                    StartCoroutine(EnemyAttack());

                    if(enemyClass == enemyType.Vampire)
                    {
                        if(vampireSlash != null)
                        {
                            GameObject cloneSlash = Instantiate(vampireSlash, transform.position, transform.rotation);
                            SlashBehaviour cloneSlashBehaviour = cloneSlash.GetComponent<SlashBehaviour>();
                            cloneSlashBehaviour.vampireSlash = true;
                            Rigidbody2D cloneRB = cloneSlash.GetComponent<Rigidbody2D>();
                            Vector2 direction = playerPosition.position - transform.position;
                            cloneRB.AddForce(direction * slashSpeed, ForceMode2D.Impulse);
                            Animator slashAnim = cloneSlash.GetComponent<Animator>();
                            if(direction.y > 0)
                            {
                                slashAnim.SetBool("dx_front", true);
                            } else if (direction.y < 0)
                            {
                                slashAnim.SetBool("sx_back", true);
                            }
                            
                        }                        
                    }
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

        Destroy(gameObject);
    }

    // chiamo questa funzione da evento nell'animazione
    public void PlaySound()
    {        
        enemyAS.Play();
    }

    IEnumerator EnemyAttack()
    {
        enemyAI.maxSpeed = 0;
        attacking = true;        
        yield return new WaitForSeconds(enemyAttackRatio);
        attacking = false;
        enemyAI.maxSpeed = tempAIspeed;
    }
}
