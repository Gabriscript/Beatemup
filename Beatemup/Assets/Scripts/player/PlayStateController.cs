using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayStateController : MonoBehaviour
{
    float blinkIntensity = 10;
    float blinkDuration = 0.05f;
    float blinkTimer;
    public  bool attack = false;
    SkinnedMeshRenderer[] skinmesh;
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
        skinmesh = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

   
    void FixedUpdate()


    {

          //TODO during combo attack state is linked
        



    }

    private void Update() {
      /*  blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intesity = (lerp * blinkIntensity);

        var color = Color.white * intesity;
        color.a = 1;
        foreach (var rend in skinmesh) {
            rend.material.color = color;
        }*/



        if (attack) {
           
            if (hittablestate != Hittablestate.attacking)
                UpDateBehaviour(Hittablestate.attacking);
        Invoke("EndAttack",1f);

        }  else if (player.hitted ) {
           

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
    public void EndAttack() {
        attack = false;
    }

    private IEnumerator Invulnerability() {

        Physics.IgnoreLayerCollision(6, 14, true);
        Physics.IgnoreLayerCollision(6, 12, true);

        //blinkTimer = blinkDuration;
        print("invulnerable");

        yield return new WaitForSeconds(2);


       
        Physics.IgnoreLayerCollision(6, 14, false);
       Physics.IgnoreLayerCollision(6, 12, false);
    }



}
