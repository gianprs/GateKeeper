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
    public Collider2D front;
    public Collider2D back;
    public Collider2D dx;
    public Collider2D sx;

    AIPath enemyAI;
    Animator enemyAC;
    AudioSource enemyAS;
    Patrol enemyPatrol;
    Rigidbody2D enemyRB;

    [HideInInspector]
    public bool attacking;
    [HideInInspector]
    public bool activeAI = true;

    private float tempAIspeed, tempAttackRadius;
    float xValue, yValue;

    Vector2 pos1, pos2;
    private float thresholdX = 0.55f;
    private float thresholdY = 0.55f;

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
        tempAttackRadius = attackRadius;
    }    

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
                    enemyAC.SetBool("walk", false);
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

        if (enemyClass == enemyType.Vampire)
        {
            if (front != null && back != null && dx != null && sx != null)
            {
                
                if (xValue > thresholdX)
                {
                    front.enabled = false;
                    back.enabled = false;
                    dx.enabled = true;
                    sx.enabled = false;

                } 
                else if (xValue < -thresholdX)
                {

                    front.enabled = false;
                    back.enabled = false;
                    dx.enabled = false;
                    sx.enabled = true;
                }
                else if (yValue > thresholdY)
                {
                    front.enabled = false;
                    back.enabled = true;
                    dx.enabled = false;
                    sx.enabled = false;
                } 
                else if (yValue < -thresholdY)
                {
                    front.enabled = true;
                    back.enabled = false;
                    dx.enabled = false;
                    sx.enabled = false;
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (enemyClass != enemyType.Ghost && enemyClass != enemyType.Vampire)
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
            if (enemyClass == enemyType.Vampire)
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

                    //if (enemyClass == enemyType.Vampire)
                    //{
                    //    enemyAI.maxSpeed = 0;
                    //    attackRadius = 0;

                    //    if (vampireSlash != null)
                    //    {
                    //        GameObject cloneSlash = Instantiate(vampireSlash, transform.position, transform.rotation);
                    //        SlashBehaviour cloneSlashBehaviour = cloneSlash.GetComponent<SlashBehaviour>();
                    //        cloneSlashBehaviour.vampireSlash = true;
                    //        Rigidbody2D cloneRB = cloneSlash.GetComponent<Rigidbody2D>();
                    //        Vector2 direction = playerPosition.position - transform.position;
                    //        cloneRB.AddForce(direction * slashSpeed, ForceMode2D.Impulse);
                    //        Animator slashAnim = cloneSlash.GetComponent<Animator>();
                            

                    //        if (direction.y > 0)
                    //        {
                    //            slashAnim.SetBool("dx_front", true);
                    //        } 
                    //        else if (direction.y < 0)
                    //        {
                    //            slashAnim.SetBool("sx_back", true);
                    //        }
                            
                    //    }                        
                    //}

                    
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
}
