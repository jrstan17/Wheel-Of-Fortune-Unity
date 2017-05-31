using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleRunner : MonoBehaviour {

    public bool useQuickStart = false;

    public AudioTracks MusicTracks;
    public RandomColorChanger[] ColorChangersToBoot;
    public MeWedgeReplacer JasonWedgeReplacer;

    public Camera MainCamera;
    public Camera TitleCamera;
    public RoundRunner RoundRunner;

    // Use this for initialization
    void Start () {
        if (!useQuickStart) {
            Cursor.visible = false;
            CursorToggler.ToggleCursor(false);
            MusicTracks.Play("theme");
            JasonWedgeReplacer.StartJasonTimer();

            foreach (RandomColorChanger rcc in ColorChangersToBoot) {
                rcc.StartColorChange();
            }
        } else {
            PlayerList.Players.Add(new Player("Jason"));
            PlayerList.Players.Add(new Player("Philip"));
            PlayerList.Players.Add(new Player("Leslie"));
            PlayerList.Players.Add(new Player("David"));
            //PlayerList.Players.Add(new Player("Rod"));
            //PlayerList.Players.Add(new Player("Jane"));
            //PlayerList.Players.Add(new Player("Ken"));
            //PlayerList.Players.Add(new Player("Joanne"));
            PlayerList.RandomizePlayers();

            RoundRunner.Initialize();
        }
    }
}
