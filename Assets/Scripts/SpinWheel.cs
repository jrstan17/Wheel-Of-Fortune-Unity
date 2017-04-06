﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SpinWheel : MonoBehaviour {
    public List<AnimationCurve> animationCurves;
    public RoundRunner RoundRunner;
    public KeyPress KeyPress;

    internal bool HasSpun;
    internal bool HasPast = false;

    internal bool spinning;
    internal static float SpinTime = 5f;
    internal static int minAngle = 360;
    internal static int maxAngle = 720;

    void Start() {
        spinning = false;
    }

    private void OnEnable() {
        HasSpun = false;
    }

    void Update() {

    }

    public void Spin() {
        if (!spinning && !HasSpun) {
            float rndAngle = Random.Range(transform.eulerAngles.z + minAngle, transform.eulerAngles.z + maxAngle);
            StartCoroutine(SpinTheWheel(SpinTime, rndAngle));
        }
    }

    private IEnumerator SpinTheWheel(float time, float rndAngle) {
        spinning = true;
        HasSpun = true;

        float timer = 0.0f;
        float startAngle = transform.eulerAngles.z;
        rndAngle = startAngle - rndAngle;

        int animationCurveNumber = Random.Range(0, animationCurves.Count);

        while (timer < time) {
            //to calculate rotation            
            float angle = rndAngle * animationCurves[animationCurveNumber].Evaluate(timer / time);
            transform.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
            timer += Time.deltaTime;
            yield return 0;
        }

        spinning = false;

        CountdownTimer countdownTimer = gameObject.AddComponent<CountdownTimer>();
        countdownTimer.timeLeft = 5;
        countdownTimer.TimesUp += CountdownTimer_TimesUp;
        countdownTimer.StartTimer();

        if (RoundRunner.CurrentWedge.WedgeType == WedgeType.LoseATurn) {
            RoundRunner.AudioTracks.Play("buzzer");
        } else if (RoundRunner.CurrentWedge.WedgeType == WedgeType.Bankrupt) {
            RoundRunner.AudioTracks.Play("bankrupt");
        }
    }

    private void CountdownTimer_TimesUp(object sender, System.EventArgs e) {
        GameObject wheelObject = GameObject.FindGameObjectWithTag("WheelObject");

        KeyPress.isWheelActive = false;
        wheelObject.SetActive(false);
        RoundRunner.WheelWindowClosed();
    }
}
