using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour {
   
    void Start() {
       
    }

    // Update is called once per frame
    void Update() {

    }
    private void OnCollisionEnter(Collision collision) {
       
        var c = collision.collider.GetComponent<IDamageable>();
        if (c != null) {
            print("swordcollision");
           
            c.TakeDamage(new HitData(1));
            
       
        }

    
    }
}
