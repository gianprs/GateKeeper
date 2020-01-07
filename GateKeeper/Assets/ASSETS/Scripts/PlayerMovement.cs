using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{    
    public Rigidbody2D playerRB;
    public Animator playerAC;    

    public float playerSpeed;

    Vector2 movement;

    private float assignedSpeed;    

    void Start()
    {
        assignedSpeed = playerSpeed;
    }

    void Update()
    {
        if(!PlayerBehaviour.instancePB.playerDead)
        {
            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
        }        
        
        if (Input.GetButton("Horizontal") && Input.GetButton("Vertical"))
        {
            playerSpeed = assignedSpeed - 0.5f;
        } 
        else
        {
            playerSpeed = assignedSpeed;
        }

        // GESTIONE ANIMATOR CONTROLLER
        if(playerAC != null)
        {
            Movement();
        }        
    }

    void FixedUpdate()
    {
        if (!PlayerBehaviour.instancePB.playerDead)
        {
            playerRB.MovePosition(playerRB.position + movement * playerSpeed * Time.fixedDeltaTime);
        }        
    }

    void Movement()
    {
        playerAC.SetBool("move", (Input.GetButton("Horizontal") || Input.GetButton("Vertical")));

        if (movement.magnitude != 0)
        {
            playerAC.SetFloat("Hor", movement.x);
            playerAC.SetFloat("Ver", movement.y);
        }
    }    
}
