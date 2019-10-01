using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float playerSpeed = 2;

    public Rigidbody2D playerRB;
    public Animator playerAC;

    Vector2 movement;
    
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");


        //questo lo attivo quando ho tutte le animazioni per ogni lato

        /*
        if(movement.magnitude != 0)
        {
            playerAC.SetFloat("_hor", movement.x);
            playerAC.SetFloat("_ver", movement.y);
        }
        */


        if(playerAC != null)
        {
            playerAC.SetFloat("_hor", movement.x);
            playerAC.SetFloat("_ver", movement.y);
        }        

    }


    void FixedUpdate()
    {
        playerRB.MovePosition(playerRB.position + movement * playerSpeed * Time.fixedDeltaTime);
    }
}
