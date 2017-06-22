using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts.Enums;

public class SolveFunctions : MonoBehaviour {

    public RoundRunner RoundRunner;
    public InputField SolveField;
    public CountdownTimer Countdown;
    public Button IllSpinButton;

    public void ToggleWindow(bool enable) {
        if (enable) {
            GetComponent<Canvas>().enabled = true;
            Countdown.StartTimer();
            RoundRunner.IsTimeForLetter = false;
            SolveField.text = "";
            SolveField.ActivateInputField();
        } else {
            Countdown.StopTimer();
            GetComponent<Canvas>().enabled = false;
        }
    }

    public bool IsWindowEnabled() {
        return GetComponent<Canvas>().enabled;
    }

    // Update is called once per frame
    void Update () {
        if (IsWindowEnabled()) {
            
            RoundRunner.ToggleUIButtonsParsing("all", false);
                        
            SolveField.text = SolveField.text.ToUpper();

            if (Input.GetKeyUp(KeyCode.Return)) {
                Submit_Clicked();
            }

            if (Countdown.TimeLeft == 0) {
                StartCoroutine(RoundRunner.SolvedIncorrectly(SolvedIncorrectlyArg.IsOutOfTime));
            }
        }
    }

    public void Submit_Clicked() {
        ToggleWindow(false);

        string guess = Format(SolveField.GetComponent<InputField>().text);
        string correct = Format(RoundRunner.Puzzle.Text);


        if (guess.Equals(correct)) {
            RoundRunner.ToggleUIButtonsParsing("all", false);
            StartCoroutine(RoundRunner.SolvedCorrectly());
        } else {
            RoundRunner.ToggleUIButtonsParsing("all", false);

            if (RoundRunner.KeyPress.expressWedgeLanded.IsExpressRunning) {
                StartCoroutine(RoundRunner.SolvedIncorrectly(SolvedIncorrectlyArg.Express));
            } else {
                StartCoroutine(RoundRunner.SolvedIncorrectly(SolvedIncorrectlyArg.Default));
            }
        }
    }

    public string Format(string str) {
        StringBuilder sb = new StringBuilder();

        string replacementWord = "AND";

        for(int i = 0; i < str.Length; i++) {
            char c = str[i];

            if (!char.IsLetter(c)) {
                if (c == '&') {
                    sb.Append(replacementWord);
                }
            } else {
                c = char.ToUpper(c);
                sb.Append(c);
            }
        }

        return sb.ToString();
    }
}
