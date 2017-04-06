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
    public GameObject Background;

    private RoundRunner RoundRunner;
    internal bool isMenuActive = false;
    internal bool isWheelActive = false;
    internal InputField DebugInputField;

    internal string[] Splits;
    List<string> History = new List<string>();
    int HistoryIndex = 0;

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
                    History.Add(DebugInputField.text);
                    HistoryIndex = History.Count - 1;
                    ParseDebugSplits(DebugInputField.text.Split(' '));
                    DebugInputObject.SetActive(false);
                } else if (Input.GetKey(KeyCode.Escape)) {
                    DebugInputObject.SetActive(false);
                } else if (Input.GetKey(KeyCode.UpArrow)) {
                    DebugInputField.text = History[HistoryIndex];

                    if (HistoryIndex != 0) {
                        HistoryIndex--;
                    }
                } else if (Input.GetKey(KeyCode.DownArrow)) {
                    DebugInputField.text = History[HistoryIndex];

                    if (HistoryIndex != History.Count - 1) {
                        HistoryIndex++;
                    }
                }
            }
        }
    }

    private void ParseDebugSplits(string[] splits) {
        Splits = splits;

        if (splits == null || splits.Length == 0) {
            return;
        }

        for (int i = 0; i < splits.Length; i++) {
            Splits[i] = Splits[i].ToUpper();
        }

        Reveal();
        BackgroundColor();
        XYSpeed();
        RotateSpeed();
        SpinTime();
        SpinAngleRange();
    }

    private void BackgroundColor() {
        if (Splits[0].Equals("BACKGROUNDCOLOR") && Splits.Length == 4) {
            int[] asInt = new int[4];
            for (int i = 0; i < Splits.Length - 1; i++) {
                bool isParsed = int.TryParse(Splits[i + 1], out asInt[i]);
                if (!isParsed || asInt[i] < 0 || asInt[i] > 255) {
                    return;
                }
            }

            Color color = new Color32((byte)asInt[0], (byte)asInt[1], (byte)asInt[2], 255);
            Background.gameObject.GetComponent<Renderer>().material.SetColor("_Color", color);
        }
    }

    private void Reveal() {
        if (Splits[0].Equals("REVEAL")) {
            if (Splits.Length == 1) {
                RoundRunner.Reveal_Clicked();
            } else {
                List<char> letters = new List<char>();
                foreach (char c in Splits[1]) {
                    letters.Add(c);
                    RoundRunner.UsedLetterText[c - 65].color = Constants.USED_LETTER_DISABLED_COLOR;
                }

                RoundRunner.boardFiller.RevealLetters(letters);
            }
        }
    }

    private void SpinTime() {
        if (Splits[0].Equals("SPINTIME") && Splits.Length == 2) {
            float parsed = 0;
            bool isParsed = float.TryParse(Splits[1], out parsed);

            if (isParsed) {
                SpinWheel.SpinTime = parsed;
            }
        }
    }

    private void SpinAngleRange() {
        if (Splits[0].Equals("SPINANGLERANGE") && Splits.Length == 3) {
            int parsed = 0;
            bool isParsed = int.TryParse(Splits[1], out parsed);
            int parsed2 = 0;
            bool isParsed2 = int.TryParse(Splits[2], out parsed2);

            if (isParsed && isParsed2) {
                SpinWheel.minAngle = parsed;
                SpinWheel.maxAngle = parsed2;
            }
        }
    }

    private void XYSpeed() {
        if (Splits[0].Equals("XYSPEED")) {
            if (Splits.Length == 2) {
                float parsed = 0;
                bool isParsed = float.TryParse(Splits[1], out parsed);

                if (isParsed) {
                    Scroller scroller = Background.GetComponent<Scroller>();
                    scroller.xYSpeed = parsed;
                }
            }
        }
    }

    private void RotateSpeed() {
        if (Splits[0].Equals("ROTATESPEED")) {
            if (Splits.Length == 2) {
                float parsed = 0;
                bool isParsed = float.TryParse(Splits[1], out parsed);

                if (isParsed) {
                    Scroller scroller = Background.GetComponent<Scroller>();
                    scroller.rotateSpeed = parsed;
                }
            }
        }
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
