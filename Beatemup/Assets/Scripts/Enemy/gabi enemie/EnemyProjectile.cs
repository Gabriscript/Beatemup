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
    Transform targ;






    void Start() {
        player = FindObjectOfType<PlayStateController>();
        targ = GameObject.FindGameObjectWithTag("Player").transform;
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

        var origin = transform.position + 0.5f * Vector3.up;
        var targetPos = targ.position + 0.5f * Vector3.up;
        var dir = targetPos - origin;
        dir.y = 0;
        dir = dir.normalized;
        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.forward, contact.normal);
        Vector3 pos = contact.point;
        var hitvfx = Instantiate(hit, pos, rot);
        var particlehit = hitvfx.transform.GetChild(0).GetComponent<ParticleSystem>();
      

        var c = collision.collider.GetComponentInParent<IDamageable>();

          if (player.hittablestate == PlayStateController.Hittablestate.normal) {


              if (c != null) {


                  c.TakeDamage(new HitData(1, dir));
                Debug.Log("projectile hit u");

              }
          } 

          if (player.hittablestate == PlayStateController.Hittablestate.attacking) {
              if (c != null) {

                  Debug.Log("something");
                  c.TakeDamage(new HitData(0));
                  Instantiate(Resources.Load<GameObject>("Prefab/Sparks"), pos, rot);

              }




          }
        Destroy(hitvfx, particlehit.main.duration);
        Destroy(gameObject);
    }
}

    

  


