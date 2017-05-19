using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelClickPlayer : MonoBehaviour {

    public AudioSource[] Clicks;

    public void Play() {
        int index;

        do {
            index = Random.Range(0, Clicks.Length);
        } while (Clicks[index].isPlaying);

        Clicks[index].Play();
    }

    private void Update() {
        int playing = 0;

        foreach(AudioSource source in Clicks) {
            if (source.isPlaying) {
                playing++;
            }
        }

        if (playing == Clicks.Length) {
            Debug.LogWarning("Wheel click audio overflow!");
        }
    }
}
