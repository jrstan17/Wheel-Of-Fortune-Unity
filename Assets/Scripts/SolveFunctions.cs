using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SolveFunctions : MonoBehaviour {

    public RoundRunner RoundRunner;
    public InputField SolveField;    

	// Use this for initialization
	void Start () {
        EventSystem.current.SetSelectedGameObject(SolveField.gameObject, null);
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey(KeyCode.Return)) {
            Submit_Clicked();
        }
    }

    public void Submit_Clicked() {
        string solveText = SolveField.GetComponent<InputField>().text;
        solveText = solveText.ToUpper();

        if (solveText.Equals(RoundRunner.Puzzle.Text)) {
            RoundRunner.SolvedCorrectly();
        } else {
            RoundRunner.SolvedIncorrectly();
        }
    }
}
