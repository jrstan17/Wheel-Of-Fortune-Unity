using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewWedgeEntered : MonoBehaviour {

    public GameObject TextBox;
    public GameObject Wedge;

    private WedgeData data;
    private Text text;

	// Use this for initialization
	void Start () {
        text = TextBox.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    void FixedUpdate() {

    }

    void OnTriggerEnter2D(Collider2D col) {
        data = Wedge.GetComponent<WedgeData>();

        if (data != null) {
            text.text = data.Text;
        }
    }
}
