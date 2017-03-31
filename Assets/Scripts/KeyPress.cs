using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPress : MonoBehaviour {

    public GameObject RoundRunnerObject;
    public GameObject MenuObject;
    public GameObject BoardObject;
    public GameObject WheelObject;

    private RoundRunner RoundRunner;
    private bool isMenuActive = false;

	// Use this for initialization
	void Start () {
        RoundRunner = RoundRunnerObject.GetComponent<RoundRunner>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.anyKeyDown) {

            bool isWheelActive = WheelObject.activeSelf;

            if (Input.GetKeyDown(KeyCode.Space) && isWheelActive && !isMenuActive) {
                SpinWheel spinWheel = WheelObject.transform.GetChild(0).transform.GetChild(0).GetComponent<SpinWheel>();
                spinWheel.Spin();
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

            if (!isWheelActive && !isMenuActive) {
                for (char i = 'a'; i <= 'z'; i++)
                {
                    string strChar = (i.ToString());
                    if (Input.GetKeyDown(strChar))
                    {
                        RoundRunner.LetterPressed(i);
                    }
                }
            }
        }
    }
}
