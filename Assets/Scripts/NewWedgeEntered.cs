using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewWedgeEntered : MonoBehaviour {

    public GameObject WheelWithSpinObject;
    public GameObject SajakTextBox;
    public GameObject Wedge;

    private WedgeData data;
    private Text sajakText;

	// Use this for initialization
	void Start () {
        sajakText = SajakTextBox.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    void FixedUpdate() {

    }

    void OnTriggerEnter2D(Collider2D col) {
        data = Wedge.GetComponent<WedgeData>();

        if (data != null) {
            sajakText.text = data.Text;
        }
    }
}
