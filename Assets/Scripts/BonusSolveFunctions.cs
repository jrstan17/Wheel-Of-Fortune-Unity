using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BonusSolveFunctions : MonoBehaviour {

    public RoundRunner RoundRunner;
    public BonusRoundRunner BonusRoundRunner;
    public InputField SolveField;
    public CountdownTimer Countdown;

    internal BoardFiller BoardFiller;

    bool timerStopFlag = false;

    void OnEnable() {
        RoundRunner.IsTimeForLetter = false;
        Countdown.StartTimer();
        Countdown.TimeLeft = Constants.BONUS_SOLVE_TIME;
        SolveField.text = "";
        BoardFiller = BonusRoundRunner.BoardFiller;
        RoundRunner.AudioTracks.Play("countdown");
        KeyPress.IsSolvingActive = true;
    }

    // Update is called once per frame
    void Update() {
        if (!timerStopFlag) {
            SolveField.ActivateInputField();
            SolveField.text = SolveField.text.ToUpper();

            if (Input.GetKeyUp(KeyCode.Return)) {
                Submit_Clicked();
            }

            if (Countdown.TimeLeft == 0) {
                KeyPress.IsSolvingActive = false;
                timerStopFlag = true;
                Countdown.StopTimer();
                RoundRunner.AudioTracks.StopAll();
                StartCoroutine(BonusRoundRunner.SolvedIncorrectly());
                GetComponent<Canvas>().enabled = false;
            }
        }
    }

    public void Submit_Clicked() {
        string solveText = SolveField.GetComponent<InputField>().text;
        solveText = solveText.ToUpper();

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < RoundRunner.Puzzle.Text.Length; i++) {
            sb.Append(RoundRunner.Puzzle.Text[i]);

            if (RoundRunner.Puzzle.Text[i] == '-' && RoundRunner.Puzzle.Text[i + 1] == ' ') {
                i++;
            }
        }

        if (solveText.Equals(sb.ToString())) {
            KeyPress.IsSolvingActive = false;
            timerStopFlag = true;
            Countdown.StopTimer();
            RoundRunner.AudioTracks.StopAll();
            GetComponent<Canvas>().enabled = false;
            StartCoroutine(BonusRoundRunner.SolvedCorrectly());
        } else {
            SolveField.text = "";
            RoundRunner.AudioTracks.Play("double_buzzer");
        }
    }
}
