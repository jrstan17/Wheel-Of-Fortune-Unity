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
        yield return SaySajak("Press (2) to go the Express route...", 6f);
        yield return SaySajak("Where you keep calling consonants for 1,000 or buying vowels...", 7f);
        yield return SaySajak("until you solve the puzzle, but...", 6f);
        yield return SaySajak("Be careful. If you guess or solve incorrectly...", 6f);
        yield return SaySajak("or take more than 10 seconds to think of your next letter...", 7.5f);
        yield return SaySajak("after the previous letter had been completely revealed...", 7.5f);
        yield return SaySajak("You'll BANKRUPT!", 5f);
        Sajak.text = "Press (1) to skip.  Press (2) to Express!";
    }

    public void StopTimer() {
        Timer.StopTimer();
    }

    public void StartTimer() {
        Timer.StartTimer();
    }

    internal IEnumerator TakeChance() {
        IsExpressRunning = true;
        Sajak.text = "Let's begin.";
        RoundRunner.IsTimeForLetter = true;
        Timer.StartTimer();
        RoundRunner.AudioTracks.Play("express_music");
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
