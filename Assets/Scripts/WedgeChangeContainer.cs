using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WedgeChangeContainer : MonoBehaviour {

    public string RuleName;

    public GameObject[] BeforeColliders;
    public GameObject[] BeforeWedges;
    public GameObject[] AfterColliders;
    public GameObject[] AfterWedges;

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

        foreach (GameObject go in BeforeWedges) {
            go.SetActive(!toggle);
        }

        foreach (GameObject go in AfterColliders) {
            go.SetActive(toggle);
        }

        foreach (GameObject go in AfterWedges) {
            go.SetActive(toggle);
        }
    }
}
