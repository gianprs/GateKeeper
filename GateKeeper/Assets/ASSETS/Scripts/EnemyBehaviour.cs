using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyBehaviour : MonoBehaviour
{
    public int enemyLife = 2;

    public int enemyScore = 100;

    AIPath enemyAI;

    public Transform tempPlayerPosition;

    Vector3 initPos;
    public bool followPlayer;

    public bool DEBUG;


    void Start()
    {
        if (!DEBUG)
        {
            enemyAI = GetComponent<AIPath>();

            initPos = transform.position;
        }        
    }

    
    void Update()
    {

        if (!DEBUG) 
        {
            if (enemyAI.destination != null)
            {
                if (followPlayer)
                {
                    enemyAI.destination = tempPlayerPosition.position;
                }
                else
                {
                    enemyAI.destination = initPos;
                }

            }
        }
    }

    public void EnemyDeath()
    {

        // TODO animazione morte

        Destroy(gameObject);
    }
}
