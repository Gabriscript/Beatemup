using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision) {
        var c = collision.collider.GetComponent<IDamageable>();

       
            if (c != null) {


                c.TakeDamage(new HitData(1, Vector3.back));
                   

                       

            }
    }
}
