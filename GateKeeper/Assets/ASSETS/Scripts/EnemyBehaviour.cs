using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Player reference")]
    public Transform playerPosition;
    public float chaseRadius;
    public float attackRadius;

    [Header("Enemy stats")]
    public int enemyLife = 2;
    public int enemyScore = 100;
    public float enemyAttackRatio;
    public float enemyImpulseForce;

    AIPath enemyAI;
    Animator enemyAC;
    AudioSource enemyAS;
    Patrol enemyPatrol;
    Rigidbody2D enemyRB;

    Vector3 initPos;
    public bool followPlayer;

    public bool DEBUG;
    
    public bool attacking;
    [HideInInspector]
    public bool activeAI = true;

    void Start()
    {
        enemyAI = GetComponent<AIPath>();
        enemyAC = GetComponent<Animator>();
        enemyAS = GetComponent<AudioSource>();
        enemyPatrol = GetComponent<Patrol>();
        enemyRB = GetComponent<Rigidbody2D>();

        activeAI = true;
        if (DEBUG)
        {
            initPos = transform.position;
        }        
    }

    float xValue, yValue;

    void Update()
    {
        enemyAI.enabled = activeAI;

        xValue = (enemyRB.velocity.normalized.x);


        if (DEBUG) 
        {
            CheckDistance();
        }
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlaySound(); // TEMP -> va utilizzata nell'animazione
            PlayerMovement _playerMovement = collision.GetComponent<PlayerMovement>();
            _playerMovement.enabled = false;
            Rigidbody2D _playerRB = collision.GetComponent<Rigidbody2D>();
            PlayerBehaviour.instancePB.PlayerDamaged();
            Vector2 difference = (_playerRB.transform.position - transform.position);
            difference = difference.normalized * enemyImpulseForce;
            _playerRB.AddForce(difference, ForceMode2D.Impulse);
            StartCoroutine(StopPlayer(_playerRB, _playerMovement));
        }
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
        attacking = true;        
        yield return new WaitForSeconds(enemyAttackRatio);
        attacking = false;        
    }

    IEnumerator StopPlayer(Rigidbody2D playerRB, PlayerMovement playerMove)
    {
        yield return new WaitForSeconds(1f);
        playerMove.enabled = true;
        playerRB.velocity = Vector2.zero;
    }
}
