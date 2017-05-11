﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleRunner : MonoBehaviour {

    public AudioTracks MusicTracks;
    public RandomColorChanger BackgroundRCC;
    public RandomColorChanger[] BoardRCCS;

	// Use this for initialization
	void Start () {
        MusicTracks.Play("theme");
        BackgroundRCC.StartColorChange();

        foreach(RandomColorChanger rcc in BoardRCCS) {
            rcc.StartColorChange();
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}