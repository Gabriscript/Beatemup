using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemieDestroy : MonoBehaviour
{
    public GameObject myself;


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "weapon")
        {
            Destroy(myself);
        }
    }
    
}
