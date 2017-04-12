using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WedgeController : MonoBehaviour {
    public GameObject[] WheelObjects;

    public void RemoveMillionWedge(int wheelIndex) {
        Transform[] children = WheelObjects[wheelIndex].GetComponentsInChildren<Transform>();
        foreach (Transform child in children) {
            if (child.name.Equals("MillionContainer")) {
                child.gameObject.SetActive(false);
            }
        }

        NewWedgeEntered newWedgeEntered;
        children = WheelObjects[wheelIndex].GetComponentsInChildren<Transform>();
        foreach (Transform child in children) {
            if (child.name.Equals("Collider12")) {
                newWedgeEntered = child.gameObject.GetComponent<NewWedgeEntered>();
                newWedgeEntered.UseAlternativeWedge = true;
                break;
            }
        }
    }

    public void RemovePrizeWedge(int wheelIndex) {
        Transform[] children = WheelObjects[wheelIndex].GetComponentsInChildren<Transform>();
        foreach (Transform child in children) {
            if (child.name.Equals("W19A")) {
                child.gameObject.SetActive(false);
            }
        }

        NewWedgeEntered newWedgeEntered;
        children = WheelObjects[wheelIndex].GetComponentsInChildren<Transform>();
        foreach (Transform child in children) {
            if (child.name.Equals("Collider19")) {
                newWedgeEntered = child.gameObject.GetComponent<NewWedgeEntered>();
                newWedgeEntered.UseAlternativeWedge = true;
                break;
            }
        }
    }
}