using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testAngle : MonoBehaviour
{

    public Transform player, princess;

    public GameObject up, down, dx, dx_up, dx_down, sx, sx_up, sx_down;

    

    // Update is called once per frame
    void Update()
    {
        Vector2 vectorDir = princess.position - player.position;

        float angle = Vector2.SignedAngle(vectorDir, player.position);
        print(angle);

        if(angle < 22.5f && angle > -22.5f)
        {
            // down
            up.SetActive(false);
            down.SetActive(true);
            dx.SetActive(false);
            dx_down.SetActive(false);
            dx_up.SetActive(false);
            sx.SetActive(false);
            sx_down.SetActive(false);
            sx_up.SetActive(false);
        } 
        else if (angle > 22.5f && angle < 67.5f)
        {
            // sx_down
            up.SetActive(false);
            down.SetActive(false);
            dx.SetActive(false);
            dx_down.SetActive(false);
            dx_up.SetActive(false);
            sx.SetActive(false);
            sx_down.SetActive(true);
            sx_up.SetActive(false);
        }
        else if (angle > 67.5f && angle < 112.5f)
        {
            // sx
            up.SetActive(false);
            down.SetActive(false);
            dx.SetActive(false);
            dx_down.SetActive(false);
            dx_up.SetActive(false);
            sx.SetActive(true);
            sx_down.SetActive(false);
            sx_up.SetActive(false);
        }
        else if (angle > 112.5f && angle < 157.5f)
        {
            // sx_up
            up.SetActive(false);
            down.SetActive(false);
            dx.SetActive(false);
            dx_down.SetActive(false);
            dx_up.SetActive(false);
            sx.SetActive(false);
            sx_down.SetActive(false);
            sx_up.SetActive(true);
        }
        else if (angle > -157.5f && angle < -112.5f)
        {
            // dx_up
            up.SetActive(false);
            down.SetActive(false);
            dx.SetActive(false);
            dx_down.SetActive(false);
            dx_up.SetActive(true);
            sx.SetActive(false);
            sx_down.SetActive(false);
            sx_up.SetActive(false);
        }
        else if (angle > -112.5f && angle < -67.5f)
        {
            // dx
            up.SetActive(false);
            down.SetActive(false);
            dx.SetActive(true);
            dx_down.SetActive(false);
            dx_up.SetActive(false);
            sx.SetActive(false);
            sx_down.SetActive(false);
            sx_up.SetActive(false);
        }
        else if (angle > -67.5f && angle < -22.5f)
        {
            // dx_down
            up.SetActive(false);
            down.SetActive(false);
            dx.SetActive(false);
            dx_down.SetActive(true);
            dx_up.SetActive(false);
            sx.SetActive(false);
            sx_down.SetActive(false);
            sx_up.SetActive(false);
        }
        else 
        {
            // up
            up.SetActive(true);
            down.SetActive(false);
            dx.SetActive(false);
            dx_down.SetActive(false);
            dx_up.SetActive(false);
            sx.SetActive(false);
            sx_down.SetActive(false);
            sx_up.SetActive(false);
        }
    }
}
