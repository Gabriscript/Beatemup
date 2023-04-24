using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum EnemyGiantStates { Idle, Walk, Fight, Death }

public class EnemyGiant : MonoBehaviour, IDamageable {
    EnemyGiantStates myStases;
    LayerMask visibilityBlockers;


    SkinnedMeshRenderer[] skinmesh;
    NavMeshAgent enemy;
    Transform player;
    [HideInInspector]
    public UIhealthbar healthbar;
    Animator anim;
    AudioSource audiosource;
    public GameObject crack;
    public GameObject cracktwo;


    [SerializeField] private AudioClip MeleeSound;
    [SerializeField] private AudioClip GruntSound;





    float maxSightRange = 15f;
    float maxSightAngle = 45f;

    bool instantieted = false;
    float cd;
    int maxHealth = 5;
    int currentHealth;
    float blinkIntensity = 10;
    float blinkDuration = 0.05f;
    float blinkTimer;
    bool bloodOut = false;

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
        crack.SetActive(false);
        cracktwo.SetActive(false);




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
        if (!hit && dir.magnitude < maxSightRange && Vector3.Angle(transform.forward, dir) < maxSightAngle && !isDead) {  //player get noticed 

            if (myStases != EnemyGiantStates.Walk)
                UpDateBehaviour(EnemyGiantStates.Walk);


            FaceTarget();

            noticed = true;


        }
        if (noticed && dir.magnitude >= enemy.stoppingDistance && !isDead) {  //if player too far get chase
            FaceTarget();
            enemy.SetDestination(player.position);


        } else if (noticed && dir.magnitude <= enemy.stoppingDistance) { //if player is inside this range get attacked
            if (myStases != EnemyGiantStates.Fight)
                UpDateBehaviour(EnemyGiantStates.Fight);


            MeleeAttack();


        }
        if (isDead) {
            if (myStases != EnemyGiantStates.Death) {
                UpDateBehaviour(EnemyGiantStates.Death);


                enemy.isStopped = true;
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
            if (myStases != EnemyGiantStates.Idle)
                UpDateBehaviour(EnemyGiantStates.Idle);

        }

       /* if (currentHealth == chance) {
            if (!bloodOut)

                Blood();

        }*/

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


    void MeleeAttack() {

        enemymeleeLeft.enabled = true;
        enemymeleeRight.enabled = true;

        anim.SetInteger("AttackIndex", UnityEngine.Random.Range(0, 2));
        anim.SetTrigger("Attack");


    }
    void Blood() {

        Instantiate(Resources.Load<GameObject>("prefab/Blood"), transform.position, Quaternion.Euler(new Vector3(0, UnityEngine.Random.Range(0, 360), 0)));
        bloodOut = true;



    }
    public void TakeDamage(HitData hit) {

        if (currentHealth > 0)
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


            target.TakeDamage(new HitData(1, dir));


        }
    }

    void Die() {
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        Destroy(transform.parent.gameObject, 4.1f);


        anim.SetBool("RoboDeath", true);

    }
    void LifeSpawn() {


        var hitvfx = Instantiate(Resources.Load<GameObject>("VFX/VFXPrefab/HealthPartciles"), transform.position, Quaternion.identity);
        instantieted = true;

        Destroy(hitvfx, 10);

    }
    void UpDateBehaviour(EnemyGiantStates state) {

        myStases = state;

        switch (myStases) {

            case EnemyGiantStates.Walk:
                enemy.SetDestination(player.position);
                anim.SetBool("RoboWalk", true);
                anim.SetBool("RoboIdle", false);
                //   FindAnyObjectByType<PlayRandomizedClip>().PlayfootSteps();




                break;
                case EnemyGiantStates.Idle:

                anim.SetBool("RoboIdle", true);
                anim.SetBool("RoboWalk", false);

                break;
            case EnemyGiantStates.Fight:

                anim.SetBool("RoboIdle", false);
                anim.SetBool("RoboWalk", false);
                break;
            case EnemyGiantStates.Death:
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
    public void SpawnParticles() {

        crack.SetActive(true);
        cracktwo.SetActive(true);
        Invoke("DespawnParticles",1);
    }
    void DespawnParticles() {
        crack.SetActive(false);
        cracktwo.SetActive(false);
    }
    public void Sound() {


        audiosource.PlayOneShot(GruntSound, 0.2f);
    }
}
