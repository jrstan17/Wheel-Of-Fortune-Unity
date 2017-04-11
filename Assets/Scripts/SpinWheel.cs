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
    internal static int minAngle = 275;
    internal static int maxAngle = 540;

    internal bool isDebugSpin = false;
    internal string debugWedgeText;

    private IEnumerator coroutine;

    void Start() {
        spinning = false;
    }

    private void OnEnable() {
        HasSpun = false;
    }

    void Update() {

    }

    public void Randomize() {
        float rndAngle = Random.Range(transform.eulerAngles.z, transform.eulerAngles.z + 360);
        transform.eulerAngles = new Vector3(0, 0, rndAngle);
    }

    public IEnumerator Spin(bool isDebugSpin) {
        this.isDebugSpin = isDebugSpin;
        if (isDebugSpin) {
            maxAngle = 360;
            minAngle = 360;
        }

        if (!spinning && !HasSpun) {
            float rndAngle = Random.Range(transform.eulerAngles.z + minAngle, transform.eulerAngles.z + maxAngle);
            yield return StartCoroutine(SpinTheWheel(SpinTime, rndAngle));
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

            if (isDebugSpin && RoundRunner.CurrentWedge.Text.Equals(debugWedgeText)) {
                break;
            }

            yield return 0;
        }

        spinning = false;
        float waitTime = 1f;

        if (RoundRunner.CurrentWedge.WedgeType == WedgeType.HighAmount) {
            RoundRunner.AudioTracks.Play("oh");
        } else if (RoundRunner.CurrentWedge.WedgeType == WedgeType.TenThousand) {
            RoundRunner.AudioTracks.Play("cheering");
            waitTime = 2f;
        } else if (RoundRunner.CurrentWedge.WedgeType == WedgeType.FreePlay) {
            RoundRunner.AudioTracks.Play("freeplay");
            waitTime = 2f;
        } else if (RoundRunner.CurrentWedge.WedgeType == WedgeType.Bankrupt) {
            RoundRunner.AudioTracks.Play("bankrupt");
            waitTime = 2f;
        }

        isDebugSpin = false;
        minAngle = 275;
        maxAngle = 540;

        coroutine = TimesUp(waitTime);
        StartCoroutine(coroutine);
    }

    public IEnumerator TimesUp(float time) {
        yield return new WaitForSeconds(time);

        GameObject wheelObject = GameObject.FindGameObjectWithTag("WheelObject");

        KeyPress.isWheelActive = false;
        wheelObject.SetActive(false);
        RoundRunner.WheelWindowClosed();
    }
}
