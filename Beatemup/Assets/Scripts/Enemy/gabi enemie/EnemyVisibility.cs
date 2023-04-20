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
using static Unity.VisualScripting.StickyNote;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.DebugUI.Table;

enum EnemyType { Melee, Range }
enum EnemyStates { Idle, Walk, Fight,Death }
public class EnemyVisibility : MonoBehaviour, IDamageable {
    EnemyStates myStases;
    EnemyType enemyType;
   LayerMask visibilityBlockers;

    [SerializeField] 
    GameObject firestart;
     SkinnedMeshRenderer[] skinmesh;
     NavMeshAgent enemy;
     Transform player;
    [HideInInspector]
     public UIhealthbar healthbar; 
     Animator anim;
    AudioSource audiosource;
    
    [SerializeField] private AudioClip ShootSound;
    [SerializeField] private AudioClip MeleeSound;





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
    float chance;
    bool noticed = false;
    float fadingTime= 1f;
    public Collider enemymelee;


    public comboManger ComboManger;


    void Start() {
        audiosource = GetComponent<AudioSource>();
        healthbar = GetComponentInChildren<UIhealthbar>();
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        enemy = GetComponent<NavMeshAgent>();
       player = GameObject.FindGameObjectWithTag("Player").transform;
        skinmesh = GetComponentsInChildren<SkinnedMeshRenderer>();       
        anim = GetComponent<Animator>();       
        enemies = FindObjectsOfType<EnemyVisibility>();
       visibilityBlockers = LayerMask.GetMask("VisibilityBlocker");
        chance = UnityEngine.Random.Range(1, maxHealth-1);
        enemymelee.enabled = false;


        for (int i = 0; i < enemies.Length; i++) {

            even = i % 2 == 0;

            if (even) enemies[i].enemyType = EnemyType.Melee; else enemies[i].enemyType = EnemyType.Range;

        }

    }

    void Update() {
           
         
        // blinking effect
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intesity = (lerp * blinkIntensity) ;

        var color = Color.white * intesity;
        color.a = 1;
        foreach (var rend in skinmesh) {
          rend.material.color = color;
        }
      
              

        // checking where the player is
        var origin = transform.position + 0.5f * Vector3.up;
        var targetPos = player.position + 0.5f * Vector3.up;
        var dir = targetPos - origin;
        bool hit = Physics.Raycast(origin, dir, dir.magnitude, visibilityBlockers);
        if (!hit && dir.magnitude < maxSightRange && Vector3.Angle(transform.forward, dir) < maxSightAngle && !isDead) {  //player get noticed 

            if (myStases != EnemyStates.Walk)
                UpDateBehaviour(EnemyStates.Walk);

          
            FaceTarget();
         
            noticed = true;

          
        }
        if (noticed && dir.magnitude >= enemy.stoppingDistance && !isDead) {  //if player too far get chase
            FaceTarget();
            enemy.SetDestination(player.position);
          

        } else if (noticed && dir.magnitude <= enemy.stoppingDistance) { //if player is inside this range get attacked
            if (myStases != EnemyStates.Fight)
                UpDateBehaviour(EnemyStates.Fight);



            if (enemyType == EnemyType.Melee) {

                enemy.stoppingDistance = 2;
                if (dir.magnitude < 2)
                    //  anim.SetTrigger("EnemyAttack");
                    MeleeAttack();
                //Invoke("DisableAttack", 0.3f);

            } else if (enemyType == EnemyType.Range) {

                enemy.stoppingDistance = 10;

                anim.ResetTrigger("EnemyAttack");

                anim.SetTrigger("EnemyShoot");

                if (dir.magnitude <= 2) {

                    MeleeAttack();
                   // Invoke("DisableAttack",0.3f);
                } else if (coolDown == true && dir.magnitude > 2) {

                    Shoot();

                }
            }

                

        }
        if(isDead){
            if (myStases != EnemyStates.Death) {
                UpDateBehaviour(EnemyStates.Death);

               
                enemy.isStopped = true;
            }

            fadingTime -= Time.deltaTime*0.2f;
            foreach (var rend in skinmesh) {
                rend.material.color = new Color(0,0,0,fadingTime); 
            }

            if (!instantieted && chance == 1) {
                cd += Time.deltaTime;
                if (cd >= 3)
                    LifeSpawn();
            }

        }             
                
       if(!noticed) {
            if (myStases != EnemyStates.Idle)
                UpDateBehaviour(EnemyStates.Idle);

        }
      
        if (currentHealth == chance) {
            if(!bloodOut)

            Blood();
           
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

          
            Instantiate(Resources.Load<GameObject>("prefab/Enemybullet"), firestart.transform.position, firestart.transform.rotation);
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

        enemymelee.enabled = true;
      
        anim.ResetTrigger("EnemyShoot");
        anim.SetTrigger("EnemyAttack");
        anim.speed = 1;
      
    }
    void Blood() {
        
            Instantiate(Resources.Load<GameObject>("prefab/Blood"), transform.position, Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(0, 360), 0)));
        bloodOut = true;
            
                  

    }
    public void TakeDamage(HitData hit) {
       // anim.ResetTrigger("EnemyAttack");
        //anim.ResetTrigger("EnemyShoot");
        anim.speed = 1;
        anim.SetTrigger("EnemyGetHit");
        blinkTimer = blinkDuration; //reset timer
        ComboManger.currenthits += 1;
    
        currentHealth -= hit.damage;
        healthbar.SetHealth(currentHealth);
        Debug.Log("enemy attacked");
        if (currentHealth == 0) {
            isDead = true;
        

        }
      

    }


    void OnTriggerEnter(Collider collision) {
        var origin = transform.position + 0.5f * Vector3.up;
        var targetPos = player.position + 0.5f * Vector3.up;
        var dir = targetPos - origin;

        PlayerMovementScript target = collision.GetComponent<PlayerMovementScript>();
        if (target != null) {
            
            
                target.TakeDamage(new HitData(1, dir*100));


        }
    }

    void Die() {
        GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);                   

        Destroy(transform.parent.gameObject, 4.1f);        
           

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
             //   FindAnyObjectByType<PlayRandomizedClip>().PlayfootSteps();




                break;
            case EnemyStates.Idle:

                anim.SetBool("EnemyIdle", true);
             anim.SetBool("EnemyWalk", false);
               
                break;
            case EnemyStates.Fight:

                anim.SetBool("EnemyIdle", false);
                anim.SetBool("EnemyWalk",false);
                break;
            case EnemyStates.Death:
                GetComponent<CapsuleCollider>().enabled = false;
                Die();
                break;

        }

    }
    public void PlayAttackeffect() {
        audiosource.PlayOneShot(MeleeSound, 0.2f);
    }
    public void PlayShooteffect() {
        audiosource.PlayOneShot(ShootSound, 0.2f);
    }
    public  void DisableAttack() {

        enemymelee.enabled = false;
       

    }

}
