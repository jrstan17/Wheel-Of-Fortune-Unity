using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SpinWheel : MonoBehaviour {
    public List<AnimationCurve> animationCurves;
    public float AverageFriction;
    public float RandomFriction;

    private bool spinning;
    private float randomTime;

    void Start() {
        spinning = false;
    }

    void Update() {

    }

    public void Spin() {
        if (!spinning) {
            randomTime = Random.Range(AverageFriction - RandomFriction, AverageFriction + RandomFriction);
            float maxAngle = 360 * randomTime;

            StartCoroutine(SpinTheWheel(5.1f * randomTime, maxAngle));
        }
    }

    private IEnumerator SpinTheWheel(float time, float maxAngle) {
        spinning = true;

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

        transform.eulerAngles = new Vector3(0.0f, 0.0f, maxAngle + startAngle);
        spinning = false;
    }
}
