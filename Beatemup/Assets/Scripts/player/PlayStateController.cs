using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class PlayStateController : MonoBehaviour
{
    public enum BattleState
    {
        Passive,
        attacking,
    }
    public BattleState battleState = BattleState.Passive;
    public enum Hittablestate
    {
        normal,
        GotHit,
        attacking
    }
    public Hittablestate hittablestate = Hittablestate.normal;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (hittablestate) 
        {
        case Hittablestate.normal:

                if(Input.GetKey(KeyCode.Mouse0))
                {
                    hittablestate= Hittablestate.attacking;
                }
                break;
        case Hittablestate.attacking:


                break;
        case Hittablestate.GotHit:


                break;
        }
    }

}
