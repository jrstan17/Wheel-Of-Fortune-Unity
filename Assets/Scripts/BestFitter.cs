using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestFitter : MonoBehaviour {

    int nameCount = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        List<GameObject> newNameBtns = new List<GameObject>();
        foreach(Transform t in transform) {
            if (t.gameObject.activeSelf) {
                newNameBtns.Add(t.gameObject);
            }
        }

        if (newNameBtns.Count != nameCount) {            
            nameCount = newNameBtns.Count;

            foreach (GameObject go in newNameBtns) {
                Text text = go.transform.GetChild(0).GetComponent<Text>();
                text.resizeTextForBestFit = true;
            }

            int smallest = 0;
            bool smallestHasChanged = false;
            foreach(GameObject go in newNameBtns) {
                Text text = go.transform.GetChild(0).GetComponent<Text>();
                int bestfit = text.cachedTextGenerator.fontSizeUsedForBestFit;

                if (bestfit < smallest) {
                    smallest = text.fontSize;
                    smallestHasChanged = true;
                }
            }

            if (smallestHasChanged) {
                foreach (GameObject go in newNameBtns) {
                    Text text = go.transform.GetChild(0).GetComponent<Text>();
                    text.resizeTextForBestFit = false;
                    text.fontSize = smallest;
                }
            }
        }
	}
}
