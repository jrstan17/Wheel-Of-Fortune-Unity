using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisabilityToggler : MonoBehaviour {

	public GameObject[] ObjectsToToggle;

	public void ToggleVisability(bool isOn) {
		for (int i = 0; i < ObjectsToToggle.Length; i++) {
			GameObject obj = ObjectsToToggle[i];

			if (obj.GetComponent<Text>() != null) {
				obj.GetComponent<Text>().enabled = isOn;
			}

			if (obj.GetComponent<Image>() != null) {
				obj.GetComponent<Image>().enabled = isOn;
			}

			if (obj.GetComponent<SpriteRenderer>() != null) {
				obj.GetComponent<SpriteRenderer>().enabled = isOn;
			}
		}
	}
}
