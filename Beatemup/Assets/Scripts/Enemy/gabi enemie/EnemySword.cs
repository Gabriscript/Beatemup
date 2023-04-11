using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour {
    PlayStateController player;
    void Start() {
        player = FindObjectOfType<PlayStateController>();
    }

    // Update is called once per frame
    void Update() {

    }
    private void OnCollisionEnter(Collision collision) {
        var c = collision.collider.GetComponent<IDamageable>();

        if (player.hittablestate == PlayStateController.Hittablestate.normal) {


            if (c != null) {


                c.TakeDamage(new HitData(1, Vector3.back));


            }
        }else if(player.hittablestate == PlayStateController.Hittablestate.attacking) {
            if (c != null) {


                c.TakeDamage(new HitData(1));


            }

        }
    }
    }
