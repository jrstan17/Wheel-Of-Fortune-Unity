using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTracks : MonoBehaviour {

    public AudioClip PuzzleReveal;
    public AudioClip LetterReveal;
    public AudioClip Buzzer;
    public AudioClip RoundWin;
    public AudioClip Bankrupt;
    public AudioClip Oh;
    public AudioClip Remains;

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
        } else if (name.Equals("bankrupt")) {
            AudioSource.clip = Bankrupt;
        } else if (name.Equals("oh")) {
            AudioSource.clip = Oh;
        } else if (name.Equals("remains")) {
            AudioSource.clip = Remains;
        } else {
            return;
        }

        AudioSource.Play();
    }
}
