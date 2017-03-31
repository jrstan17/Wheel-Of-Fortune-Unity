using UnityEngine;
using System.Collections;
using System.Text;

public class SortLayerExposer : MonoBehaviour {

    public string SortingLayerName = "Default";
    public int SortingOrder = 0;

    void Awake() {
        gameObject.GetComponent<MeshRenderer>().sortingLayerName = SortingLayerName;
        gameObject.GetComponent<MeshRenderer>().sortingOrder = SortingOrder;
    }
}
