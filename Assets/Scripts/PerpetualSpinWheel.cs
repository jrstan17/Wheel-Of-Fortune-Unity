using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PerpetualSpinWheel : MonoBehaviour {

    public RandomColorChanger ColorChanger;
    float currentAngle = 0;
    public float Speed;
    public bool SpinClockwise = true;

    private void Start() {
        currentAngle = Random.Range(0, 360);
        ColorChanger.StartColorChange();
        StartCoroutine(Spin());
    }

    public IEnumerator Spin() {
        while (true) {
            transform.eulerAngles = new Vector3(0.0f, 0.0f, currentAngle);

            if (SpinClockwise) {
                currentAngle -= Speed;
            } else {
                currentAngle += Speed;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
