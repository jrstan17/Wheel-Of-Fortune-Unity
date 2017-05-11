using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleRunner : MonoBehaviour {

    public AudioTracks MusicTracks;

	// Use this for initialization
	void Start () {
        MusicTracks.Play("theme");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
