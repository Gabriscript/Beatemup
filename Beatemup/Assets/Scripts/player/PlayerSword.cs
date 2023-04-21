using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour {

    Transform player;
    public float knockbackPower = 500;
    void Start()
    {
        player = GetComponentInParent<PlayerMovementScript>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  


   private void OnTriggerEnter(Collider collision) {
         var c = collision.GetComponentInParent<IDamageable>();
         var origin = player.position + 0.5f * Vector3.up;
         var targetPos = collision.transform.position + 0.5f * Vector3.up;
         var dir = targetPos - origin;
        dir = dir.normalized;
       dir.y = 0;


        

             if (c != null ) {


                 c.TakeDamage(new HitData(1, dir));

          collision.GetComponentInParent<Rigidbody>().AddForce(dir *350);





         }
     }
}
