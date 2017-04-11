using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour {

    public static float timePerChar = 1;

    internal float TimeLeft;
    private bool IsStarted = false;
    private Text text;

    void Start() {
        text = gameObject.GetComponent<Text>();        
    }

    void Update() {
        if (IsStarted) {
            TimeLeft -= Time.deltaTime;

            text.text = TimeLeft.ToString("N1");

            if (TimeLeft < 0) {
                TimeLeft = 0;
                IsStarted = false;
            }
        }
    }

    public void StartTimer() {
        TimeLeft = RoundRunner.Puzzle.Text.Length * timePerChar;
        IsStarted = true;
    }

    public void StopTimer() {
        IsStarted = false;
    }

    public bool isRunning() {
        return IsStarted;
    }
}
