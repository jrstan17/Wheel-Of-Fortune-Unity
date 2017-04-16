using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BonusRoundRunner : MonoBehaviour {

    public RoundRunner RoundRunner;
    public BoardFiller BoardFiller;
    public GameObject BonusSolveCanvas;

    internal Player Winner;

    public const float NORMAL_SAJAK_SPEED = 5f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

    public IEnumerator Run() {
        Debug.Log(RoundRunner.Puzzle.Text);

        Winner = PlayerList.WinningPlayer();

        RoundRunner.CategoryText.text = "";

        RoundRunner.BonusInputText.text = "";
        RoundRunner.RegularRoundButtonsObject.SetActive(false);

        RoundRunner.PlayerBar.SetActive(false);
        RoundRunner.Puzzle = RoundRunner.factory.NewPuzzle(RoundType.Bonus);
        BoardFiller.ClearBoard();
        RoundRunner.SetRoundColors(0);

        yield return UpdateSajak("Welcome to the Bonus Round, " + Winner.Name + ".", NORMAL_SAJAK_SPEED);
        yield return UpdateSajak("Let's spin the Bonus Wheel to select...", NORMAL_SAJAK_SPEED);
        yield return UpdateSajak("which prize you will be playing for!", NORMAL_SAJAK_SPEED);
        yield return UpdateSajak("The prizes range mostly from $35,000 to $50,000", NORMAL_SAJAK_SPEED);

        if (Winner.HasMillionWedge) {
            yield return UpdateSajak("But because you've made it this far with the One Million wedge,", NORMAL_SAJAK_SPEED);
            yield return UpdateSajak("the one, top prize that would normally be $100,000...", NORMAL_SAJAK_SPEED);
            yield return UpdateSajak("has been replaced with $1,000,000!!", NORMAL_SAJAK_SPEED);
        } else {
            yield return UpdateSajak("with one of the prizes being $100,000!", NORMAL_SAJAK_SPEED);
        }

        RoundRunner.CategoryText.text = RoundRunner.Puzzle.Category;
        RoundRunner.AudioTracks.Play("reveal");
        BoardFiller.InitBoard();
        yield return UpdateSajak("To begin, the category is " + RoundRunner.Puzzle.Category + ".", NORMAL_SAJAK_SPEED);

        yield return UpdateSajak("Now let's reveal RSTLNE to help you.", 3f);
        yield return BoardFiller.RevealRSTLNE();
        yield return new WaitForSeconds(1.5f);

        yield return UpdateSajak("We now need 3 more consonants and a vowel. Please enter them now.", 1f);

        RoundRunner.BonusRoundButtonsObject.SetActive(true);
        EventSystem.current.SetSelectedGameObject(RoundRunner.BonusInputText.gameObject);
        yield return 0;
    }

    public IEnumerator LettersSubmitted(List<char> inputedList) {
        RoundRunner.BonusRoundButtonsObject.SetActive(false);
        yield return UpdateSajak("OK, let's see what we've got...", 0);
        yield return BoardFiller.RevealLetters(inputedList);

        int revealed = RoundRunner.FindHowManyToReveal(inputedList);        

        if (revealed > 3) {
            yield return UpdateSajak("Looks like you got quite a bit of help there.", NORMAL_SAJAK_SPEED);
        } else if (revealed > 1) {
            yield return UpdateSajak("Looks like you've got a little help there.", NORMAL_SAJAK_SPEED);
        } else if (revealed == 1) {
            yield return UpdateSajak("Looks like that's it.", NORMAL_SAJAK_SPEED);
        } else {
            yield return new WaitForSeconds(3f);
            yield return UpdateSajak("Well, I'm sorry " + Winner.Name + ", but those letters were no help at all.", NORMAL_SAJAK_SPEED);
        }

        yield return UpdateSajak("You now have 30 seconds to solve the puzzle. Good luck.", NORMAL_SAJAK_SPEED);
        BonusSolveCanvas.SetActive(true);
    }

    public void SolvedCorrectly() {
        Closeout();
    }

    public IEnumerator SolvedIncorrectly() {
        RoundRunner.AudioTracks.Play("buzzer");
        yield return UpdateSajak("I'm sorry, " + Winner.Name + ", but it looks like you're out of time.", NORMAL_SAJAK_SPEED);
        yield return UpdateSajak("Let's see what the solution was...", 2f);
        yield return BoardFiller.RevealBoard();
        RoundRunner.AudioTracks.Play("ah");
        yield return new WaitForSeconds(3f);

        yield return Closeout();
    }

    public IEnumerator Closeout() {
        RoundRunner.AudioTracks.Play("theme");
        yield return UpdateSajak(Winner.Name + ", you're leaving us with a total of " + Winner.TotalWinnings.ToString("C0") + " in cash and prizes!", 5f);
        yield return UpdateSajak("Thank you all for playing Wheel of Fortune! See you next time!", 0f);
    }

    public IEnumerator UpdateSajak(string text, float time) {
        RoundRunner.SajakText.text = text;
        yield return new WaitForSeconds(time);
    }
}
