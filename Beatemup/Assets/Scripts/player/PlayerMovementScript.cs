using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour , IDamageable {  
    [Header("movement")]
    public float moveSpeed;
    public float groundDrag;
    public float jumpForce;
    public float jumpCooldown;
    public float airmultiplier;
    bool readyToJump = true;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("ground check")]
    public float playerHeight;
    public LayerMask whatisGround;
    public bool grounded;

    [Header("hits")]
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
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playState = FindObjectOfType<PlayStateController>();
    }
    void Update() {
        grounded = Physics.Raycast(transform.position, Vector3.down, 0.05f, whatisGround);
        Debug.DrawRay(transform.position,Vector3.down,Color.red,1f);
        MyInput();
        

        if (grounded) {
            rb.drag = groundDrag;

        } else {
            rb.drag = 0;
        }

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

    void FixedUpdate() {
        moverPlayer();
        speedcontrol();
    }
    public void MyInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");



        if (Input.GetKey(jumpKey) && readyToJump && grounded) {
            readyToJump = false;

            Jump();

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

            Debug.Log("-1lifepoint");
            rb.AddForce(hit.push);
            //hit.push;
        }
        
       


        if (currentHealth <= 0) {
            Debug.Log("I´m dead!");
            //Die();
        }
    }
}
