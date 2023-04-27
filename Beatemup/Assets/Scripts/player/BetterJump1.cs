using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterJump1 : MonoBehaviour
{
    Rigidbody rb;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplayer = 2f;
    // Start is called before the first frame update
    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (rb.velocity.y < 0) {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space)) {
            rb.velocity += Vector3.up * Physics.gravity.y * (lowJumpMultiplayer - 1) * Time.deltaTime;
        }
    }
}
