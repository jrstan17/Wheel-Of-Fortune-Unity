using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorToggler : MonoBehaviour {

    public float CursorVisibleTime;

    internal float Timer = 0;
    internal float StartTime = 0;

	// Use this for initialization
	void Start () {
        StartTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0 || MouseButtonDown()) {
            Cursor.visible = true;
            Timer = 0;
            StartTime = Time.time;
        } else {
            Timer = Time.time - StartTime;
        }

        if (Cursor.visible && Timer >= CursorVisibleTime) {
            Cursor.visible = false;
        }
    }

    bool MouseButtonDown() {
        for(int i = 0; i < 3; i++) {
            if (Input.GetMouseButtonDown(i)) {
                return true;
            }
        }

        return false;
    }
}
