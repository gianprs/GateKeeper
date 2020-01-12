using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class DEBUG_enemyFollowPlayer : MonoBehaviour
{
    
    Path path;
    Rigidbody2D myRB;
    Seeker seeker;
    Animator myAnim;

    public Transform tempPlayerPosition;

    public float speed;
    public float nextWaypointDistance = 3f;
    int currentWaypoint = 0;
    


    // Start is called before the first frame update
    void Start()
    {
        
        seeker = GetComponent<Seeker>();
        myRB = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();

        InvokeRepeating("UpdatePath", 0f, .5f);
        
    }

    void UpdatePath()
    {
        if(seeker.IsDone())
            seeker.StartPath(myRB.position, tempPlayerPosition.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    


    
    void FixedUpdate()
    {
        if(path == null)
        {
            return;
        }

        

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - myRB.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        myRB.AddForce(force);

        float distance = Vector2.Distance(myRB.position, path.vectorPath[currentWaypoint]);
        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }


        myAnim.SetFloat("hor", direction.x);
        myAnim.SetFloat("ver", direction.y);

    }
}
