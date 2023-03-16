using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class EnemyProjectile : MonoBehaviour {

    public float speed;
    Rigidbody rb;
    public GameObject muzzle;
    public GameObject hit;
    bool reflected = false;
    public GameObject[] enemy;
    
    void Start() {
        enemy = GameObject.FindGameObjectsWithTag("enemie");
        rb = GetComponent<Rigidbody>();
        var muzzlevfx = Instantiate(muzzle, transform.position, Quaternion.identity);
        muzzlevfx.transform.forward = gameObject.transform.forward;
        var particlemuzzle = muzzlevfx.transform.GetChild(0).GetComponent<ParticleSystem>();
        Destroy(muzzlevfx, particlemuzzle.main.duration);
    }


    void Update() {
        if (speed != 0 && rb != null)
            rb.position += transform.forward * speed * Time.deltaTime;
        Destroy(gameObject, 5);
       
    }

    private void OnCollisionEnter(Collision collision) {
        if (Input.GetKey(KeyCode.R)) {

            var origin = transform.position + 0.5f * Vector3.up;
            var targetPos = enemy[0].transform.position + 0.5f * Vector3.up;
            var dir = targetPos - origin;
            bool hit = Physics.Raycast(origin, dir, dir.magnitude);
            if (hit && Vector3.Angle(transform.forward, dir) > 45) {
                rb.position += transform.forward * speed * Time.deltaTime;
                print("going back to enemie");
            } else if (hit && Vector3.Angle(transform.forward, dir) < 45)
                rb.position += transform.forward * speed * Time.deltaTime;
            print("going elsewhere");
        } else {




            var c = collision.collider.GetComponentInParent<IDamageable>();
            if (c != null) {


                c.TakeDamage(new HitData(1, Vector3.back));


            }



            speed = 0;


            ContactPoint contact = collision.contacts[0];
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
            Vector3 pos = contact.point;
            var hitvfx = Instantiate(hit, pos, rot);
            var particlehit = hitvfx.transform.GetChild(0).GetComponent<ParticleSystem>();
            Destroy(hitvfx, particlehit.main.duration);
            Destroy(gameObject);

        }
    }
}
