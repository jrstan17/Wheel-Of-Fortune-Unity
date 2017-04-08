using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SolveFunctions : MonoBehaviour {

    public RoundRunner RoundRunner;
    public InputField SolveField;
    public CountdownTimer Countdown;

    void OnEnable() {
        Countdown.StartTimer();
        SolveField.text = "";
    }

    // Update is called once per frame
    void Update () {
        SolveField.ActivateInputField();
        SolveField.text = SolveField.text.ToUpper();

        if (Input.GetKey(KeyCode.Return)) {
            Submit_Clicked();
        }

        if (Countdown.timeLeft == 0) {
            RoundRunner.SolvedIncorrectly(true);
        }
    }

    public void Submit_Clicked() {
        string solveText = SolveField.GetComponent<InputField>().text;
        solveText = solveText.ToUpper();

        Countdown.StopTimer();

        StringBuilder sb = new StringBuilder();
        for(int i = 0; i < RoundRunner.Puzzle.Text.Length; i++) {
            sb.Append(RoundRunner.Puzzle.Text[i]);

            if (RoundRunner.Puzzle.Text[i] == '-' && RoundRunner.Puzzle.Text[i+1] == ' ') {
                i++;
            }
        }

        if (solveText.Equals(sb.ToString())) {
            RoundRunner.SolvedCorrectly();
        } else {
            RoundRunner.SolvedIncorrectly(false);
        }
    }
}
