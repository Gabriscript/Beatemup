using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyType {Melee , Range}
public class EnemyVisibility : MonoBehaviour {
    [SerializeField] EnemyType enemyType;

    public LayerMask visibilityBlockers;
    Transform player;
    public float maxSightRange = 15f;
    public float maxSightAngle = 45f;
    NavMeshAgent enemy;
    void Start() {
        enemy = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

   
    void Update() {

        

        var origin = transform.position + 0.5f * Vector3.up;
        var targetPos = player.position + 0.5f * Vector3.up;
        var dir = targetPos - origin;
        bool hit = Physics.Raycast(origin, dir, dir.magnitude, visibilityBlockers);
        if (!hit && dir.magnitude < maxSightRange && Vector3.Angle(transform.forward, dir) < maxSightAngle) {


          



            enemy.SetDestination(player.position);
            if (enemyType == EnemyType.Melee) {
                if (dir.magnitude <= enemy.stoppingDistance) {
                 //attack
                    FaceTarget();
                }
            }
            if (enemyType == EnemyType.Range) {
                if (dir.magnitude <= enemy.stoppingDistance+20f) {
                   //attack
                    FaceTarget();
                }
            }


        }
    }
    void FaceTarget() {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRootation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRootation, Time.deltaTime * 5);
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, maxSightRange);

    }
}
