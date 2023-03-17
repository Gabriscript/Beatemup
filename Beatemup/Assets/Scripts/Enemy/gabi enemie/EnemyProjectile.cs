using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyProjectile : MonoBehaviour {

    public float speed;
    Rigidbody rb;
    public GameObject muzzle;
    public GameObject hit;
    public GameObject[] enemy;

    Vector3 lastvelocity;


    void Start() {
        enemy = GameObject.FindGameObjectsWithTag("enemie");
        rb = GetComponent<Rigidbody>();
        var muzzlevfx = Instantiate(muzzle, transform.position, Quaternion.identity);
        muzzlevfx.transform.forward = gameObject.transform.forward;
        var particlemuzzle = muzzlevfx.transform.GetChild(0).GetComponent<ParticleSystem>();
        Destroy(muzzlevfx, particlemuzzle.main.duration);



    }


    void Update() {
        lastvelocity = rb.velocity;

        if (speed != 0 && rb != null) {
            rb.position += transform.forward * speed * Time.deltaTime;
        }
        Destroy(gameObject, 5);

    }

    private void OnCollisionEnter(Collision collision) {
        var origin = collision.contacts[0].point + 0.5f * Vector3.up;
        var targetPos = enemy[0].transform.position + 0.5f * Vector3.up;
        var dir = targetPos - origin;
        bool hit = Physics.Raycast(origin, dir, dir.magnitude);

        if (hit && Vector3.Angle(transform.forward, dir) < 90) {
            if (Input.GetKey(KeyCode.R) && collision.gameObject.CompareTag("Player")) {


                BounceBack(collision.contacts[0].normal);
                print("going back to enemie");


            }


        } else if (hit && Vector3.Angle(transform.forward, dir) > 90) {


            var c = collision.collider.GetComponentInParent<IDamageable>();
            if (c != null) {


                c.TakeDamage(new HitData(1, Vector3.back));


            }

            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            var hitvfx = Instantiate(this.hit, pos, rot);
            var particlehit = hitvfx.transform.GetChild(0).GetComponent<ParticleSystem>();
            Destroy(hitvfx, particlehit.main.duration);
            Destroy(gameObject);

        }
    }
    private void BounceBack(Vector3 collisionNormal) {
        rb.transform.Rotate(0, 180, 0);

        var speed = lastvelocity.magnitude;
        var bounceDirection = Vector3.Reflect(lastvelocity, collisionNormal);
        var directionToEnemy = enemy[0].transform.position - transform.position;

        var direction = Vector3.Lerp(bounceDirection, directionToEnemy, 1);

        Debug.Log("Out Direction: " + direction);
        rb.velocity = direction * speed;
    }

}
