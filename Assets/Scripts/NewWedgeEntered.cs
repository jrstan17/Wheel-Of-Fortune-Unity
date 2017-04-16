using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewWedgeEntered : MonoBehaviour {

    public GameObject Wedge;
    public GameObject Wedge2;

    private WedgeData data;
    internal bool UseAlternativeWedge = false;
    internal TextMesh CurrentWedgeText;

    private void Start() {
        CurrentWedgeText = GameObject.FindGameObjectWithTag("CurrentWedgeText").GetComponent<TextMesh>();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (!UseAlternativeWedge) {
            data = Wedge.GetComponent<WedgeData>();
        } else {
            data = Wedge2.GetComponent<WedgeData>();
        }

        RoundRunner.CurrentWedge = data;
    }
}
