using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySword : MonoBehaviour {
    Rigidbody rb;
    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {

    }
    private void OnCollisionEnter(Collision collision) {

        var c = collision.collider.GetComponent<IDamageable>();
        if (c != null) {
            c.TakeDamage(new HitData(1));
        }

      //  if (collision.gameObject.CompareTag("Player")) {

          //  Vector3 dir = (rb.transform.position - collision.transform.position).normalized;
           // collision.gameObject.GetComponent<Rigidbody>().AddForce(-dir, ForceMode.Impulse);
      //  }
    }
}
