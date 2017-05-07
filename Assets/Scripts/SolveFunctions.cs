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

        if (Input.GetKeyUp(KeyCode.Return)) {
            Submit_Clicked();
        }

        if (Countdown.TimeLeft == 0) {
            RoundRunner.SolvedIncorrectly(true);
        }
    }

    public void Submit_Clicked() {
        Countdown.StopTimer();

        string solveText = SolveField.GetComponent<InputField>().text;
        solveText = solveText.ToUpper();
        solveText = RemoveSpaces(solveText);

        string correct = RemoveSpaces(RoundRunner.Puzzle.Text);

        if (solveText.Equals(correct)) {
            RoundRunner.ToggleUIButtonsParsing("all", false);
            StartCoroutine(RoundRunner.SolvedCorrectly());
        } else {
            RoundRunner.ToggleUIButtonsParsing("all", false);
            RoundRunner.SolvedIncorrectly(false);
        }
    }

    public string RemoveSpaces(string str) {
        StringBuilder sb = new StringBuilder();

        foreach(char c in str){
            if (c != ' ') {
                sb.Append(c);
            }
        }

        return sb.ToString();
    }
}
