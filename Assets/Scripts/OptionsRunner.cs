using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsRunner : MonoBehaviour {

    public RoundRunner RoundRunner;
    public RevealCanvas RevealCanvas;

    public Text RoundNumberValueText;
    public Slider RoundNumberSlider;

    public InputField VowelCostText;

    public Toggle FullScreenToggle;

    public static int NumberOfRounds = 4;
    public static int VowelCost = 250;
    public static bool IsFullScreen = true;

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
        NumberOfRounds = PlayerPrefs.GetInt("roundNumber_Value", 4);
        RoundNumberSlider.value = NumberOfRounds;

        VowelCost = PlayerPrefs.GetInt("vowelCost_Value", 250);
        VowelCostText.text = VowelCost.ToString();

        int fullScreenInt = PlayerPrefs.GetInt("fullScreen_Value", 1);
        if (fullScreenInt == 1) {
            FullScreenToggle.isOn = true;
        } else {
            FullScreenToggle.isOn = false;
        }
    }

    private void SaveAndApplyValues() {
        NumberOfRounds = (int) RoundNumberSlider.value;
        PlayerPrefs.SetInt("roundNumber_Value", (int)RoundNumberSlider.value);

        VowelCost = int.Parse(VowelCostText.text);
        PlayerPrefs.SetInt("vowelCost_Value", int.Parse(VowelCostText.text));

        if (FullScreenToggle.isOn) {
            Screen.fullScreen = true;
            PlayerPrefs.SetInt("fullScreen_Value", 1);
        } else {
            Screen.fullScreen = false;
            PlayerPrefs.SetInt("fullScreen_Value", 0);
        }

        PlayerPrefs.Save();
    }
}
