using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_Life : MonoBehaviour
{
    private CircleCollider2D myCollider2D;
    private AudioSource myAudio;
    private SpriteRenderer childrenSprite;

    void Start()
    {
        myCollider2D = GetComponent<CircleCollider2D>();
        myAudio = GetComponent<AudioSource>();
        childrenSprite = GetComponentInChildren<SpriteRenderer>();
    }

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

            myCollider2D.enabled = false;
            childrenSprite.enabled = false;
            StartCoroutine(SoundAndDestroy());
        }
    }

    IEnumerator SoundAndDestroy()
    {
        myAudio.Play();
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
