using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpressCountdownTimer : MonoBehaviour {

    internal float TimeLeft;
    private bool IsStarted = false;
    bool IsFirstTimeStarting = true;

    RoundRunner RoundRunner;

    void Update() {
        if (IsStarted) {
            TimeLeft -= Time.deltaTime;

            if (IsFirstTimeStarting) {
                RoundRunner.SajakText.text = " GO!  ";
            } else {
                RoundRunner.SajakText.text = " Keep Going!  ";
            }

            RoundRunner.SajakText.text += "You have " + TimeLeft.ToString("N2") + " seconds to guess.";

            if (TimeLeft < 0) {
                TimeLeft = 0;
                IsStarted = false;
                StartCoroutine(RoundRunner.ExpressTimeElapsed());
            }
        }
    }

    public void SetRoundRunner(RoundRunner runner) {
        RoundRunner = runner;
    }

    public void StartTimer(bool isFirstTimeStarting) {
        IsFirstTimeStarting = isFirstTimeStarting;
        TimeLeft = Constants.EXPRESS_LETTER_GUESS_TIME;
        IsStarted = true;
    }

    public void StopTimer() {
        IsStarted = false;
        TimeLeft = Constants.EXPRESS_LETTER_GUESS_TIME;
    }

    public bool isRunning() {
        return IsStarted;
    }
}

