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

    public GameObject AudioSource;

    private PuzzleFactory factory;
    private Puzzle Puzzle;
    private BoardFiller boardFiller;
    private List<Text> UsedLetters = new List<Text>();
    private Text CategoryText;
    private AudioTracks AudioTracks;

    // Use this for initialization
    void Start() {
        factory = new PuzzleFactory(DataTextFile);
        boardFiller = gameObject.AddComponent<BoardFiller>();

        foreach(GameObject obj in UsedLetterObjects) {
            Text text = obj.GetComponent<Text>();
            UsedLetters.Add(text);
        }

        CategoryText = CategoryTextObject.GetComponent<Text>();
        AudioTracks = AudioSource.GetComponent<AudioTracks>();
        boardFiller.AudioTracks = AudioTracks;

        NewBoard();
    }

    public void NewBoard() {
        foreach(Text text in UsedLetters) {
            text.color = Constants.USED_LETTER_ENABLED_COLOR;
        }

        Puzzle = factory.NewPuzzle(RoundType.Regular);

        Debug.Log("Puzzle Solution: " + Puzzle.Text);

        boardFiller.InitBoard(Puzzle);
        CategoryText.text = Puzzle.Category;

        AudioTracks.Play("reveal");
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

    public void RevealRSTLNE_Clicked() {
        List<char> rstlne = new List<char>();
        rstlne.Add('R');
        rstlne.Add('S');
        rstlne.Add('T');
        rstlne.Add('L');
        rstlne.Add('N');
        rstlne.Add('E');
        boardFiller.RevealLetters(rstlne);
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
            List<char> letters = new List<char>();
            letters.Add(letter);
            boardFiller.RevealLetters(letters);
            UsedLetters[letter - 97].color = Constants.USED_LETTER_DISABLED_COLOR;
        }
    }

    void Update() {

    }
}