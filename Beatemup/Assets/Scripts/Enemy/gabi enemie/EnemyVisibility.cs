using System;
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

enum EnemyType { Melee, Range }
enum EnemyStates { Idle, Walk, Fight }
public class EnemyVisibility : MonoBehaviour, IDamageable {
    EnemyStates myStases;
     EnemyType enemyType;
   LayerMask visibilityBlockers;

    [SerializeField] GameObject firestart;
     SkinnedMeshRenderer[] skinmesh;
     NavMeshAgent enemy;
     Transform player;
    [HideInInspector]
     public UIhealthbar healthbar; 
     Animator anim;
   

    float maxSightRange = 15f;
    float maxSightAngle = 45f;
    float timeToFire = 4f;
    bool instantieted = false;
    float cd;
     int maxHealth = 5;
    int currentHealth;
    float blinkIntensity = 10;
    float blinkDuration = 0.05f;
    float blinkTimer;
    bool bloodOut = false;
    bool coolDown = false;
    bool isDead = false;
     EnemyVisibility[] enemies;
    bool even;
    



    void Start() {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        enemy = GetComponent<NavMeshAgent>();
       player = GameObject.FindGameObjectWithTag("Player").transform;
        skinmesh = GetComponentsInChildren<SkinnedMeshRenderer>();       
        anim = GetComponent<Animator>();       
        enemies = FindObjectsOfType<EnemyVisibility>();
       visibilityBlockers = LayerMask.GetMask("VisibilityBlocker");

        

        for (int i = 0; i < enemies.Length; i++) {

            even = i % 2 == 0;

            if (even) enemies[i].enemyType = EnemyType.Melee; else enemies[i].enemyType = EnemyType.Range;

        }

    }

    void Update() {

      //TODO    projectile ignore layer of other enemy but when reflect change layer 
          //TODO     fix animation
      //TODO     fix the life energy drop
      //TODO fading out after death





        // blinking effect
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intesity = (lerp * blinkIntensity) ;

        foreach (var rend in skinmesh) {
           rend.material.color = Color.white * intesity;
        }
      

       

        // checking where the player is
        var origin = transform.position + 0.5f * Vector3.up;
        var targetPos = player.position + 0.5f * Vector3.up;
        var dir = targetPos - origin;
        bool hit = Physics.Raycast(origin, dir, dir.magnitude, visibilityBlockers);
        if (!hit && dir.magnitude < maxSightRange && Vector3.Angle(transform.forward, dir) < maxSightAngle && dir.magnitude > enemy.stoppingDistance) {

            if (myStases != EnemyStates.Walk)
                UpDateBehaviour(EnemyStates.Walk);


            FaceTarget();




        } else if (!hit && dir.magnitude < maxSightRange && Vector3.Angle(transform.forward, dir) < maxSightAngle && dir.magnitude <= enemy.stoppingDistance) {

            if (myStases != EnemyStates.Fight)
                UpDateBehaviour(EnemyStates.Fight);
                              


            if (enemyType == EnemyType.Melee) {

                enemy.stoppingDistance = 1;

                MeleeAttack();

            }
             if (enemyType == EnemyType.Range) {

                enemy.stoppingDistance = 10;

                anim.ResetTrigger("EnemyAttack");

                anim.SetTrigger("EnemyShoot");

                if (dir.magnitude < 2)
                    MeleeAttack();
                else if (coolDown == true && dir.magnitude > 2) {

                    Shoot();

                }
            }
          

            FaceTarget();

        } else {
            if (myStases != EnemyStates.Idle)
                UpDateBehaviour(EnemyStates.Idle);

        }


        var chance = UnityEngine.Random.Range(1, maxHealth - 1);
        if (currentHealth == chance) {
            if(!bloodOut)

            Blood();
           
        }
        if (isDead) {
          
            Die();
            
                if (!instantieted && chance == 1) {
                LifeSpawn();
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

         
            Instantiate(Resources.Load<GameObject>("prefab/TempProjectile"), firestart.transform.position, firestart.transform.rotation);
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



        anim.ResetTrigger("EnemyShoot");
        anim.SetTrigger("EnemyAttack");
        
      
    }
    void Blood() {
        
            Instantiate(Resources.Load<GameObject>("VFX/VFXPrefab/BloodVFX"), transform.position, Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(0, 360), 0)));
        bloodOut = true;
            
            Debug.Log("bloood");
        

    }
    public void TakeDamage(HitData hit) {
        anim.ResetTrigger("EnemyAttack");
        anim.ResetTrigger("EnemyShoot");
       
        anim.SetTrigger("EnemyGetHit");
        blinkTimer = blinkDuration; //reset timer
       
        currentHealth -= hit.damage;
        healthbar.SetHealth(currentHealth);
        Debug.Log("enemy attacked");
        if (currentHealth == 0) {
            isDead = true;
        

        }
      

    }


    void OnTriggerEnter(Collider collision) {

        PlayerMovementScript target = collision.GetComponent<PlayerMovementScript>();
        if (target != null) {
            target.TakeDamage(new HitData(1, Vector3.back));
        }
    }

    void Die() {
      //var  FadeTime = (Time.time - FadeStartTime) * FadeSpeed;
        //TimerColor.a = Mathf.SmoothStep(1, 0, FadeTime);
       
        Destroy(transform.parent.gameObject, 8);
        foreach (var rend in skinmesh) {
          //  rend.material.color.a = 
                
                
                }

         anim.SetBool("Death",true);

    }
    void LifeSpawn() {
      
       

            var hitvfx = Instantiate(Resources.Load<GameObject>("VFX/VFXPrefab/HealthPartciles"), transform.position, Quaternion.identity);
            instantieted = true;

            Destroy(hitvfx, 10);




    }
    void UpDateBehaviour(EnemyStates state) {

        myStases = state;

        switch (myStases) {

            case EnemyStates.Walk:
                enemy.SetDestination(player.position);
                anim.SetBool("EnemyWalk", true);
                anim.SetBool("EnemyIdle", false);


                break;
            case EnemyStates.Idle:

                anim.SetBool("EnemyIdle", true);
                anim.SetBool("EnemyWalk", false);

                break;
            case EnemyStates.Fight:

                anim.SetBool("EnemyIdle", false);
                anim.SetBool("EnemyWalk", false);
                break;

        }

    }

}
