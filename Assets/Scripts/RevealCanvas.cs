using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealCanvas : MonoBehaviour {

    public GameObject NewGamePanel;
    public GameObject TitleButtonPanel;
    public SpriteRenderer TitleRenderer;
    public SpriteRenderer CreditRenderer;
    public Texture2D CursorTexture;

    public GameObject OptionPanel;
    public GameObject MainPanels;

    public void NewGame_Clicked() {
        NewGamePanel.SetActive(true);
        TitleRenderer.enabled = false;
        CreditRenderer.enabled = false;
        TitleButtonPanel.SetActive(false);
    }

    public void Exit_Clicked() {
        Application.Quit();
    }

    public void Options_Clicked() {
        MainPanels.SetActive(false);
        OptionPanel.SetActive(true);
    }

    public void CloseOptions() {
        MainPanels.SetActive(true);
        OptionPanel.SetActive(false);
    }
}
