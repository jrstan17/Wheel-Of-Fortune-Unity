using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsRunner : MonoBehaviour {

    public RoundRunner RoundRunner;
    public RevealCanvas RevealCanvas;

    public Text RoundNumberValueText;
    public Slider RoundNumberSlider;

    private void OnEnable() {
        LoadValues();
    }

    public void RoundNumber_Changed() {
        RoundNumberValueText.text = RoundNumberSlider.value.ToString();
    }

    public void ApplyAndClose_Clicked() {
        SaveAndApplyValues();
        RevealCanvas.CloseOptions();
    }

    public void Close_Clicked() {
        RevealCanvas.CloseOptions();
    }

    private void LoadValues() {
        RoundNumberSlider.value = PlayerPrefs.GetFloat("roundNumber_Value", 4f);
    }

    private void SaveAndApplyValues() {
        RoundRunner.MaxRounds = (int) RoundNumberSlider.value;
        PlayerPrefs.SetFloat("roundNumber_Value", RoundNumberSlider.value);
        PlayerPrefs.Save();
    }
}
