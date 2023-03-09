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
    public float attack1duration;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        attackcheck();
    }
    public void attackcheck()
    {
        if (Input.GetKey(attackButton1))
        {
            swordbox1.enabled = true;
            playerStateController.battleState = PlayStateController.BattleState.attacking;
            playerStateController.hittablestate = PlayStateController.Hittablestate.attacking;
            animator.SetBool("attack1", true);
            StartCoroutine(duration("attack1", attack1duration));

        }
    }
    IEnumerator duration(string attackname, float duration)
    {

        yield return new WaitForSeconds(duration);
        animator.SetBool(attackname, false);
        swordbox1.enabled = false;

    }

}

