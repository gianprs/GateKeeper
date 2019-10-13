using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG : MonoBehaviour
{
    
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && PlayerBehaviour.instancePB.playerLife >= 1)
        {
            PlayerBehaviour.instancePB.PlayerDamaged();
            print(PlayerBehaviour.instancePB.playerLife);
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            PlayerBehaviour.instancePB.playerLife = 6;
        }
    }
}
