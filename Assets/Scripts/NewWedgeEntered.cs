using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewWedgeEntered : MonoBehaviour {

    public GameObject WheelWithSpinObject;
    public GameObject Wedge;

    private WedgeData data;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

    void FixedUpdate() {

    }

    void OnTriggerEnter2D(Collider2D col) {
        data = Wedge.GetComponent<WedgeData>();
        RoundRunner.CurrentWedge = data;
    }
}
