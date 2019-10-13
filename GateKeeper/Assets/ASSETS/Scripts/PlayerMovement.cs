using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    public Rigidbody2D playerRB;
    public Animator playerAC;

    public float playerSpeed = 2;

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
        if (PlayerBehaviour.instancePB.playerLife >= 1)
        playerRB.MovePosition(playerRB.position + movement * playerSpeed * Time.fixedDeltaTime);
    }
}
