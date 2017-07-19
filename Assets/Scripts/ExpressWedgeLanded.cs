using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpressWedgeLanded : MonoBehaviour {

    RoundRunner RoundRunner;
    KeyPress KeyPress;
    Text Sajak;

    public bool IsExpressRunning = false;

    ExpressCountdownTimer Timer;

    // Use this for initialization
    public void Start() {
        RoundRunner = gameObject.GetComponent<RoundRunner>();
        KeyPress = RoundRunner.KeyPress;
        KeyPress.expressWedgeLanded = this;
        Sajak = RoundRunner.SajakText;
        Timer = gameObject.AddComponent<ExpressCountdownTimer>();
        Timer.SetRoundRunner(RoundRunner);
    }

    internal IEnumerator Landed() {
        RoundRunner.IsTimeForLetter = false;
        KeyPress.IsTimeForExpressDecision = true;
        yield return SaySajak("You've landed on the Express Wedge, " + PlayerList.CurrentPlayer.Name + "!", 4.5f);

        yield return SaySajak("Press (1) to skip the Express and guess a consonant for 1,000 or...", 7f);
        yield return SaySajak("Press (2) to get on the Express ride...", 6f);
        yield return SaySajak("Where you keep calling consonants for 1,000 or buy vowels...", 7f);
        yield return SaySajak("until you solve the puzzle, but be careful...", 7f);
        yield return SaySajak("If you make a mistake or take too long to guess...", 6f);
        yield return SaySajak("You'll BANKRUPT!", 6f);
        Sajak.text = "Press (1) to skip.  Press (2) to ride the Express!";
    }

    public void StopTimer() {
        Timer.StopTimer();
    }

    public void StartTimer(bool isFirstTimeStarting) {
        Timer.StartTimer(isFirstTimeStarting);
    }

    internal IEnumerator TakeChance() {
        RoundRunner.AudioTracks.Play("get_set");
        Sajak.text = "<!-- 3 --!>";
        yield return new WaitForSeconds(1f);
        RoundRunner.AudioTracks.Play("get_set");
		Sajak.text = "<!-- 2 --!>";
        yield return new WaitForSeconds(1f);
        RoundRunner.AudioTracks.Play("get_set");
		Sajak.text = "<!-- 1 --!>";
        yield return new WaitForSeconds(0.558f);
        RoundRunner.AudioTracks.Play("express_music");
        yield return new WaitForSeconds(0.442f);
        RoundRunner.AudioTracks.Play("go");

        IsExpressRunning = true;
        RoundRunner.IsTimeForLetter = true;
        Timer.StartTimer(true);
        RoundRunner.ToggleUIButtons();
        yield return 0;
    }

    internal void DontTakeChance() {
        Sajak.text = "Very well. Choose a consonant for " + RoundRunner.CurrentWedge.Value + ".";
        RoundRunner.IsTimeForLetter = true;
    }

    private IEnumerator SaySajak(string text, float time) {
        Sajak.text = text;
        yield return new WaitForSeconds(time);
    }
}
