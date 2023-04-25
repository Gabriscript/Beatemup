using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScr : MonoBehaviour
{
    public bool chainAllowed = false;

    Animation anim;

    public AnimationClip[] chainAttack;

    // Update is called once per frame
    void Update()
    {
        if(chainAllowed == true)
        {
            for(int i = 0; i < chainAttack.Length; i++)
            {
                anim.clip = chainAttack[i];
                anim.Play();
            }
        }
    }

    public void ChainAttackAllowed()
    {
        chainAllowed = true;
    }

    public void ChainAttackDisable()
    {
        chainAllowed = false;
    }
}
