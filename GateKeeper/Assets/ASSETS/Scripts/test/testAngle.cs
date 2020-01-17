using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAngle : MonoBehaviour
{

    public Transform obj;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vectorDir = obj.position - transform.position;

        float angle = Vector2.SignedAngle(vectorDir, transform.position);

        print(angle);
    }
}
