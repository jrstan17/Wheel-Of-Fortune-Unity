using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WedgeChangeContainer : MonoBehaviour {

    public string RuleName;

    public GameObject[] BeforeColliders;
    public GameObject BeforeWedge;
    public GameObject[] AfterColliders;
    public GameObject AfterWedge;

	public void ToggleAfter() {
        ToggleForAfter(true);
    }

    public void ToggleBefore() {
        ToggleForAfter(false);
    }

    private void ToggleForAfter(bool toggle) {
        foreach (GameObject go in BeforeColliders) {
            go.SetActive(!toggle);
        }
        BeforeWedge.SetActive(!toggle);

        foreach (GameObject go in AfterColliders) {
            go.SetActive(toggle);
        }
        AfterWedge.SetActive(toggle);
    }
}
