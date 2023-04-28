using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerShooting : MonoBehaviour
{
    public GameObject firestart;
    public Animator animator;
    AudioSource audioSource;
    [SerializeField] private AudioClip ShootSound;
    public int CurrentCombo=0;
    public bool canShoot;
    public GameObject gun;
    public PlayStateController playerStateController;
    public int recovertime;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Shoot();
    }
    void Shoot() {
        if (Input.GetKeyDown(KeyCode.F)) {
            CurrentCombo += 1;
        if (CurrentCombo == 1 && canShoot == true)
        {
            canShoot = false;
            
            gun.SetActive(true);
            animator.Play("shoot1");
            Instantiate(Resources.Load<GameObject>("prefab/Playerbullet"), firestart.transform.position, firestart.transform.rotation);

            }

        else if (CurrentCombo == 2 && canShoot == true)
        {
            canShoot = false;
            
            gun.SetActive(true);
            animator.Play("shoot2");
            Instantiate(Resources.Load<GameObject>("prefab/Playerbullet"), firestart.transform.position, firestart.transform.rotation);


            }
        else if (CurrentCombo == 3 && canShoot == true)
        {
            canShoot = false;
            
            gun.SetActive(true);
            animator.Play("shoot3");
            Instantiate(Resources.Load<GameObject>("prefab/Playerbullet"), firestart.transform.position, firestart.transform.rotation);

            }

        if (CurrentCombo >= 3)
        {
            CurrentCombo = 0;
            duration(recovertime);
            canShoot = true;
        }
            
            
        }
        

        

        
    }
    void CanShoot()
    {
        canShoot = true;
    }
    IEnumerator duration(float duration)
    {


        yield return new WaitForSeconds(duration);
        canShoot = true;
    }
    void DeactivatecolliderShoot()
    {
        gun.SetActive(false);

    }
}

