using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyPress : MonoBehaviour {

    public GameObject RoundRunnerObject;
    public GameObject MenuObject;
    public GameObject BoardObject;
    public GameObject WheelObject;
    public GameObject DebugInputObject;

    private RoundRunner RoundRunner;
    internal bool isMenuActive = false;
    internal bool isWheelActive = false;
    internal InputField DebugInputField;

    // Use this for initialization
    void Start() {
        RoundRunner = RoundRunnerObject.GetComponent<RoundRunner>();
        DebugInputField = DebugInputObject.GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.anyKeyDown) {

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.BackQuote)) {
                if (DebugInputObject.activeInHierarchy) {
                    DebugInputObject.SetActive(false);
                } else {
                    DebugInputObject.SetActive(true);
                    DebugInputField.text = "";
                    EventSystem.current.SetSelectedGameObject(DebugInputObject, null);
                    DebugInputField.OnPointerClick(new PointerEventData(EventSystem.current));
                    return;
                }
            }

            if (!DebugInputObject.activeInHierarchy) {
                IfNoDebugField();
            } else {
                if (Input.GetKey(KeyCode.Return)) {
                    bool isValid = ParseDebugSplits(DebugInputField.text.Split(' '));
                    DebugInputObject.SetActive(false);
                } else if (Input.GetKey(KeyCode.Escape)) {
                    DebugInputObject.SetActive(false);
                }
            }
        }
    }

    private bool ParseDebugSplits(string[] splits) {
        if (splits == null || splits.Length == 0) {
            return false;
        }

        if (splits[0].Equals("reveal")) {
            if (splits.Length == 1) {
                RoundRunner.Reveal_Clicked();
                return true;
            } else {
                List<char> letters = new List<char>();
                foreach(char c in splits[1]) {
                    letters.Add(c);
                }

                RoundRunner.boardFiller.RevealLetters(letters);
                return true;
            }
        }

        return false;
    }

    private void IfNoDebugField() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            SpinWheel spinWheel = WheelObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SpinWheel>();

            if (!isMenuActive && isWheelActive && !spinWheel.HasSpun) {
                spinWheel.Spin();
                return;
            }
        }

        if (Input.GetKeyDown(KeyCode.F1)) {
            RoundRunner.NewRegular_Clicked();
            return;
        }

        if (Input.GetKeyDown(KeyCode.F2)) {
            RoundRunner.Reveal_Clicked();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isMenuActive) {
                MenuObject.SetActive(false);
                MenuObject.SetActiveRecursively(false);
                isMenuActive = false;
            } else {
                MenuObject.SetActive(true);
                MenuObject.SetActiveRecursively(true);
                isMenuActive = true;
            }

            return;
        }

        if (!isWheelActive && !isMenuActive && !RoundRunner.IsBonusRound) {
            for (char i = 'a'; i <= 'z'; i++) {
                string strChar = (i.ToString());
                if (Input.GetKeyDown(strChar)) {
                    RoundRunner.LetterPressed(i);
                }
            }
        }
    }
}
