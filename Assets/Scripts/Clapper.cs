using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clapper : MonoBehaviour {

    public AudioSource ClapSource;
    public AudioClip ClapStart;
    public AudioClip ClapSustain;
    public AudioClip ClapFinish;

    public IEnumerator Play() {
        if (ClapSource.clip != ClapSustain) {
            ClapSource.clip = ClapStart;
            ClapSource.Play();
            yield return new WaitForSeconds(ClapStart.length);
        }

        ClapSource.loop = true;
        ClapSource.clip = ClapSustain;
        ClapSource.Play();
    }

    public IEnumerator PlayFor(float seconds) {
        if (ClapSource.clip == ClapSustain && ClapSource.isPlaying) {
            seconds += ClapStart.length;
        }

        yield return Play();
        yield return new WaitForSeconds(seconds);
        Stop();
    }

    public void Stop() {
        ClapSource.loop = false;
        ClapSource.clip = ClapFinish;
        ClapSource.Play();
    }
}
