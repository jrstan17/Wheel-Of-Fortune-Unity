using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPress : MonoBehaviour {

    public GameObject RoundRunnerObject;
    public GameObject MenuObject;
    public GameObject BoardObject;
    public GameObject WheelObject;

    private RoundRunner RoundRunner;
    public bool isMenuActive = false;
    public bool isWheelActive = false;

    // Use this for initialization
    void Start() {
        RoundRunner = RoundRunnerObject.GetComponent<RoundRunner>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.anyKeyDown) {

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
}
