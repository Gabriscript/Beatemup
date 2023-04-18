using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour {

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision) {
        var c = collision.collider.GetComponentInParent<IDamageable>();
        c.TakeDamage(new HitData(1, Vector3.back));
    }


    /* private void OnTriggerEnter(Collider collision) {
         var c = collision.GetComponentInParent<IDamageable>();
         var origin = transform.position + 0.5f * Vector3.up;
         var targetPos = collision.transform.position + 0.5f * Vector3.up;
         var dir = targetPos - origin;
        // dir = dir.normalized;
       //  dir.y = 0;




             if (c != null ) {


                 c.TakeDamage(new HitData(1, dir));

             collision.GetComponentInParent<Rigidbody>().AddForce(Vector3.left ,ForceMode.Impulse);





         }
     }*/
}
