using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour {
    [System.Serializable]
    public struct SnapshotData {
        public AudioMixerSnapshot snapshot;
        public float transitionSeconds;
        public KeyCode debugKey;
    }

    [SerializeField] List<SnapshotData> snapshotDatas;

    void Start() {

    }

    void Update() {
        foreach (var data in snapshotDatas) {

            if (Input.GetKeyDown(data.debugKey)) {
                data.snapshot.TransitionTo(data.transitionSeconds);
            }

        }
    }
}
