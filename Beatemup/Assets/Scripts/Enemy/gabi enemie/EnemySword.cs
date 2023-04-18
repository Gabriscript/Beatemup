using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour {
    PlayStateController player;
    Transform targ;
    public Transform enemy;
    void Start() {
        player = FindObjectOfType<PlayStateController>();
        targ = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    // Update is called once per frame
    void Update() {

    }
    private void OnTriggerEnter(Collider collision) {
           var origin = enemy.position + 0.5f * Vector3.up;
        var targetPos = targ.position + 0.5f * Vector3.up;
        var dir = targetPos - origin;
        dir.y = 0;
        dir = dir.normalized;
        var c = collision.GetComponentInParent<IDamageable>();
        
        if (player.hittablestate == PlayStateController.Hittablestate.normal) {
            
            if (c != null) {
              
                c.TakeDamage(new HitData(1,dir*100));
            }
        }else if(player.hittablestate == PlayStateController.Hittablestate.attacking) {
            if (c != null) {

                c.TakeDamage(new HitData(1,dir*100));

            }
        } 
    }
    }
