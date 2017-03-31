﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoundRunner : MonoBehaviour {

    public TextAsset DataTextFile;
    public List<GameObject> UsedLetterObjects;

    public GameObject WheelCanvas;
    public GameObject MenuCanvas;

    private PuzzleFactory factory;
    private Puzzle Puzzle;
    private BoardFiller boardFiller;
    private List<TextMesh> UsedLetters = new List<TextMesh>();

    // Use this for initialization
    void Start() {
        factory = new PuzzleFactory(DataTextFile);
        Puzzle = factory.NewPuzzle(RoundType.Regular);
        boardFiller = new BoardFiller();

        foreach(GameObject obj in UsedLetterObjects) {
            TextMesh mesh = obj.GetComponent<TextMesh>();
            UsedLetters.Add(mesh);
        }

        NewBoard();
    }

    public void NewBoard() {
        foreach(TextMesh mesh in UsedLetters) {
            mesh.color = Constants.USED_LETTER_ENABLED_COLOR;
        }

        Puzzle = factory.NewPuzzle(RoundType.Regular);
        boardFiller.InitBoard(Puzzle);
    }

    public void NewPuzzle_Clicked() {
        NewBoard();
    }

    public void Exit_Clicked()
    {
        Application.Quit();
    }

    public void Reveal_Clicked() {
        boardFiller.RevealBoard();
    }

    public void Spin_Clicked() {
        ToggleUIButtons(false);
        WheelCanvas.SetActive(true);
    }

    public void ToggleUIButtons(bool enable) {
        GameObject buttons = GameObject.FindGameObjectWithTag("UIButtons");

        for(int i = 0; i < buttons.transform.childCount; i++) {
            Button b = buttons.transform.GetChild(i).GetComponent<Button>();
            b.enabled = enable;
        }
    }

    public void LetterPressed(char letter) {
        if (UsedLetters[letter - 97].color == Constants.USED_LETTER_ENABLED_COLOR) {
            boardFiller.RevealLetter(letter);
            UsedLetters[letter - 97].color = Constants.USED_LETTER_DISABLED_COLOR;
        }
    }

    void Update() {

    }
}