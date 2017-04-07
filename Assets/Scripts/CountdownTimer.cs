using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour {

    public float timePerChar;

    private float timeLeft;
    private bool IsStarted = false;
    private Text text;

    void Start() {
        text = gameObject.GetComponent<Text>();        
    }

    void Update() {
        if (IsStarted) {
            timeLeft -= Time.deltaTime;

            text.text = timeLeft.ToString("N1");

            if (timeLeft < 0) {
                timeLeft = 0;
                IsStarted = false;
            }
        }
    }

    public void StartTimer() {
        timeLeft = RoundRunner.Puzzle.Text.Length * timePerChar;
        IsStarted = true;
    }

    public void StopTimer() {
        IsStarted = false;
        timeLeft = 30;
    }

    public bool isRunning() {
        return IsStarted;
    }
}
