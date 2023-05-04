using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endlevelthing : MonoBehaviour
{
    public float degreesPerSecond = 3f;
    public float amplitude = 0.5f;
    public float frequency = 0.5f;
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();
    void Start()
    {
        
    }
    private void Update() {
      //  Floating();
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other) {
        
            FindObjectOfType<GameOver>().GameOverfunction();
        
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
