using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPatroling : MonoBehaviour
{
    public Animator ghostAC;

    public Transform[] ghostWaypoints;

    public float ghostSpeed;
    private int waypointIndex = 0;

    SpriteRenderer ghostSR;
    public Color defaulColor, throughWallColor;

    // Start is called before the first frame update
    void Start()
    {
        ghostSR = GetComponent<SpriteRenderer>();
        ghostSR.color = defaulColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (ghostWaypoints.Length > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, ghostWaypoints[waypointIndex].position, ghostSpeed * Time.deltaTime);

            Vector3 direction = (ghostWaypoints[waypointIndex].position - transform.position).normalized;
            ghostAC.SetFloat("hor", direction.x);
            ghostAC.SetFloat("ver", direction.y);

            if (Vector2.Distance(transform.position, ghostWaypoints[waypointIndex].position) < 0.01f)
            {
                if (waypointIndex < ghostWaypoints.Length)
                {
                    waypointIndex++;
                }
                if (waypointIndex == ghostWaypoints.Length)
                {
                    waypointIndex = 0;
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("WallCollider"))
        {
            ghostSR.color = throughWallColor;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("WallCollider"))
        {
            ghostSR.color = defaulColor;
        }
    }
}
