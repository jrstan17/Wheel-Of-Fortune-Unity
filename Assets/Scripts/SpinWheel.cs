﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SpinWheel : MonoBehaviour {
    public List<AnimationCurve> animationCurves;
    public RoundRunner RoundRunner;
    public BonusRoundRunner BonusRoundRunner;
    public KeyPress KeyPress;
    public WheelClickPlayer ClickPlayer;

    internal bool HasSpun;
    internal bool HasPast = false;
    internal bool IsBonusSpin = false;

    internal static bool spinning;
    internal static float SpinTime = 6f;
    internal static int minAngle = Constants.DEFAULT_MIN;
    internal static int maxAngle = Constants.DEFAULT_MAX;
    internal bool isDebugSpin = false;
    internal string debugWedgeText;

    void Start() {
        spinning = false;
    }

    private void OnEnable() {
        HasSpun = false;
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
            float rndAngle = transform.eulerAngles.z + RandomGrabBag.Grab() + Random.Range(0, 1f);
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

            if (RoundRunner.CurrentWedge != null && isDebugSpin && RoundRunner.CurrentWedge.Text.Equals(debugWedgeText)) {
                break;
            }

			yield return new WaitForEndOfFrame();
        }

        spinning = false;
        float waitTime = 1f;

        if (!IsBonusSpin) {
            WedgeType currentType = RoundRunner.CurrentWedge.WedgeType;

            if (currentType == WedgeType.HighAmount || currentType == WedgeType.TenThousand) {
                RoundRunner.AudioTracks.Play("oh");
            } else if (currentType == WedgeType.FreePlay || currentType == WedgeType.Prize || currentType == WedgeType.Million || currentType == WedgeType.Wild || currentType == WedgeType.Mystery) {
                RoundRunner.AudioTracks.Play("freeplay");
                waitTime = 2f;
            } else if (currentType == WedgeType.Express) {
                RoundRunner.AudioTracks.Play("express_landed");
            }

            ItemManager itemManager = RoundRunner.ItemManager;
            if (currentType == WedgeType.FreePlay) {
                itemManager.ToggleFreePlay(true);
            } else if (currentType == WedgeType.Wild) {
                itemManager.ToggleWild(IconState.Enabled);
            }
        }

        isDebugSpin = false;
		minAngle = Constants.DEFAULT_MIN;
		maxAngle = Constants.DEFAULT_MAX;

        StartCoroutine(TimesUp(waitTime));
    }

    public IEnumerator TimesUp(float time) {
        yield return new WaitForSeconds(time);

        if (IsBonusSpin) {
            GameObject wheelObject = GameObject.FindGameObjectWithTag("BonusWheelObject");
            KeyPress.isBonusWheelActive = false;
            wheelObject.SetActive(false);
            BonusRoundRunner.WheelWindowClosed();
        } else {
            GameObject wheelObject = GameObject.FindGameObjectWithTag("WheelObject");
            KeyPress.isWheelActive = false;
            wheelObject.SetActive(false);
            RoundRunner.WheelWindowClosed();
        }
    }
}
