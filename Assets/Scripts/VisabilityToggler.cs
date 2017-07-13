using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisabilityToggler : MonoBehaviour {

	public GameObject[] ObjectsToToggle;

	public void ToggleVisability(bool isOn) {
		for (int i = 0; i < ObjectsToToggle.Length; i++) {
			Animator a = ObjectsToToggle[i].GetComponent<Animator>();
			if (a != null) {
				a.enabled = false;
			}

			ObjectsToToggle[i].SetActive(isOn);
		}
	}
}
