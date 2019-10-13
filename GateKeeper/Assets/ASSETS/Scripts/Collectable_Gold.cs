using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_Gold : MonoBehaviour
{
    public int collectableScore = 50;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.score += collectableScore;
            Destroy(this.gameObject);
        }
    }
}
