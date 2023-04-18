using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomizedClip : MonoBehaviour
{
    public AudioSource source;
    public AudioClip[] clips;
    public bool  RandomizePitch;
    public bool RandomizeVolume;

    public void Play() {
        if (clips.Length == 0) {
            Debug.LogError("Add audio clips in inspector!");
            return;
        }

        var clip = clips[Random.Range(0, clips.Length)];
        source.pitch = Random.Range(-1f, 2f);
        source.volume = Random.Range(0.5f , 1f);
        source.PlayOneShot(clip);

      /*  if(RandomizePitch) {


        }
        if(RandomizeVolume)*/
    } 
}
