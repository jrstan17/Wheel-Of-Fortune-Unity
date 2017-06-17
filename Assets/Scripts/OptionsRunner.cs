﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsRunner : MonoBehaviour {

    public RoundRunner RoundRunner;
    public RevealCanvas RevealCanvas;

    public Text RoundNumberValueText;
    public Slider RoundNumberSlider;
    public Toggle AutoToggle;

    public InputField VowelCostText;

    public Toggle FullScreenToggle;

    public InputField XResolutionInput;
    public InputField YResolutionInput;

    public Dropdown QualityDropdown;

    public static int NumberOfRounds = 4;
    public static bool IsNumberOfRoundsOnAuto = true;
    public static int VowelCost = 250;
    public static bool IsFullScreen = true;
    public static int XResolution = Screen.width;
    public static int YResolution = Screen.height;
    public static int QualityIndex;

    private void OnEnable() {
        AutoToggle_OnChange();
        PopulateQualityDropdown();
        LoadValues();
    }

    public void RoundNumber_Changed() {
        RoundNumberValueText.text = RoundNumberSlider.value.ToString();
    }

    public void AutoToggle_OnChange() {
        if (AutoToggle.isOn) {
            RoundNumberSlider.enabled = false;
            RoundNumberValueText.text = "";
            IsNumberOfRoundsOnAuto = true;
        } else {
            RoundNumberSlider.enabled = true;
            RoundNumberValueText.text = RoundNumberSlider.value.ToString();
            IsNumberOfRoundsOnAuto = false;
        }
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
            IsFullScreen = true;
        } else {
            FullScreenToggle.isOn = false;
            IsFullScreen = false;
        }

        XResolution = PlayerPrefs.GetInt("xResolution_Value", Screen.width);
        XResolutionInput.text = XResolution.ToString();

        YResolution = PlayerPrefs.GetInt("yResolution_Value", Screen.height);
        YResolutionInput.text = YResolution.ToString();

        QualityIndex = PlayerPrefs.GetInt("qualityIndex_Value", QualitySettings.names.Length - 1);
        QualityDropdown.value = QualityIndex;

        int autoInt = PlayerPrefs.GetInt("autoRounds_Value", 1);
        if (autoInt == 1) {
            AutoToggle.isOn = true;
        } else {
            AutoToggle.isOn = false;
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
            IsFullScreen = true;
        } else {
            Screen.fullScreen = false;
            PlayerPrefs.SetInt("fullScreen_Value", 0);
            IsFullScreen = false;
        }

        XResolution = int.Parse(XResolutionInput.text);
        PlayerPrefs.SetInt("xResolution_Value", int.Parse(XResolutionInput.text));

        YResolution = int.Parse(YResolutionInput.text);
        PlayerPrefs.SetInt("yResolution_Value", int.Parse(YResolutionInput.text));
        Screen.SetResolution(XResolution, YResolution, IsFullScreen);

        QualityIndex = QualityDropdown.value;
        PlayerPrefs.SetInt("qualityIndex_Value", QualityIndex);
        QualitySettings.SetQualityLevel(QualityIndex);

        if (AutoToggle.isOn) {
            PlayerPrefs.SetInt("autoRounds_Value", 1);
        } else {
            PlayerPrefs.SetInt("autoRounds_Value", 0);
        }

        PlayerPrefs.Save();
    }

    private void PopulateQualityDropdown() {
        if (QualityDropdown.options.Count == 0) {
            string[] names = QualitySettings.names;
            List<Dropdown.OptionData> dataList = new List<Dropdown.OptionData>();

            foreach (string name in names) {
                dataList.Add(new Dropdown.OptionData(name));
            }

            QualityDropdown.AddOptions(dataList);
        }
    }
}