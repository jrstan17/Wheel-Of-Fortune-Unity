using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisabilityToggler : MonoBehaviour {

	public GameObject[] ObjectsToToggle;
	private List<Vector3> origLoc = new List<Vector3>();

	public void ToggleVisability(bool isOn) {
		if (!isOn) {
			if (origLoc.Count == 0) {
				foreach (GameObject go in ObjectsToToggle) {
					origLoc.Add(go.transform.position);
				}
			}

			foreach (GameObject go in ObjectsToToggle) {				
				go.transform.position = new Vector3(-1000, -1000);
			}
		} else {
			if (origLoc.Count != 0) {
				int i = 0;
				foreach (Vector3 v in origLoc) {
					ObjectsToToggle[i].transform.position = v;
					i++;
				}
			}
		}
	}
}
