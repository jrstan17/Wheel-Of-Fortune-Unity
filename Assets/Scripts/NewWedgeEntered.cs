using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewWedgeEntered : MonoBehaviour {

    public string WedgeText;
    public int WedgeValue;
    public WedgeType WedgeType;

    AudioTracks AudioTracks;

    private void Start() {
        AudioTracks = GameObject.FindGameObjectWithTag("RoundRunner").GetComponent<RoundRunner>().AudioTracks;
    }

    void OnTriggerEnter2D(Collider2D col) {
        RoundRunner.CurrentWedge = new WedgeData(WedgeValue, WedgeText, WedgeType);
        AudioTracks.Play("wheel_click");
    }
}
