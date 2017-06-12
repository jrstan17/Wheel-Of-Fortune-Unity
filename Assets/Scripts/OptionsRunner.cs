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

    public static int NumberOfRounds = 4;
    public static int VowelCost = 250;

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
    }

    private void SaveAndApplyValues() {
        NumberOfRounds = (int) RoundNumberSlider.value;
        VowelCost = int.Parse(VowelCostText.text);
        PlayerPrefs.SetInt("roundNumber_Value", (int) RoundNumberSlider.value);
        PlayerPrefs.SetInt("vowelCost_Value", int.Parse(VowelCostText.text));
        PlayerPrefs.Save();
    }
}
