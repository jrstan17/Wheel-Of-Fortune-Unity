using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownTimer : MonoBehaviour {

    public float timeLeft = 30.0f;
    private bool Start = false;

    void Update() {
        if (Start) {
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0) {
                timeLeft = 0;
                OnTimesUp(EventArgs.Empty);
            }
        }
    }

    public void StartTimer() {
        Start = true;
    }

    public bool isRunning() {
        return Start;
    }

    protected virtual void OnTimesUp(EventArgs e) {
        EventHandler handler = TimesUp;
        if (handler != null) {
            Start = false;
            handler(this, e);
        }
    }

    public event EventHandler TimesUp;
}
