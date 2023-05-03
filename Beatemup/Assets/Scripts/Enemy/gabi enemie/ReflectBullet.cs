using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectBullet : MonoBehaviour
{
    public float deflectDistance = 1f;
    public float maxAngleCenter = 90f;
    public float maxAngleSider = 180f;
    // Start is called before the first frame update
    void Awake()
    {
        var coll = GetComponent<BoxCollider>();
        coll.size = Vector3.one * (deflectDistance * 2);
       
    }

    // Update is called once per frame
    void Update()
    {
      
        
    }
    private void OnTriggerStay(Collider other) {
        

        var projectile = other.GetComponent<EnemyProjectile>();
        if(projectile != null) {
            var distance = Vector3.Distance(projectile.transform.position,transform.position);

            if (distance > deflectDistance)
                return;
            var dir = projectile.transform.position - transform.position;

            // shouldn't turn around things already going away from us
            if (Vector3.Angle(dir, other.transform.forward) < 90)
                return;

            var angle = Vector3.Angle(transform.forward,dir);

            if (angle > maxAngleSider/2 )
                return;

            var rb = other.GetComponent<Rigidbody>();
           

          if (angle <  maxAngleCenter/2) {
                rb.rotation = Quaternion.Euler(0, 180, 0) * rb.rotation; // Deflcting to enemy position
                print("back to enemies");
                projectile.gameObject.layer = 13;
            } 
          
         else if( angle < maxAngleCenter ) {

                print("casual");
                rb.rotation = Quaternion.Euler(0, Random.Range(-maxAngleCenter/2, maxAngleCenter/2), 0) * rb.rotation;    // Random deflection
                projectile.gameObject.layer = 13;     // can damage enemy changing layer      
            }

        }

    }
    private void OnDrawGizmosSelected() {
        Gizmos.DrawWireSphere(transform.position, deflectDistance);
    }
}
