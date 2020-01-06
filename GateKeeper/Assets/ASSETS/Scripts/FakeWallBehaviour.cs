using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeWallBehaviour : MonoBehaviour
{
    public GameObject mainFakeWallObj;

    public int wallLife = 5;
    int tempWallLife;

    void Start()
    {
        tempWallLife = wallLife;
    }

    
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (wallLife == 0)
            {
                print("distruggi");
                Destroy(mainFakeWallObj);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            wallLife = tempWallLife;
        }
    }
}
