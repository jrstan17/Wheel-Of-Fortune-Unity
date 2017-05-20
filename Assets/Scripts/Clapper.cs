using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clapper : MonoBehaviour {

    public AudioSource ClapSource;
    public AudioClip ClapStart;
    public AudioClip ClapSustain;
    public AudioClip ClapFinish;

    AudioSource SustainSource;
    AudioSource FinishSource;

    private void Start() {
        SustainSource = gameObject.AddComponent<AudioSource>();
        SustainSource.clip = ClapSustain;

        FinishSource = gameObject.AddComponent<AudioSource>();
        FinishSource.clip = ClapFinish;
    }

    public IEnumerator Play() {
        SustainSource.loop = true;

        ClapSource.clip = ClapStart;
        ClapSource.Play();
        yield return new WaitForSeconds(ClapStart.length - 0.1f);

        SustainSource.Play();
    }

    public IEnumerator PlayFor(float seconds) {
        if (ClapSource.clip == ClapSustain && ClapSource.isPlaying) {
            seconds += ClapStart.length;
        }

        yield return Play();
        yield return new WaitForSeconds(seconds - 0.1f);
        yield return Stop();
    }

    public IEnumerator Stop() {
        SustainSource.loop = false;

        while (SustainSource.isPlaying) {
            yield return 0;
        }

        FinishSource.clip = ClapFinish;
        FinishSource.Play();
    }
}
