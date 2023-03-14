using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CharacetrMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 6;

    public void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal,0f,vertical).normalized;

        if(direction.magnitude>=0.1f)
        {
            float tragetAngle = Mathf.Atan2(direction.x,direction.z)*Mathf.Rad2Deg;
            transform.rotation=quaternion.Euler(0f,tragetAngle,0f);

            controller.Move(direction*speed*Time.deltaTime); 
        }

    }
}
