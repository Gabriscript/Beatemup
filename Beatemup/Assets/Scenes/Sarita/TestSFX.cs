using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSFX : MonoBehaviour {
    AudioSource source;
    public KeyCode key;
    // Start is called before the first frame update
    void Start() {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(key)) {
            source.PlayOneShot(source.clip);
        }
    }
}
