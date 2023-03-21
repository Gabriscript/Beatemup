using System.Collections;
using System.Collections.Generic;
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
    public int MaxCombo = 3;
    public int CurrentCombo = 0;


    
   
    // Update is called once per frame
    void Update()
    {
        attackcheck();
        if (Input.GetKeyDown(KeyCode.R))
            reflcet.enabled = true;
        if (Input.GetKeyUp(KeyCode.R))
            reflcet.enabled = false;
    }
    public void attackcheck()
    {
        if (Input.GetKeyDown(attackButton1))
        {
            
            if (CurrentCombo<MaxCombo)
            {

                CurrentCombo += 1;
                animator.SetBool("attack1", true);

            }
            
            //StartCoroutine(duration("attack1", attack1duration));

        }
    }
    IEnumerator duration(string attackname, float duration)
    {

        yield return new WaitForSeconds(duration);
        
    }
    public Collider reflcet;
        void Start()
        {
      
        }
}

