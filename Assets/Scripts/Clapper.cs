using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clapper : MonoBehaviour {

    internal AudioTracks AudioTracks;

    private void Start() {
        AudioTracks = gameObject.GetComponent<AudioTracks>();
    }

    public IEnumerator Play() {
        AudioTracks.Play("clap_start");
        yield return new WaitForSeconds(AudioTracks.ClapStart.length - 0.1f);
        AudioTracks.Play("clap_sustain");
    }

    public IEnumerator PlayFor(float seconds) {
        seconds += AudioTracks.ClapStart.length;
        yield return Play();
        yield return new WaitForSeconds(seconds);
        yield return Stop();
    }

    public IEnumerator Stop() {
        AudioSource AudioSource = AudioTracks.GetSource("clap_sustained");
        AudioSource.loop = false;

        float remaining = AudioSource.clip.length - AudioSource.time;
        yield return new WaitForSeconds(remaining - 0.025f);

        AudioTracks.Play("clap_finish");
    }
}
