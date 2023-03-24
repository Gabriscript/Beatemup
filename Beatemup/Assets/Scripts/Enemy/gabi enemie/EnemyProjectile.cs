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
    public GameObject[] enemy;
    PlayerMovementScript player;

    


    void Start() {
        enemy = GameObject.FindGameObjectsWithTag("enemie");
        rb = GetComponent<Rigidbody>();
        var muzzlevfx = Instantiate(muzzle, transform.position, Quaternion.identity);
        muzzlevfx.transform.forward = gameObject.transform.forward;
        var particlemuzzle = muzzlevfx.transform.GetChild(0).GetComponent<ParticleSystem>();
        Destroy(muzzlevfx, particlemuzzle.main.duration);
        player = FindObjectOfType<PlayerMovementScript>();



    }


    void FixedUpdate() {

        var origin = transform.position;
        var targetPos = player.transform.position + Vector3.up;
        var dir = targetPos - origin;
        if (speed != 0 && rb != null) {
            //rb.position += transform.forward * speed * Time.deltaTime;
           // rb.MovePosition(rb.position + transform.forward * speed * Time.deltaTime);
          transform.position =  Vector3.MoveTowards(origin, targetPos, Time.deltaTime*speed);
        }
        Destroy(gameObject, 5);

    }

    private void OnCollisionEnter(Collision collision) {
       



            var c = collision.collider.GetComponentInParent<IDamageable>();
            if (c != null) {


                c.TakeDamage(new HitData(1, Vector3.back));


            }
            //spawn hitvfx
            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, contact.normal);
            Vector3 pos = contact.point;
            var hitvfx = Instantiate(hit, pos, rot);
            var particlehit = hitvfx.transform.GetChild(0).GetComponent<ParticleSystem>();
            Destroy(hitvfx, particlehit.main.duration);
            Destroy(gameObject);


        }

    }

    /* private void BounceBack(Vector3 collisionNormal) {
         rb.transform.Rotate(0, 180, 0);

         var speed = lastvelocity.magnitude;
         var bounceDirection = Vector3.Reflect(lastvelocity, collisionNormal);
         var directionToEnemy = enemy[0].transform.position - transform.position;

         var direction = Vector3.Lerp(bounceDirection, directionToEnemy, 1);

         Debug.Log("Out Direction: " + direction);
         rb.velocity = direction * speed;
     }*/

  


