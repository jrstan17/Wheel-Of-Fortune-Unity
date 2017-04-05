using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoundRunner : MonoBehaviour {

    public TextAsset DataTextFile;
    public List<GameObject> UsedLetterObjects;
    public GameObject WheelCanvas;
    public GameObject MenuCanvas;
    public GameObject CategoryTextObject;
    public KeyPress KeyPress;
    public GameObject AudioSource;
    public GameObject RegularRoundButtonsObject;
    public GameObject BonusRoundButtonsObject;
    internal bool IsBonusRound = false;
    public InputField BonusInputText;
    public GameObject PlayerBar;

    private PuzzleFactory factory;
    private Puzzle Puzzle;
    internal BoardFiller boardFiller;
    internal List<Text> UsedLetters = new List<Text>();
    private Text CategoryText;
    private AudioTracks AudioTracks;

    void Start() {
        List<Player> Players = PlayerList.Players;
        Players.Add(new Player("Jason"));
        Players.Add(new Player("Philip"));
        Players.Add(new Player("David"));
        Players.Add(new Player("Leslie"));
        Players.Add(new Player("Mom"));
        Players.Add(new Player("Dad"));
        GameObject panel = GameObject.FindGameObjectWithTag("PlayerPanel");
        foreach(Player p in Players) {
            GameObject panelClone = Instantiate(panel);
            Text TextObj = panelClone.transform.GetChild(0).GetComponent<Text>();
            TextObj.text = p.Name;
            panelClone.transform.SetParent(PlayerBar.transform, false);
        }

        factory = new PuzzleFactory(DataTextFile);
        boardFiller = gameObject.AddComponent<BoardFiller>();

        foreach (GameObject obj in UsedLetterObjects) {
            Text text = obj.GetComponent<Text>();
            UsedLetters.Add(text);
        }

        CategoryText = CategoryTextObject.GetComponent<Text>();
        AudioTracks = AudioSource.GetComponent<AudioTracks>();
        boardFiller.AudioTracks = AudioTracks;

        NewBoard(false);
    }

    public void NewBoard(bool isBonus) {
        IsBonusRound = isBonus;

        foreach (Text text in UsedLetters) {
            text.color = Constants.USED_LETTER_ENABLED_COLOR;
        }

        if (isBonus) {
            BonusInputText.text = "";
            RegularRoundButtonsObject.SetActive(false);
            BonusRoundButtonsObject.SetActive(true);
            Puzzle = factory.NewPuzzle(RoundType.Bonus);
            EventSystem.current.SetSelectedGameObject(BonusInputText.gameObject);
        } else {
            RegularRoundButtonsObject.SetActive(true);
            BonusRoundButtonsObject.SetActive(false);
            Puzzle = factory.NewPuzzle(RoundType.Regular);
            AudioTracks.Play("reveal");
        }

        CategoryText.text = Puzzle.Category;

        boardFiller.InitBoard(Puzzle);

        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void NewBonus_Clicked() {
        MenuCanvas.SetActive(false);
        KeyPress.isMenuActive = false;
        NewBoard(true);
    }

    public void NewRegular_Clicked() {
        MenuCanvas.SetActive(false);
        KeyPress.isMenuActive = false;
        NewBoard(false);
    }

    public void BonusTextField_Changed() {
        string toReturn = "";

        foreach(char c in BonusInputText.text) {
            toReturn += char.ToUpper(c);
        }

        BonusInputText.text = toReturn;
    }

    public void SubmitLetters_Clicked() {
        List<char> inputedList = new List<char>();
        foreach(char c in BonusInputText.text) {
            if (char.IsLetter(c)) {
                char lower = char.ToLower(c);
                inputedList.Add(lower);
                UsedLetters[lower - 97].color = Constants.USED_LETTER_DISABLED_COLOR;
            }
        }

        boardFiller.RevealLetters(inputedList);
    }

    public void Exit_Clicked() {
        Application.Quit();
    }

    public void Reveal_Clicked() {
        MenuCanvas.SetActive(false);
        KeyPress.isMenuActive = true;
        AudioTracks.Play("round_win");
        boardFiller.RevealBoard();
    }

    public void Spin_Clicked() {
        ToggleUIButtons(false);
        KeyPress.isWheelActive = true;
        WheelCanvas.SetActive(true);
    }

    public void RevealRSTLNE() {
        List<char> rstlne = new List<char>();
        rstlne.Add('r');
        rstlne.Add('s');
        rstlne.Add('t');
        rstlne.Add('l');
        rstlne.Add('n');
        rstlne.Add('e');

        foreach(char letter in rstlne) {
            UsedLetters[letter - 97].color = Constants.USED_LETTER_DISABLED_COLOR;
        }

        boardFiller.RevealLetters(rstlne);
    }

    public void Answer_Clicked() {
        Debug.Log("Puzzle Solution: " + Puzzle.Text);
    }

    public void ToggleUIButtons(bool enable) {
        GameObject buttons = GameObject.FindGameObjectWithTag("RegularRoundButtons");

        for (int i = 0; i < buttons.transform.childCount; i++) {
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
        if (Input.anyKeyDown) {
            if (Input.GetKey(KeyCode.Return) && IsInputFieldValid(BonusInputText.text)) {
                SubmitLetters_Clicked();
            }
        }
    }

    public bool IsInputFieldValid(string inputText) {
        if (inputText.Length != 4) {
            return false;
        }

        int vowelCount = 0;
        int consonantCount = 0;

        foreach(char c in inputText) {
            if (!char.IsLetter(c)) {
                return false;
            }

            if (Utilities.IsVowel(c)) {
                vowelCount++;
            } else {
                consonantCount++;
            }
        }

        return (vowelCount == 1 && consonantCount == 3);
    }
}