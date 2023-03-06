using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
   
    public float speed;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

   
    void Update()
    {
        if (speed != 0 && rb != null)
            rb.position += transform.forward  * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {

            Vector3 dir = (rb.transform.position - collision.transform.position).normalized;
           collision.gameObject.GetComponent<Rigidbody>().AddForce(-dir ,ForceMode.Impulse);
        }


            Destroy(gameObject);
        
    }
}
