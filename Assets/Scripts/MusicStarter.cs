using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicStarter : MonoBehaviour {

    public AudioTracks AudioTracks;

	public void StartTheme() {
        AudioTracks.Play("theme");
    }
}
