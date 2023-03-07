using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemieDestroy : MonoBehaviour
{
    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "weapon")
        {       
            Destroy(this);
        }
    }
}
