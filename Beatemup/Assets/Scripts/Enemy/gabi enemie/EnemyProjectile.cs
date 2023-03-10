using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

    public float speed;
    Rigidbody rb;
    public GameObject muzzle;
    public GameObject hit;
    void Start() {
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
        print("moi");
        
        var c = collision.collider.GetComponentInParent<IDamageable>();
        if (c != null) {
            //var hitdata = new HitData();
            //hitdata.damage = 1;
            //hitdata.push = Vector3.zero;
            //c.TakeDamage(hitdata);
            print("ciao");
            c.TakeDamage(new HitData(1,Vector3.back)) ;

         // collision.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.back, ForceMode.VelocityChange);
        }



        speed = 0;
       // if (collision.gameObject.CompareTag("Player")) {

           // Vector3 dir = (rb.transform.position - collision.transform.position).normalized;
           // collision.gameObject.GetComponent<Rigidbody>().AddForce(-dir, ForceMode.Impulse);

      //  }

        ContactPoint contact = collision.contacts[0];
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 pos = contact.point;
        var hitvfx = Instantiate(hit, pos, rot);
        var particlehit = hitvfx.transform.GetChild(0).GetComponent<ParticleSystem>();
        Destroy(hitvfx, particlehit.main.duration);
        Destroy(gameObject);


    }
}
