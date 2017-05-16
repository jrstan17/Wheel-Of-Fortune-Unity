using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleRunner : MonoBehaviour {

    public AudioTracks MusicTracks;
    public RandomColorChanger[] ColorChangersToBoot;
    public MeWedgeReplacer JasonWedgeReplacer;

    // Use this for initialization
    void Start () {
        Cursor.visible = false;
        CursorToggler.ToggleCursor(false);
        MusicTracks.Play("theme");
        JasonWedgeReplacer.StartJasonTimer();

        foreach(RandomColorChanger rcc in ColorChangersToBoot) {
            rcc.StartColorChange();
        }
    }
}
