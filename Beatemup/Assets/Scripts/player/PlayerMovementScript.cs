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
    int health = 3;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    public Animator animator;

    Vector3 moveDirection;

    Rigidbody rb;

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

    }
    void Update() {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatisGround);
        MyInput();
        

        if (grounded) {
            rb.drag = groundDrag;

        } else {
            rb.drag = 0;
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

    public void OnCollisionEnter(Collision col) {
        var c = col.collider.GetComponent<IDamageable>();
        if (c != null) {
            print("swordcollision");

            c.TakeDamage(new HitData(1));


        
        animator.SetBool("hit", true);
            StartCoroutine(stun());
        }
    }
    IEnumerator stun() {

        yield return new WaitForSeconds(stunduration);
        animator.SetBool("hit", false);
    }
    public void TakeDamage(HitData hit) {

        health -=hit.damage;
        Debug.Log("hit");


        if (health <= 0) {
            Debug.Log("I?m dead!");
            //Die();
        }
    }
}
