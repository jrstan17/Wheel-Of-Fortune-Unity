using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelClickPlayer : MonoBehaviour {

    public AudioSource[] Clicks;
    public float PitchBend;

    public void Play() {
        int index;

        do {
            index = Random.Range(0, Clicks.Length);
        } while (Clicks[index].isPlaying);

        Clicks[index].pitch += Random.Range(-PitchBend, PitchBend);
        Clicks[index].Play();
    }
}
