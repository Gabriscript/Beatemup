using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;

public enum EnemyType { Melee, Range }
public class EnemyVisibility : MonoBehaviour, IDamageable {
    [SerializeField] EnemyType enemyType;
    public LayerMask visibilityBlockers;
    public GameObject projectile;
    public GameObject firestart;
    Material mat;
    NavMeshAgent enemy;
   Transform player;
    public  UIhealthbar healthbar;
    public  GameObject vfx;
    public  GameObject lifeSpawn;
    Animator anim;


     float maxSightRange = 15f;
    float maxSightAngle = 45f;
    float timeToFire = 4f;
    public bool instantieted = false;
    float cd;
     int maxHealth = 5;
    int currentHealth;
    float blinkIntensity = 10;
    float blinkDuration = 0.1f;
    float blinkTimer;
    bool bloodOut = false;
    bool coolDown = false;
    



    void Start() {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        enemy = GetComponent<NavMeshAgent>();
       player = GameObject.FindGameObjectWithTag("Player").transform;
        mat = GetComponent<Renderer>().material;
       
        anim = GetComponent<Animator>();
    

    }

    void Update() {
        // blinking effect
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intesity = (lerp * blinkIntensity) + 1;
        mat.color = Color.white * intesity;
        //skinmeshrender.material.color =...

       

        // checking where the player is
        var origin = transform.position + 0.5f * Vector3.up;
        var targetPos = player.position + 0.5f * Vector3.up;
        var dir = targetPos - origin;
        bool hit = Physics.Raycast(origin, dir, dir.magnitude, visibilityBlockers);
        if (!hit && dir.magnitude < maxSightRange && Vector3.Angle(transform.forward, dir) < maxSightAngle) {


            FaceTarget();



            enemy.SetDestination(player.position);
            if (enemyType == EnemyType.Melee) {

                if (dir.magnitude <= enemy.stoppingDistance) {

                   
                    Invoke ("MeleeAttack",4); 
                    //MeleeAttack();
                }
            }
            if (enemyType == EnemyType.Range) {

                if (dir.magnitude <= enemy.stoppingDistance && dir.magnitude > 2) {
                   
                    anim.SetTrigger("EnemyShoot");
                    if (coolDown == true ) 

                        Shoot();
                } else if (dir.magnitude <= 2) {
                    Invoke("MeleeAttack", 4);
                }


            }


        }
        

        var chance = Random.Range(1, maxHealth - 1);
        if (currentHealth == chance) {
            if(!bloodOut)

            Blood();
           
        }
        if (currentHealth <= 0) {

            Die();
            
                if (!instantieted && chance == 1) {
                Invoke("LifeSpawn", 2);
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
    void Shoot() {
       cd += Time.deltaTime;
       if (cd >= timeToFire) {

         
            Instantiate(projectile, firestart.transform.position, firestart.transform.rotation);
            anim.speed = 1f;
            coolDown = false;
           
            cd = 0;

        }
    }
    public void StartSlowdown() {
        anim.speed = 0f;
        coolDown = true;
    }
    void MeleeAttack() {

       
       
       
            anim.SetTrigger("EnemyAttack");
        
      
    }
    void Blood() {
        
            Instantiate(vfx, transform.position, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)));
        bloodOut = true;
            
            Debug.Log("bloood");
        

    }
    public void TakeDamage(HitData hit) {

        anim.SetTrigger("EnemyGetHit");
        blinkTimer = blinkDuration; //reset timer
        currentHealth -= hit.damage;
        healthbar.SetHealth(currentHealth);
        Debug.Log("enemy attacked");


      

    }


    void OnTriggerEnter(Collider collision) {

        PlayerMovementScript target = collision.GetComponent<PlayerMovementScript>();
        if (target != null) {
            target.TakeDamage(new HitData(1, Vector3.back));
        }
    }

    void Die() {
        anim.SetTrigger("EnemyFall");
        Destroy(transform.parent.gameObject, 8);
       
      

    }
    void LifeSpawn() {
      
       

            var hitvfx = Instantiate(lifeSpawn, transform.position, Quaternion.identity);
            instantieted = true;

            Destroy(hitvfx, 10);
        

        

    }

}
