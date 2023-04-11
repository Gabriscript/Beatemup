using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStateController : MonoBehaviour
{
    bool passive = true;
    PlayerMovementScript player;
    public enum BattleState
    {
        Passive,
        Active,
    }
    public BattleState battleState = BattleState.Passive;
    public enum Hittablestate
    {
        normal,
        GotHit,
        attacking
    }
    public Hittablestate hittablestate = Hittablestate.normal;


    
    void Start()
    {
        player = FindObjectOfType<PlayerMovementScript>();
    }

   
    void FixedUpdate()


    {

        if (Input.GetKey(KeyCode.U)) {
            if (hittablestate != Hittablestate.attacking)
                UpDateBehaviour(Hittablestate.attacking);

        }

            if (player.hitted) {
                if (hittablestate != Hittablestate.GotHit)
                    UpDateBehaviour(Hittablestate.GotHit);
                player.hitted = false;

            }

            if (hittablestate != Hittablestate.normal)
                UpDateBehaviour(Hittablestate.normal);
        
        





    }

    private void Update() {
      
    }


    void UpDateBehaviour(Hittablestate state) {

        hittablestate = state;

        switch (hittablestate) {

            case Hittablestate.attacking:

                break;

            case Hittablestate.GotHit:
                StartCoroutine(Invulnerability());
                break;
            case Hittablestate.normal:

                break;




        }

           


    }
    private IEnumerator Invulnerability() {

        Physics.IgnoreLayerCollision(6, 7, true);
        Physics.IgnoreLayerCollision(6, 12, true);



        yield return new WaitForSeconds(2);


       
        Physics.IgnoreLayerCollision(6, 7, false);
        Physics.IgnoreLayerCollision(6, 12, false);
    }



}
