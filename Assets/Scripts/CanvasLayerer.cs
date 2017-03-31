using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLayerer : MonoBehaviour {

    public GameObject BoardCanvas;
    public GameObject WheelCanvas;
    public GameObject MenuCanvas;

    // Use this for initialization
    void Start () {
        BoardCanvas.transform.position = new Vector3(BoardCanvas.transform.position.x, BoardCanvas.transform.position.y, 0);

        WheelCanvas.transform.position = new Vector3(WheelCanvas.transform.position.x, WheelCanvas.transform.position.y, 1);

        MenuCanvas.transform.position = new Vector3(MenuCanvas.transform.position.x, MenuCanvas.transform.position.y, 2);
    }
}
