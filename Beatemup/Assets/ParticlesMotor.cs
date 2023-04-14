using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesMotor : MonoBehaviour {

  Transform player;
    
    public float degreesPerSecond = 3f;
    public float amplitude = 0.5f;
    public float frequency = 0.5f;
    bool floating;
    
   
   
    

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    // Use this for initialization
    void Start() {
        // Store the starting position & rotation of the object
        posOffset = transform.position;
        player = GameObject.FindGameObjectWithTag("Player").transform;
//FindObjectOfType<EnemyVisibility>().instantieted = true;
    }

    // Update is called once per frame
    void Update() {
        if(floating)
        Floating();



        var origin = transform.position ;
        var targetPos = player.position +  Vector3.up;
        var dir = targetPos - origin;
        
        if (player != null) {
            if (dir.magnitude < 8f) {
                floating = false;
                transform.position = Vector3.MoveTowards(origin, targetPos, Time.deltaTime / (dir.magnitude * 0.1f));
            } else
                floating = true;
        }
           
    }
    private void OnTriggerEnter(Collider other) {
     
            FindAnyObjectByType<PlayerMovementScript>().health ++;
            Destroy(gameObject);


       
    }


    void Floating() {
        // Spin object around Y-Axis
        transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}

