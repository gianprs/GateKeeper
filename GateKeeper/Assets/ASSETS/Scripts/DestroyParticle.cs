using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticle : MonoBehaviour
{
    public void DestroyMySelf()
    {
        Destroy(gameObject);
    }
}
