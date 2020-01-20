using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashBehaviour : MonoBehaviour
{
    SpriteRenderer slashSprite;
    AudioSource slashAudio;
    public Collider2D myCollider;

    public int damageAmount = 1;
    public float slashImpulseForce;

    [HideInInspector]
    public bool playerSlash, vampireSlash;

    void Start()
    {
        slashSprite = GetComponent<SpriteRenderer>();
        slashAudio = GetComponent<AudioSource>();
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WallCollider"))
        {
            Destroy(gameObject);
        }

        if (playerSlash)
        {
            if (collision.CompareTag("Fantasma") || collision.CompareTag("Zombie") || collision.CompareTag("Armatura") || collision.CompareTag("Bat"))
            {
                slashAudio.Play();
                PlayerBehaviour.instancePB.EnemyHit(collision);                

                StartCoroutine(DestroySlash());
            }
        }
        else if(vampireSlash)
        {
            if (collision.CompareTag("Player"))
            {
                slashAudio.Play();
                PlayerMovement _playerMovement = collision.GetComponent<PlayerMovement>();
                _playerMovement.enabled = false;
                Rigidbody2D _playerRB = collision.GetComponent<Rigidbody2D>();
                PlayerBehaviour.instancePB.PlayerDamaged(damageAmount);
                Vector2 difference = (_playerRB.transform.position - transform.position);
                difference = difference.normalized * slashImpulseForce;
                _playerRB.AddForce(difference, ForceMode2D.Impulse);

                StartCoroutine(DestroySlash());
            } 
        }        
    }

    IEnumerator DestroySlash()
    {
        myCollider.enabled = false;
        slashSprite.enabled = false;
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
