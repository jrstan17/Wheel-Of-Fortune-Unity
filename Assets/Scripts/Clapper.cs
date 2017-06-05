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
        yield return new WaitForSeconds(seconds - 0.1f);
        yield return Stop();
    }

    public IEnumerator Stop() {
        AudioSource AudioSource = AudioTracks.GetSource("clap_sustained");
        AudioSource.loop = false;

        while (AudioSource.isPlaying) {
            yield return 0;
        }

        AudioTracks.Play("clap_finish");
    }
}
