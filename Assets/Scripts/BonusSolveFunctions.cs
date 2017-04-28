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
            Countdown.StartTimer();
            Countdown.TimeLeft = 30f;
            SolveField.text = "";
            BoardFiller = BonusRoundRunner.BoardFiller;
            RoundRunner.MusicAudioTracks.Play("countdown");
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
                timerStopFlag = true;
                Countdown.StopTimer();
                RoundRunner.MusicAudioTracks.Stop();
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
            timerStopFlag = true;
            Countdown.StopTimer();
            RoundRunner.MusicAudioTracks.Stop();
            GetComponent<Canvas>().enabled = false;
            StartCoroutine(BonusRoundRunner.SolvedCorrectly());
        } else {
            SolveField.text = "";
            RoundRunner.SFXAudioTracks.Play("double_buzzer");
        }
    }
}
