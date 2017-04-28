using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BonusRoundRunner : MonoBehaviour {

    public RoundRunner RoundRunner;
    public BoardFiller BoardFiller;
    public GameObject BonusSolveCanvas;
    public GameObject WheelObject;
    public KeyPress KeyPress;
    public SpinWheel BonusWheelSpin;

    internal static int PrizeAmount;
    internal Player Winner;

    public const float NORMAL_SAJAK_SPEED = 5f;

    public IEnumerator Run() {
        Debug.Log(RoundRunner.Puzzle.Text);

        Winner = PlayerList.WinningPlayer();

        RoundRunner.CategoryText.text = "";

        RoundRunner.BonusInputText.text = "";
        RoundRunner.RegularRoundButtonsObject.SetActive(false);

        RoundRunner.PlayerBar.SetActive(false);
        RoundRunner.Puzzle = RoundRunner.PuzzleFactory.NewPuzzle(RoundType.Bonus);
        BoardFiller.ClearBoard();
        RoundRunner.SetRoundColors(0);

        yield return UpdateSajak("Welcome to the Bonus Round, " + Winner.Name + ".", NORMAL_SAJAK_SPEED);
        yield return UpdateSajak("Let's begin by spinning for a prize on the Bonus Wheel", NORMAL_SAJAK_SPEED);

        if (Winner.HasMillionWedge) {
            yield return UpdateSajak("But because you've made it here with the One Million Dollar wedge,", NORMAL_SAJAK_SPEED);
            yield return UpdateSajak("the top prize that would normally be $100,000...", NORMAL_SAJAK_SPEED);
            yield return UpdateSajak("has been replaced with ONE MILLION DOLLARS!!", NORMAL_SAJAK_SPEED);
        } else {
            yield return UpdateSajak("For a chance to win up to $100,000!", NORMAL_SAJAK_SPEED);
        }

        KeyPress.isBonusWheelActive = true;
        BonusWheelSpin.IsBonusSpin = true;
        WheelObject.SetActive(true);       
    }

    public IEnumerator LettersSubmitted(List<char> inputedList) {
        RoundRunner.BonusRoundButtonsObject.SetActive(false);
        yield return UpdateSajak("OK, let's see if those help you...", 2f);
        yield return BoardFiller.RevealLetters(inputedList);

        int revealed = RoundRunner.FindHowManyToReveal(inputedList);

        yield return new WaitForSeconds(3f);        

        if (revealed > 3) {
            yield return UpdateSajak("Looks like you got quite a bit of help there.", NORMAL_SAJAK_SPEED);
        } else if (revealed > 1) {
            yield return UpdateSajak("Looks like you've got a little help there.", NORMAL_SAJAK_SPEED);
        } else if (revealed == 1) {
            yield return UpdateSajak("Looks like that's it.", NORMAL_SAJAK_SPEED);
        } else {
            yield return UpdateSajak("Well, I'm sorry " + Winner.Name + ", but those letters were no help at all.", NORMAL_SAJAK_SPEED);
        }

        yield return UpdateSajak("You have 30 seconds to solve the puzzle. Good luck.", NORMAL_SAJAK_SPEED);
        yield return UpdateSajak("", 0f);
        BonusSolveCanvas.SetActive(true);
    }

    internal IEnumerator StartWheelClosedDialog() {
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
    }

    internal void WheelWindowClosed() {
        StartCoroutine(StartWheelClosedDialog());
    }

    public IEnumerator SolvedCorrectly() {
        RoundRunner.AudioTracks.Play("round_win");
        StartCoroutine(BoardFiller.RevealBoard());

        yield return UpdateSajak("That's right, " + Winner.Name + "!", NORMAL_SAJAK_SPEED);

        yield return UpdateSajak("Let's open the envelope and see what you win...", 4f);

        Winner.TotalWinnings += PrizeAmount;

        yield return UpdateSajak(Winner.Name + ", you won " + PrizeAmount.ToString("C0") + "! Congratulations!", 3f);

        yield return Closeout();
    }

    public IEnumerator SolvedIncorrectly() {
        RoundRunner.AudioTracks.Play("buzzer");
        yield return UpdateSajak("I'm sorry, " + Winner.Name + ", but it looks like you're out of time.", NORMAL_SAJAK_SPEED);
        yield return UpdateSajak("Let's see what the solution was...", 4f);
        yield return BoardFiller.RevealBoard();
        RoundRunner.AudioTracks.Play("ah");
        yield return new WaitForSeconds(4f);

        yield return UpdateSajak("Let's open the envelope and see what you would have won...", 4f);

        RoundRunner.AudioTracks.Play("bankrupt");
        yield return UpdateSajak(PrizeAmount.ToString("C0") + ". I'm sorry.", NORMAL_SAJAK_SPEED);

        yield return Closeout();
    }

    public IEnumerator Closeout() {
        RoundRunner.AudioTracks.Play("theme");
        int highScorePlace = HighScore.UpdateHighScores(Winner.Name, Winner.TotalWinnings);

        if (highScorePlace > 0) {
            yield return UpdateSajak(Winner.Name + ", you have a new high score!", NORMAL_SAJAK_SPEED);
            yield return UpdateSajak("You made it to " + highScorePlace + HighScore.GetOrdinalSuffix(highScorePlace) + " place!", NORMAL_SAJAK_SPEED);
        }

        yield return UpdateSajak(Winner.Name + ", you're leaving us with a total of " + Winner.TotalWinnings.ToString("C0") + " in cash and prizes!", 7f);
        yield return UpdateSajak("Thank you everyone for playing Wheel of Fortune! See you next time!", 0f);
    }

    public IEnumerator UpdateSajak(string text, float time) {
        RoundRunner.SajakText.text = text;
        yield return new WaitForSeconds(time);
    }
}
