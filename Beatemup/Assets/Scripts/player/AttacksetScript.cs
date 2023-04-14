using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttacksetScript : MonoBehaviour
{
    [Header("playerStatecontrollerhere!")]
    public PlayStateController playerStateController;
    [Header("attacks")]
    public Animator animator;
    [Header("attack 1")]
    public Collider swordbox1;
    public KeyCode attackButton1 = KeyCode.Mouse1;
    public float timelength;
    public float attackSpeed;
    public int MaxCombo = 3;
    public int CurrentCombo = 0;


    
   
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0)) 
        {
            animator.Play("Attack");
            swordbox1.enabled = true;
            playerStateController.attack = true;
            Invoke("Deactivatecollider",0.5f);
        }
        //attackcheck();
        if (Input.GetKeyDown(KeyCode.R))
            reflcet.enabled = true;
        if (Input.GetKeyUp(KeyCode.R))
            reflcet.enabled = false;
    }
    public void attackcheck()
    {
        if (Input.GetKey(attackButton1))
        {
            animator.SetBool("attack1", true);
            duration("attack1", timelength);
            animator.SetFloat("attack1Duration", timelength);
            animator.SetBool("attack1", false);
           

        }
    }
    IEnumerator duration(string attackname, float duration)
    {

        
        yield return new WaitForSeconds(duration);
        
    }
    public Collider reflcet;
        void Start()
        {
        playerStateController = FindObjectOfType<PlayStateController>();
        }
    void Deactivatecollider() {
        swordbox1.enabled = false;

    }
}

