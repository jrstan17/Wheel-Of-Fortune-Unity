using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealCanvas : MonoBehaviour {

    public Canvas Canvas;
    public GameObject NewGamePanel;
    public GameObject TitleButtonPanel;
    public SpriteRenderer TitleRenderer;


	public void Do() {
        Canvas.gameObject.SetActive(true);
    }

    public void NewGame_Clicked() {
        NewGamePanel.SetActive(true);
        TitleRenderer.enabled = false;
        TitleButtonPanel.SetActive(false);
    }
}
