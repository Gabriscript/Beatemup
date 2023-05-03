using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Rendering;

public class PlayerMovementScript : MonoBehaviour , IDamageable {  
    [Header("movement")]
    public float moveSpeed;
    public float groundFriction;
    public float jumpForce;
    public float jumpCooldown;
    public float airmultiplier;
    bool readyToJump = true;
    //public  BoxCollider coll;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("ground check")]
    public float playerHeight;
    public LayerMask whatisGround;
    public bool grounded;
    public bool CanClimbStairs = false;

    [Header("hits")]
    public UIhealthbar healthbar;
    public Collider hitbox;
    public float stunduration;
   public  int maxHealth = 5;
    public int currentHealth;
    [HideInInspector]
    public bool hitted = false;

    public Transform orientation;
    public comboManger Combomanger;
    public Transform ghostCamera;

    [Header("GFX")]
    public float GFXrotationSpeed;
    public Transform playerObj;
    public Animator animator;
    float horizontalInput;
    float verticalInput;
    Vector3 moveDirection;
    PlayStateController playState;

    Rigidbody rb;

    void Start() {
        currentHealth = maxHealth;
        healthbar.SetMaxHealth(maxHealth);
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

       Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        playState = FindObjectOfType<PlayStateController>();
        //coll = GetComponent<BoxCollider>();
    }

    void FixedUpdate() {
        //  grounded = Physics.Raycast(transform.position, Vector3.down,0.2f, whatisGround);
        moverPlayer();
        speedcontrol();

        var startPos = transform.position + Vector3.up;
        var size = Vector3.one * 0.5f;


        if (grounded = Physics.BoxCast(startPos, size, Vector3.down, out RaycastHit hit, Quaternion.identity, 0.5f, whatisGround)) {
            rb.position += (0.5f - hit.distance) * Vector3.up;
            var v = rb.velocity;
            v.y = 0;
            var frictionDir = -v.normalized;
            var frictionAmount = Mathf.Clamp(groundFriction * 9.81f * Time.deltaTime, 0, v.magnitude);
            v += frictionDir * frictionAmount;

            rb.velocity = v;

            // rb.velocity = Vector3.ProjectOnPlane(rb.velocity, Vector3.up);

        }
        Debug.DrawRay(transform.position, Vector3.down, Color.red, 1f);


        //bool chestcheck = Physics.Raycast(transform.localPosition + Vector3.up , rb.velocity, 1f);
        // bool feetcheck = Physics.Raycast(transform.localPosition + 0.1f * Vector3.up, rb.velocity, 0.5f);

        // if (!chestcheck && feetcheck) {

        //     CanClimbStairs = true;
        // }else {
        //     CanClimbStairs = false;
        // }

        // Debug.DrawRay(transform.position, Vector3.forward, Color.red, 1f);

    }
    void Update() {

        MyInput();
       


        //if (grounded) {
        //    rb.drag = groundDrag;
         

        //} else {
        //    rb.drag = 0;
        //}

        Vector3 viewDir = Vector3.ProjectOnPlane(ghostCamera.forward, Vector3.up);
        orientation.forward = viewDir.normalized;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
        {
           playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * GFXrotationSpeed);
        }
    }


    
    public void MyInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");



        if (Input.GetKey(jumpKey) && readyToJump && grounded) {
            readyToJump = false;

            Jump();
            animator.Play("jump");
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    public void moverPlayer() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        if (moveDirection != Vector3.zero) {
            animator.SetBool("running", true);
            if (grounded) {
              rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            } else if (!grounded) {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airmultiplier, ForceMode.Force);

            }
        } else {
            animator.SetBool("running", false);
        }




    }

    public void dash(float multiplier)
    {
        Vector3 dashdistance= playerObj.forward*multiplier;
        rb.AddForce(dashdistance, ForceMode.Impulse);

    }

    public void speedcontrol() {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed) {
         Vector3 limitedvel = flatVel.normalized * moveSpeed;
          rb.velocity = new Vector3(limitedvel.x, rb.velocity.y, limitedvel.z);
        }
    }
    public void Jump() {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    public void ResetJump() {
        readyToJump = true;
    }

    public void OnTriggerEnter(Collider col) {
        var c = col.GetComponent<IDamageable>();
        if (c != null) {
            print("swordcollision");
            
            c.TakeDamage(new HitData(1,Vector3.back));
           



            animator.SetBool("hit", true);
            StartCoroutine(stun());
        }
    }
    IEnumerator stun() {

        yield return new WaitForSeconds(stunduration);
        animator.SetBool("hit", false);
    }
    public void TakeDamage(HitData hit) {
       
        if (playState.attack) hitted = false; else hitted = true;

        if (hitted) 
        {
            currentHealth -= hit.damage;
            healthbar.SetHealth(currentHealth);
            Debug.Log("-1lifepoint");
            rb.AddForce(hit.push);
           
        }
        
       


        if (currentHealth <= 0) {
            Debug.Log("I´m dead!");
          //  Maintheme.Stop();
          //  die.Play();
          Invoke("CallGameOver", 3);
        }
    }

  /* private  bool ClimbStair() {
       return Physics.BoxCast(coll.bounds.center,coll.bounds.size,Vector3.down,Quaternion.identity,1f,whatisGround);
    }*/
    public void CallGameOver() {
        FindObjectOfType<GameOver>().GameOverfunction();
    }
}
