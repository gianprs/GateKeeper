using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum collectableType { SaccoDiMonete, Tesoro, Cuore, Moltiplicatore, Stivali, Invincibilità};

public class CollectablesBehaviour : MonoBehaviour
{
    public collectableType collectableType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(collectableType == collectableType.SaccoDiMonete)
        {
            print("monete");
        } 
        else if (collectableType == collectableType.Tesoro)
        {

        }
        else if (collectableType == collectableType.Cuore)
        {

        }
        else if (collectableType == collectableType.Moltiplicatore)
        {

        }
        else if (collectableType == collectableType.Tesoro)
        {

        }
    }
}
