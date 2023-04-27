using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class ghostCamera : MonoBehaviour
{

    public Transform player;
    public Transform childCamera;
    public float heightOffset;
    public float distance;
    public float degreeOfChild;
    public float sensitivity;
    //Vector3 lastMouseCoords;

    private void Awake()
    {
        
    }
    void Update()
    {
        //var mouseDelta = Input.mousePosition - lastMouseCoords;
        var rotInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        transform.position = player.position+Vector3.up*heightOffset;
        degreeOfChild += -rotInput.y * sensitivity;
        degreeOfChild=Mathf.Clamp(degreeOfChild, -10, 75);
        childCamera.localPosition = Quaternion.Euler(degreeOfChild,0,0)*Vector3.back* distance;
        childCamera.LookAt(transform.position);
        transform.Rotate(0,rotInput.x*sensitivity,0);
        //lastMouseCoords = Input.mousePosition;
    }

}
