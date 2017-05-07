using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewWedgeEntered : MonoBehaviour {

    public string WedgeText;
    public int WedgeValue;
    public WedgeType WedgeType;

    void OnTriggerEnter2D(Collider2D col) {
        RoundRunner.CurrentWedge = new WedgeData(WedgeValue, WedgeText, WedgeType);
    }
}
