using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MysteryWedgeLanded : MonoBehaviour {

    RoundRunner RoundRunner;
    KeyPress KeyPress;
    Text Sajak;

    // Use this for initialization
    public void Start() {
        RoundRunner = gameObject.GetComponent<RoundRunner>();
        KeyPress = RoundRunner.KeyPress;
        KeyPress.mysteryWedgeLanded = this;
        Sajak = RoundRunner.SajakText;
    }

    internal IEnumerator Landed() {
        RoundRunner.IsTimeForLetter = false;
        KeyPress.IsTimeForMysteryDecision = true;
        yield return SaySajak("You've landed on a Mystery Wedge, " + PlayerList.CurrentPlayer.Name + "!", 4.5f);

        yield return SaySajak("Press (1) to skip the mystery and guess a consonant for 1,000 or...", 7f);
        yield return SaySajak("Press (2) to see what's under the Mystery Wedge...", 6f);
        yield return SaySajak("Hidden underneath is either $10,000 or a Bankrupt!", 7f);
        Sajak.text = "Press (1) to skip.  Press (2) to take the chance.";
    }

    internal IEnumerator TakeChance() {
        int picked = Random.Range(0, 2);

        RoundRunner.AudioTracks.Play("drumroll");
        Sajak.text = "Underneath the Mystery Wedge is...";
        yield return new WaitForSeconds(RoundRunner.AudioTracks.Drumroll.length);
        RoundRunner.AudioTracks.Play("cymbal_crash");

        if (picked == 0) {
            RoundRunner.AudioTracks.Play("bankrupt");
            yield return SaySajak("A BANKRUPT!", 3.5f);
            yield return SaySajak("I'm sorry, " + PlayerList.CurrentPlayer.Name + ". That's the risk of the Mystery Wedge.", 5f);
            RoundRunner.doBankruptLogic(PlayerList.CurrentPlayer);
            RoundRunner.GotoNextPlayer();
            Sajak.text = "It's now " + PlayerList.CurrentPlayer.Name + "'s turn.";
        } else {
            RoundRunner.AudioTracks.Play("pq");
            PlayerList.CurrentPlayer.RoundWinnings += Constants.MYSTERY_WEDGE_WIN;
            yield return SaySajak(Constants.MYSTERY_WEDGE_WIN.ToString("C0") + "!  Luck was on your side!", 6f);
            Sajak.text = "Please select a consonant for " + RoundRunner.CurrentWedge.Value.ToString("N0") + ".";
        }

        RoundRunner.IsTimeForLetter = true;

        GameObject WheelBaseObject = RoundRunner.WheelCanvas.transform.GetChild(0).gameObject;
        int index = WedgeRules.GetWedgeChangeIndex("mystery", WheelBaseObject);
        WedgeChangeContainer wildChange =
            WheelBaseObject.GetComponents<WedgeChangeContainer>()[index];
        wildChange.ToggleAfter();
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
