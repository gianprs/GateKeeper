using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable_Gold : MonoBehaviour
{  
    public int collectableScore = 50;

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
            GameManager.score += collectableScore;
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
