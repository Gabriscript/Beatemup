using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttacksetScript : MonoBehaviour
{
    [Header("playerStatecontrollerhere!")]
    public PlayStateController playerStateController;
    public PlayerMovementScript playerMovementScript;
    [Header("attacks")]
    public Animator animator;
    [Header("attack 1")]
    public GameObject sword;
    
    public KeyCode attackButton1 = KeyCode.Mouse1;
    public float timelength;
    public float attackSpeed;
    public int CurrentCombo = 0;
    PlayerSword playerSword;
    public bool canAttack = true;
    public int recovertime;
    
    
   
    // Update is called once per frame
    void Update()
    {
        
            
            attackcheck();
            
        
        
        
        
    }
    public void attackcheck()
    {
        if (Input.GetKeyDown(attackButton1))
        {
            CurrentCombo += 1;
            if (CurrentCombo==1 && canAttack == true)
             {
            canAttack= false;
            playerMovementScript.dash(5);
            sword.SetActive(true);
            animator.Play("sword1");
            playerStateController.attack = true;
            reflcet.enabled = true;
            }

            else if (CurrentCombo == 2 && canAttack == true)
            {
            canAttack = false;
            playerMovementScript.dash(5);
            sword.SetActive(true);
            animator.Play("sword2");
            playerStateController.attack = true;
            reflcet.enabled = true;

            }
            else if (CurrentCombo == 3 && canAttack == true)
            {
            canAttack = false;
            playerMovementScript.dash(5);
            sword.SetActive(true);
            animator.Play("sword3");
            playerStateController.attack = true;
            reflcet.enabled = true;
            }

            if (CurrentCombo >= 3)
            {
            CurrentCombo= 0;
            duration(recovertime);
            canAttack = true;
            }
        }
       


        
        
    }
    IEnumerator duration(float duration)
    {

        
        yield return new WaitForSeconds(duration);
        canAttack= true;
    }
    void Deactivatecollider()
    {
        sword.SetActive(false);


        reflcet.enabled = false;

    }
    void CanAttack()
    {
        canAttack = true;
    }
    public Collider reflcet;
        void Start()
        {
        playerStateController = FindObjectOfType<PlayStateController>();

         playerSword = FindObjectOfType<PlayerSword>();
        }
    
}

