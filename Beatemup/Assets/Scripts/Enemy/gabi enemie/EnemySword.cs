using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour {
   
    void Start() {
       
    }

    // Update is called once per frame
    void Update() {

    }
    private void OnTriggerEnter(Collider collision) {
       
        var c = collision.GetComponent<IDamageable>();
        if (c != null) {
            print("swordcollision");
           
            c.TakeDamage(new HitData(1));
            
       
        }

    
    }
}
