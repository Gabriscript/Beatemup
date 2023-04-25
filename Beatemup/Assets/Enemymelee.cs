using System.Collections;
using System.Collections.Generic;
using UnityEditor.XR;
using UnityEngine;
using UnityEngine.AI;

enum EnemyMeleeStates { Idle, Walk, Fight, Death }

public class Enemymelee : MonoBehaviour, IDamageable {
    EnemyMeleeStates myStases;
    LayerMask visibilityBlockers;

   
    SkinnedMeshRenderer[] skinmesh;
    NavMeshAgent enemy;
    Transform player;
    [HideInInspector]
    public UIhealthbar healthbar;
    Animator anim;
    AudioSource audiosource;

    
    [SerializeField] private AudioClip MeleeSound;





    float maxSightRange = 15f;
    float maxSightAngle = 45f;
    bool damaged = false;
    bool instantieted = false;
    float cd;
    int maxHealth = 5;
    int currentHealth;
    float blinkIntensity = 10;
    float blinkDuration = 0.05f;
    float blinkTimer;
    bool bloodOut = false;
    float resetNavmesh = 0f;
    bool isDead = false;
   
   
    float chance;
    bool noticed = false;
    float fadingTime = 1f;
    public Collider enemymeleeLeft;
    public Collider enemymeleeRight;


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
        
        visibilityBlockers = LayerMask.GetMask("VisibilityBlocker");
        chance = UnityEngine.Random.Range(1, maxHealth - 1);
        enemymeleeLeft.enabled = false;
        enemymeleeRight.enabled = false;


        /*  for (int i = 0; i < enemies.Length; i++) {

              even = i % 2 == 0;

              if (even) enemies[i].enemyType = EnemyType.Melee; else enemies[i].enemyType = EnemyType.Range;

          }*/

    }

    void Update() {


        // blinking effect
        blinkTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(blinkTimer / blinkDuration);
        float intesity = (lerp * blinkIntensity);

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
        if (!hit && dir.magnitude < maxSightRange && Vector3.Angle(transform.forward, dir) < maxSightAngle && !isDead || damaged ) {  //player get noticed 

            if (myStases != EnemyMeleeStates.Walk)
                UpDateBehaviour(EnemyMeleeStates.Walk);


            FaceTarget();

            noticed = true;


        }
        if (noticed && dir.magnitude >= enemy.stoppingDistance && !isDead  ) {  //if player too far get chase
          

            FaceTarget();
           enemy.SetDestination(player.position);


        } else if (noticed && dir.magnitude <= enemy.stoppingDistance) { //if player is inside this range get attacked
            if (myStases != EnemyMeleeStates.Fight)
                UpDateBehaviour(EnemyMeleeStates.Fight);                             
                      
                
                    MeleeAttack();              


        }
        if (isDead) {
            if (myStases != EnemyMeleeStates.Death) {
                UpDateBehaviour(EnemyMeleeStates.Death);


               // enemy.isStopped = true;
            }

            fadingTime -= Time.deltaTime * 0.2f;
            foreach (var rend in skinmesh) {
                rend.material.color = new Color(0, 0, 0, fadingTime);
            }

            if (!instantieted && chance == 1) {
                cd += Time.deltaTime;
                if (cd >= 3)
                    LifeSpawn();
            }

        }

        if (!noticed) {
            if (myStases != EnemyMeleeStates.Idle)
                UpDateBehaviour(EnemyMeleeStates.Idle);

        }

        if (currentHealth == chance) {
            if (!bloodOut)

                Blood();

        }
       

    }
   /* private void FixedUpdate() {
        if (damaged) {
            resetNavmesh += Time.deltaTime;
            if (resetNavmesh == 0.01f) {
                gameObject.GetComponent<NavMeshAgent>().enabled = true;
                GetComponent<Rigidbody>().isKinematic = true;
                damaged = false;

                noticed = true;
            }
        }
    }
   */

    void FaceTarget() {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRootation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRootation, Time.deltaTime * 5);
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, maxSightRange);

    }
   
  
    void MeleeAttack() {

        enemymeleeLeft.enabled =true;
        enemymeleeRight.enabled = true;

        anim.SetInteger("AttackIndex", Random.Range(0,2));
       anim.SetTrigger("Attack");
      

    }
    void Blood() {

        Instantiate(Resources.Load<GameObject>("prefab/Blood"), transform.position, Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(0, 360), 0)));
        bloodOut = true;



    }
    public void TakeDamage(HitData hit) {
        /*gameObject.GetComponent<NavMeshAgent>().enabled = false;
       
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(hit.push *2, ForceMode.Impulse);*/

        damaged = true;
        if (currentHealth >0)
        anim.SetTrigger("RoboDamage");

        blinkTimer = blinkDuration; //reset timer
        ComboManger.currenthits += 1;

        currentHealth -= hit.damage;
        healthbar.SetHealth(currentHealth);
        Debug.Log("enemy attacked");
        if (currentHealth == 0) {
            isDead = true;
            Die();


        }


    }


    void OnTriggerEnter(Collider collision) {
        var origin = transform.position + 0.5f * Vector3.up;
        var targetPos = player.position + 0.5f * Vector3.up;
        var dir = targetPos - origin;


        PlayerMovementScript target = collision.GetComponent<PlayerMovementScript>();
        if (target != null) {


            target.TakeDamage(new HitData(1, dir ));


        }
    }

    void Die() {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        Destroy(transform.parent.gameObject, 4.1f);


        anim.SetBool("RoboDead", true);

    }
    void LifeSpawn() {


        var hitvfx = Instantiate(Resources.Load<GameObject>("VFX/VFXPrefab/HealthPartciles"), transform.position, Quaternion.identity);
        instantieted = true;

        Destroy(hitvfx, 10);

    }
    void UpDateBehaviour(EnemyMeleeStates state) {

        myStases = state;

        switch (myStases) {

            case EnemyMeleeStates.Walk:
                enemy.SetDestination(player.position);
                anim.SetBool("RoboWalk", true);
                anim.SetBool("RoboIdle", false);
                //   FindAnyObjectByType<PlayRandomizedClip>().PlayfootSteps();




                break;
            case EnemyMeleeStates.Idle:

                anim.SetBool("RoboIdle", true);
                anim.SetBool("RoboWalk", false);

                break;
            case EnemyMeleeStates.Fight:

                anim.SetBool("RoboIdle", false);
                anim.SetBool("RoboWalk", false);
                break;
            case EnemyMeleeStates.Death:
                GetComponent<CapsuleCollider>().enabled = false;
               
                break;

        }

    }
    public void PlayAttackeffect() {
        audiosource.PlayOneShot(MeleeSound, 0.2f);
    }
   
    public void DisableAttack() {

        enemymeleeLeft.enabled = false;
        enemymeleeRight.enabled = false;


    }
    public void StopAnimation() {
        anim.speed = 0;
    }
}
