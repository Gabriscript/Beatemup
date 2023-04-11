using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;
using static UnityEngine.UI.Image;

public class EnemyProjectile : MonoBehaviour {

    public float speed;
    Rigidbody rb;
    public GameObject muzzle;
    public GameObject hit;
    PlayStateController player;






    void Start() {
        player = FindObjectOfType<PlayStateController>();

        rb = GetComponent<Rigidbody>();
        var muzzlevfx = Instantiate(muzzle, transform.position, Quaternion.identity);
        muzzlevfx.transform.forward = gameObject.transform.forward;
        var particlemuzzle = muzzlevfx.transform.GetChild(0).GetComponent<ParticleSystem>();
        Destroy(muzzlevfx, particlemuzzle.main.duration);




    }


    void FixedUpdate() {



        if (speed != 0 && rb != null) {

            rb.MovePosition(rb.position + transform.forward * speed * Time.deltaTime);

        }
        Destroy(gameObject, 5);

    }

    private void OnCollisionEnter(Collision collision) {


        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, contact.normal);
        Vector3 pos = contact.point;
        var hitvfx = Instantiate(hit, pos, rot);
        var particlehit = hitvfx.transform.GetChild(0).GetComponent<ParticleSystem>();
      

        var c = collision.collider.GetComponent<IDamageable>();

        if (player.hittablestate == PlayStateController.Hittablestate.normal) {


            if (c != null) {


                c.TakeDamage(new HitData(1, Vector3.back));


            }
        } else if (player.hittablestate == PlayStateController.Hittablestate.attacking) {
            if (c != null) {


                c.TakeDamage(new HitData(0));
                Instantiate(Resources.Load<GameObject>("VFX/VFXPrefab/Sparks_vfx"), pos, rot);

            }
           

            Destroy(hitvfx, particlehit.main.duration);
            Destroy(gameObject);

        }

    }
}

    

  


