using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioAmbienceSwitch : MonoBehaviour
{ GameOver gameoverscript;
    PauseMenu pausemenuscript;
    AudioSource audioambience;
    public AudioMixerSnapshot snapshotenter, snapshotexit;
    public float transitionSeconds;

    public void TransitionEnter() {
        snapshotenter.TransitionTo(transitionSeconds);
    }
    public void TransitionExite() {
        snapshotexit.TransitionTo(transitionSeconds);
    }
    private void Start() {
       gameoverscript = FindObjectOfType<GameOver>();
        pausemenuscript = FindObjectOfType<PauseMenu>();
        audioambience = GetComponent<AudioSource>();
    }
    private void Update() {
        if (gameoverscript.GameIsOver == true || pausemenuscript.gameIspause == true) {
            audioambience.volume = 0.02f;
            GetComponentInChildren<AudioSource>().volume = 0.02f;
        } else {
            audioambience.volume = 0.1f;
            GetComponentInChildren<AudioSource>().volume = 0.1f;
        }

    }
}
