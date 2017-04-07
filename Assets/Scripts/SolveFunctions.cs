using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SolveFunctions : MonoBehaviour {

    public RoundRunner RoundRunner;
    public InputField SolveField;
    public CountdownTimer Countdown;

    private void OnEnable() {
        Countdown.StartTimer();
        EventSystem.current.SetSelectedGameObject(SolveField.gameObject, null);
    }

    // Update is called once per frame
    void Update () {
		if (Input.GetKey(KeyCode.Return)) {
            Submit_Clicked();
        }

        if (!Countdown.isRunning()) {
            RoundRunner.SolvedIncorrectly(true);
        }
    }

    public void Submit_Clicked() {
        string solveText = SolveField.GetComponent<InputField>().text;
        solveText = solveText.ToUpper();

        Countdown.StopTimer();

        if (solveText.Equals(RoundRunner.Puzzle.Text)) {
            RoundRunner.SolvedCorrectly();
        } else {
            RoundRunner.SolvedIncorrectly(false);
        }
    }
}
