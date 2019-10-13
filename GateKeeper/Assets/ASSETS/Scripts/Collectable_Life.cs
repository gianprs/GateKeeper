using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_Life : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            
            if(PlayerBehaviour.instancePB.playerLife <= 5)
            {
                PlayerBehaviour.instancePB.playerLife++;
                GameManager.instanceGM.UpdateHeart();
            } 
            else if (PlayerBehaviour.instancePB.playerLife == 6)
            {
                GameManager.score += 100;
            }

            Destroy(gameObject);
        }
    }
}
