using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour {

    public Texture2D CursorTexture;

    public void RevealCursor() {
        Cursor.SetCursor(CursorTexture, new Vector2(0, 0), CursorMode.ForceSoftware);
        CursorToggler.ToggleCursor(true);
    }
}
