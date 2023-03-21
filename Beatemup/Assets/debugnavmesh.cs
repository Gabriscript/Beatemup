using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class debugnavmesh : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<NavMeshAgent>().destination= Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
