using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour {

    public GameObject[] ToDestroy;

	public void Destroy() {
        foreach(GameObject go in ToDestroy) {
            Destroy(go);
        }
    }
}
