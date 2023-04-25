using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SnapshotMixer : MonoBehaviour
{
    public AudioMixerSnapshot snapshot1;
    public AudioMixerSnapshot snapshot2;
    public Slider slider;
    public AudioMixer mixer;

    public void UpdateSnapshotMixing() {
        float snap2 = slider.value;
        float snap1 = 1 - snap2;
        mixer.TransitionToSnapshots(
            new AudioMixerSnapshot[] { snapshot1, snapshot2 },
            new float[] { snap1, snap2 },
            0);
    }
}
