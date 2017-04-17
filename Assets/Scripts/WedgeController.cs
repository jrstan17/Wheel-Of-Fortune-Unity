using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WedgeController : MonoBehaviour {
    public GameObject[] WheelObjects;

    public void RemoveMillionWedge(int wheelIndex) {
        for (int i = wheelIndex; i < WheelObjects.Length; i++) {
            Transform[] children = WheelObjects[i].GetComponentsInChildren<Transform>();
            foreach (Transform child in children) {
                if (child.name.Equals("MillionContainer")) {
                    child.gameObject.SetActive(false);
                }
            }

            NewWedgeEntered newWedgeEntered;

            children = WheelObjects[i].GetComponentsInChildren<Transform>();
            foreach (Transform child in children) {
                if (child.name.Equals("Collider12")) {
                    newWedgeEntered = child.gameObject.GetComponent<NewWedgeEntered>();
                    newWedgeEntered.UseAlternativeWedge = true;
                    break;
                }
            }
        }
    }

    public void TogglePrizeWedge(int wheelIndex, bool enable) {
        Transform[] children = WheelObjects[wheelIndex].GetComponentsInChildren<Transform>();
        foreach (Transform child in children) {
            if (child.name.Equals("W19A")) {
                SpriteRenderer renderer = child.gameObject.GetComponent<SpriteRenderer>();
                renderer.enabled = enable;
                MeshRenderer mesh = child.GetChild(0).GetComponent<MeshRenderer>();
                mesh.enabled = enable;
            }
        }

        NewWedgeEntered newWedgeEntered;
        children = WheelObjects[wheelIndex].GetComponentsInChildren<Transform>();
        foreach (Transform child in children) {
            if (child.name.Equals("Collider19")) {
                newWedgeEntered = child.gameObject.GetComponent<NewWedgeEntered>();
                newWedgeEntered.UseAlternativeWedge = !enable;
                break;
            }
        }
    }
}