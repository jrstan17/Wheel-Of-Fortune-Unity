using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MysteryWedgeLanded : MonoBehaviour {

    RoundRunner RoundRunner;
    KeyPress KeyPress;
    Text Sajak;

	// Use this for initialization
	public void Start () {
        RoundRunner = gameObject.GetComponent<RoundRunner>();
        KeyPress = RoundRunner.KeyPress;
        KeyPress.mysteryWedgeLanded = this;
        Sajak = RoundRunner.SajakText;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal IEnumerator Landed() {
        RoundRunner.IsTimeForLetter = false;
        yield return SaySajak("You've landed on the Mystery Wedge!", 3f);
        KeyPress.IsTimeForMysteryDecision = true;
        Sajak.text = "Press '1' to guess a consonant for 1,000. Press '2' to take a chance first.";
    }

    internal IEnumerator TakeChance() {
        int picked = Random.Range(0, 2);

        RoundRunner.SFXAudioTracks.Play("drumroll");
        Sajak.text = "And under the Mystery Wedge is...";
        yield return new WaitForSeconds(RoundRunner.SFXAudioTracks.Drumroll.length);
        RoundRunner.MusicAudioTracks.Play("cymbal_crash");

        if (picked == 0) {
            RoundRunner.SFXAudioTracks.Play("bankrupt");
            Sajak.text = "A BANKRUPT! I'm so sorry, " + PlayerList.CurrentPlayer.Name + ", but that's the luck of the draw.";
            RoundRunner.doBankruptLogic(PlayerList.CurrentPlayer);
            yield return new WaitForSeconds(5f);            
            RoundRunner.GotoNextPlayer();
            Sajak.text = "It's now " + PlayerList.CurrentPlayer + "'s turn.";
        } else {
            RoundRunner.SFXAudioTracks.Play("pq");
            Sajak.text = "$10,000!  Luck was on your side!";
            PlayerList.CurrentPlayer.RoundWinnings += 10000;
            yield return new WaitForSeconds(5f);
            Sajak.text = "Please select a consonant for " + RoundRunner.CurrentWedge.Value + ".";
        }

        RoundRunner.IsTimeForLetter = true;
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
