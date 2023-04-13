using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStateController : MonoBehaviour
{
   public  bool attack = false;
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
    public Hittablestate hittablestate;


    
    void Start()
    {
        player = FindObjectOfType<PlayerMovementScript>();
    }

   
    void FixedUpdate()


    {

          
        //TODO fix the state : they have to change properly,attack does not receive projectile,reflectng projectile not working to fix



    }

    private void Update() {
        if (attack) {
            if (hittablestate != Hittablestate.attacking)
                UpDateBehaviour(Hittablestate.attacking);

        } else if (player.hitted) {
            if (hittablestate != Hittablestate.GotHit)
                UpDateBehaviour(Hittablestate.GotHit);
            Invoke("Deactivate", 2);

        } else
            UpDateBehaviour(Hittablestate.normal);
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
    void Deactivate() {
        player.hitted = false;
    }
    private IEnumerator Invulnerability() {

        Physics.IgnoreLayerCollision(6, 14, true);
        Physics.IgnoreLayerCollision(6, 12, true);



        yield return new WaitForSeconds(2);


       
        Physics.IgnoreLayerCollision(6, 14, false);
       Physics.IgnoreLayerCollision(6, 12, false);
    }



}
