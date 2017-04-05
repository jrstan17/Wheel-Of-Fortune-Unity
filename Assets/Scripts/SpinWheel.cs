using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SpinWheel : MonoBehaviour {
    public List<AnimationCurve> animationCurves;
    public float AverageFriction;
    public float RandomFriction;
    public RoundRunner RoundRunner;
    public KeyPress KeyPress;

    internal bool HasSpun;

    internal bool spinning;
    private float randomTime;

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
            randomTime = Random.Range(1f, 1.1f);
            float maxAngle = 360 * randomTime;

            StartCoroutine(SpinTheWheel(5f * randomTime, maxAngle));
        }
    }

    private IEnumerator SpinTheWheel(float time, float maxAngle) {
        spinning = true;
        HasSpun = true;

        float timer = 0.0f;
        float startAngle = transform.eulerAngles.z;
        maxAngle = startAngle - maxAngle;

        int animationCurveNumber = Random.Range(0, animationCurves.Count);

        while (timer < time) {
            //to calculate rotation
            float angle = maxAngle * animationCurves[animationCurveNumber].Evaluate(timer / time);
            transform.eulerAngles = new Vector3(0.0f, 0.0f, angle + startAngle);
            timer += Time.deltaTime;
            yield return 0;
        }

        //transform.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
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
        RoundRunner.ToggleUIButtons("all", true);
        GameObject wheelObject = GameObject.FindGameObjectWithTag("WheelObject");

        KeyPress.isWheelActive = false;
        wheelObject.SetActive(false);
        RoundRunner.WheelWindowClosed();
    }
}
