using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTracks : MonoBehaviour {

    public AudioClip PuzzleReveal;
    public AudioClip LetterReveal;
    public AudioClip Buzzer;
    public AudioClip RoundWin;

    AudioSource AudioSource;

    void Start() {
        AudioSource = gameObject.GetComponent<AudioSource>();
    }

    public void Play(string name) {
        if (name.Equals("reveal")) {
            AudioSource.clip = PuzzleReveal;
        } else if (name.Equals("ding")) {
            AudioSource.clip = LetterReveal;
        } else if (name.Equals("buzzer")) {
            AudioSource.clip = Buzzer;
        } else if (name.Equals("round_win")) {
            AudioSource.clip = RoundWin;
        } else {
            return;
        }

        AudioSource.Play();
    }
}
