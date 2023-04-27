using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject firestart;
    AudioSource audioSource;
    [SerializeField] private AudioClip ShootSound;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            Shoot();
            audioSource.PlayOneShot(ShootSound, 0.3f);
        }
    }
    void Shoot() {
       


            Instantiate(Resources.Load<GameObject>("prefab/Playerbullet"), firestart.transform.position, firestart.transform.rotation);
           

        }
    }

