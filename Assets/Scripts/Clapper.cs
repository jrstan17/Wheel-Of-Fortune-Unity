using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clapper : MonoBehaviour {

    public AudioSource ClapSource;
    public AudioClip ClapStart;
    public AudioClip ClapSustain;
    public AudioClip ClapFinish;

    public IEnumerator Play() {
        ClapSource.clip = ClapStart;
        ClapSource.Play();
        yield return new WaitForSeconds(ClapStart.length);

        ClapSource.loop = true;
        ClapSource.clip = ClapSustain;
        ClapSource.Play();
    }

    public void Stop() {
        ClapSource.loop = false;
        ClapSource.clip = ClapFinish;
        ClapSource.Play();
    }
}
