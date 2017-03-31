using UnityEngine;
using System.Collections;
using System.Text;

public class SortLayerExposer : MonoBehaviour {

    public string SortingLayerName = "Default";
    public int SortingOrder = 0;

    void Awake() {
        MeshRenderer mesh = gameObject.GetComponent<MeshRenderer>();

        if (mesh != null) {
            mesh.sortingLayerName = SortingLayerName;
            mesh.sortingOrder = SortingOrder;
        }
    }
}
