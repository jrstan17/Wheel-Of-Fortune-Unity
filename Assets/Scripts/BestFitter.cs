using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BestFitter : MonoBehaviour {

    public Canvas Canvas;
    int nameCount = 0;

    public IEnumerator UpdateTextSize() {
        List<GameObject> newNameBtns = new List<GameObject>();
        foreach (Transform t in transform) {
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

            int smallest = int.MaxValue;
            bool smallestHasChanged = false;
            foreach (GameObject go in newNameBtns) {
                Text text = go.transform.GetChild(0).GetComponent<Text>();

                while (text.cachedTextGenerator.fontSizeUsedForBestFit == 0) {
                    yield return new WaitForFixedUpdate();
                }

                int bestfit = (int)(text.cachedTextGenerator.fontSizeUsedForBestFit / Canvas.scaleFactor);

                Debug.Log("After: " + bestfit);

                if (bestfit < smallest) {
                    smallest = bestfit;
                    smallestHasChanged = true;
                }
            }
            Debug.Log("");

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
