using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealCanvas : MonoBehaviour {

    public GameObject NewGamePanel;
    public GameObject TitleButtonPanel;
    public SpriteRenderer TitleRenderer;
    public SpriteRenderer CreditRenderer;
    public Texture2D CursorTexture;

    public void Do() {
        Cursor.SetCursor(CursorTexture, new Vector2(0, 0), CursorMode.ForceSoftware);
        CursorToggler.ToggleCursor(true);
    }

    public void NewGame_Clicked() {
        NewGamePanel.SetActive(true);
        TitleRenderer.enabled = false;
        CreditRenderer.enabled = false;
        TitleButtonPanel.SetActive(false);
    }

    public void Exit_Clicked() {
        Application.Quit();
    }
}
