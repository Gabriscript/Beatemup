using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject firestart;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) 
            Shoot();
        
    }
    void Shoot() {
       


            Instantiate(Resources.Load<GameObject>("prefab/Playerbullet"), firestart.transform.position, firestart.transform.rotation);
           

        }
    }

