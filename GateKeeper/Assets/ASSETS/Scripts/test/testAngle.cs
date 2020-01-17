using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAngle : MonoBehaviour
{

    public Transform obj;

    public GameObject up, down, dx, sx;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 vectorDir = obj.position - transform.position;

        float angle = Vector2.SignedAngle(vectorDir, transform.position);

        if(angle < 45 && angle > -45)
        {
            // down
            up.SetActive(false);
            down.SetActive(true);
            dx.SetActive(false);
            sx.SetActive(false);
        } 
        else if (angle > 45 && angle < 135)
        {
            // sx
            up.SetActive(false);
            down.SetActive(false);
            dx.SetActive(false);
            sx.SetActive(true);
        }         
        else if (angle > -135 && angle < -45)
        {
            // dx
            up.SetActive(false);
            down.SetActive(false);
            dx.SetActive(true);
            sx.SetActive(false);
        }
        else 
        {
            // up
            up.SetActive(true);
            down.SetActive(false);
            dx.SetActive(false);
            sx.SetActive(false);
        }
    }
}
