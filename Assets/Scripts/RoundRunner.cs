using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoundRunner : MonoBehaviour {

    public TextAsset DataTextFile;
    public List<GameObject> UsedLetterObjects;

    public GameObject WheelCanvas;
    public GameObject MenuCanvas;

    public GameObject CategoryTextObject;
    public GameObject KeyPressObject;

    private PuzzleFactory factory;
    private Puzzle Puzzle;
    private BoardFiller boardFiller;
    private List<Text> UsedLetters = new List<Text>();
    private Text CategoryText;

    // Use this for initialization
    void Start() {
        factory = new PuzzleFactory(DataTextFile);
        boardFiller = gameObject.AddComponent<BoardFiller>();

        foreach(GameObject obj in UsedLetterObjects) {
            Text text = obj.GetComponent<Text>();
            UsedLetters.Add(text);
        }

        CategoryText = CategoryTextObject.GetComponent<Text>();

        NewBoard();
    }

    public void NewBoard() {
        foreach(Text text in UsedLetters) {
            text.color = Constants.USED_LETTER_ENABLED_COLOR;
        }

        Puzzle = factory.NewPuzzle(RoundType.Regular);
        boardFiller.InitBoard(Puzzle);
        CategoryText.text = Puzzle.Category;
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

        KeyPress press = KeyPressObject.GetComponent<KeyPress>();
        press.isWheelActive = true;

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