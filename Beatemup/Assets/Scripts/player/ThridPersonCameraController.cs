using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdpersonCameraController : MonoBehaviour
{
    [Header("refrences")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;
    public PlayerMovementScript playerMovementScript;

    public float rotationSpeed;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        Vector3 viewDir = Vector3.ProjectOnPlane(player.position - transform.position, Vector3.up);

        //Vector3 view = player.position - transform.position;
        //view.z = 0;
        orientation.forward = viewDir.normalized;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
        {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}
