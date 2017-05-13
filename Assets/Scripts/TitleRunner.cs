using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleRunner : MonoBehaviour {

    public AudioTracks MusicTracks;
    public RandomColorChanger BackgroundRCC;
    public RandomColorChanger[] BoardRCCS;
    public MeWedgeReplacer JasonWedgeReplacer;

	// Use this for initialization
	void Start () {
        Cursor.visible = false;
        CursorToggler.ToggleCursor(false);
        MusicTracks.Play("theme");
        BackgroundRCC.StartColorChange();
        JasonWedgeReplacer.StartJasonTimer();

        foreach(RandomColorChanger rcc in BoardRCCS) {
            rcc.StartColorChange();
        }
    }
}
