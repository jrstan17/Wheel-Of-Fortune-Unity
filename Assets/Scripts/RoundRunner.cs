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
    internal bool IsTimeForLetter = false;
    public InputField BonusInputText;
    public GameObject PlayerBar;

    private PuzzleFactory factory;
    private Puzzle Puzzle;
    internal BoardFiller boardFiller;
    internal List<Text> UsedLetterText = new List<Text>();
    internal List<char> UsedLetters = new List<char>();
    private Text CategoryText;
    internal AudioTracks AudioTracks;

    internal int VowelPurchaseCost = 250;

    internal static WedgeData CurrentWedge;

    void Start() {
        List<Player> Players = PlayerList.Players;
        Players.Add(new Player("Jason"));
        Players.Add(new Player("Philip"));
        Players.Add(new Player("David"));
        Players.Add(new Player("Leslie"));
        Players.Add(new Player("Mom"));
        Players.Add(new Player("Dad"));

        GameObject panel = GameObject.FindGameObjectWithTag("PlayerPanel");
        foreach (Player p in Players) {
            GameObject panelClone = Instantiate(panel);
            Text TextObj = panelClone.transform.GetChild(0).GetComponent<Text>();
            TextObj.text = p.Name;
            panelClone.transform.SetParent(PlayerBar.transform, false);
        }

        factory = new PuzzleFactory(DataTextFile);
        boardFiller = gameObject.AddComponent<BoardFiller>();

        foreach (GameObject obj in UsedLetterObjects) {
            Text text = obj.GetComponent<Text>();
            UsedLetterText.Add(text);
        }

        CategoryText = CategoryTextObject.GetComponent<Text>();
        AudioTracks = AudioSource.GetComponent<AudioTracks>();
        boardFiller.AudioTracks = AudioTracks;

        NewBoard(false);
    }

    public void NewBoard(bool isBonus) {
        IsBonusRound = isBonus;
        GotoNextPlayer();

        foreach(Player p in PlayerList.Players) {
            p.RoundWinnings = 0;
            p.FreePlays = 0;
        }

        foreach (Text text in UsedLetterText) {
            text.color = Constants.USED_LETTER_ENABLED_COLOR;
        }
        UsedLetters = new List<char>();

        if (isBonus) {
            BonusInputText.text = "";
            RegularRoundButtonsObject.SetActive(false);
            BonusRoundButtonsObject.SetActive(true);
            Puzzle = factory.NewPuzzle(RoundType.Bonus);
            EventSystem.current.SetSelectedGameObject(BonusInputText.gameObject);
        } else {
            RegularRoundButtonsObject.SetActive(true);
            BonusRoundButtonsObject.SetActive(false);
            ToggleUIButtonsParsing("all", false);
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

        foreach (char c in BonusInputText.text) {
            toReturn += char.ToUpper(c);
        }

        BonusInputText.text = toReturn;
    }

    public void SubmitLetters_Clicked() {
        List<char> inputedList = new List<char>();
        foreach (char c in BonusInputText.text) {
            if (char.IsLetter(c)) {
                char lower = char.ToLower(c);
                inputedList.Add(lower);
                UsedLetterText[lower - 97].color = Constants.USED_LETTER_DISABLED_COLOR;
                UsedLetters.Add(lower);
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
        ToggleUIButtonsParsing("all", false);
        KeyPress.isWheelActive = true;
        WheelCanvas.SetActive(true);
    }

    public void WheelWindowClosed() {
        Debug.Log(CurrentWedge.Text);

        ToggleUIButtonsParsing("all", false);
        WedgeType CurrentType = CurrentWedge.WedgeType;

        if (CurrentType == WedgeType.Bankrupt) {
            PlayerList.CurrentPlayer.RoundWinnings = 0;
            GotoNextPlayer();
        } else if (CurrentType == WedgeType.LoseATurn) {
            
        } else {
            IsTimeForLetter = true;
        }
    }

    public void GotoNextPlayer() {
        PlayerList.GotoNextPlayer();
        ToggleUIButtons();
    }

    public void ToggleUIButtons() {
        if (PlayerList.CurrentPlayer.RoundWinnings >= VowelPurchaseCost) {
            ToggleUIButtonsParsing("all", true);
        } else {
            ToggleUIButtonsParsing("spin solve", true);
        }
    }

    public void Answer_Clicked() {
        Debug.Log("Puzzle Solution: " + Puzzle.Text);
    }

    public void ToggleUIButtonsParsing(string buttons, bool enable) {
        GameObject buttonObj = GameObject.FindGameObjectWithTag("RegularRoundButtons");
        string[] splits = buttons.Split(' ');

        foreach (string str in splits) {
            if (str.Equals("spin")) {
                Button b = buttonObj.transform.GetChild(0).GetComponent<Button>();
                b.interactable = enable;
            } else if (str.Equals("buy")) {
                Button b = buttonObj.transform.GetChild(1).GetComponent<Button>();
                b.interactable = enable;
            } else if (str.Equals("solve")) {
                Button b = buttonObj.transform.GetChild(2).GetComponent<Button>();
                b.interactable = enable;
            } else if (str.Equals("all")) {
                for (int i = 0; i < buttonObj.transform.childCount; i++) {
                    Button b = buttonObj.transform.GetChild(i).GetComponent<Button>();
                    b.interactable = enable;
                }
            }
        }
    }

    public void LetterPressed(char letter) {
        if (IsTimeForLetter) {
            IsTimeForLetter = false;

            List<char> letters = new List<char>();
            letters.Add(letter);
            int trilonsRevealed = boardFiller.RevealLetters(letters);

            if (!UsedLetters.Contains(letter) && trilonsRevealed > 0) {
                UsedLetterText[letter - 97].color = Constants.USED_LETTER_DISABLED_COLOR;
                PlayerList.CurrentPlayer.RoundWinnings += CurrentWedge.Value * trilonsRevealed;
            } else {
                GotoNextPlayer();
                AudioTracks.Play("buzzer");
            }
        }
    }

    void Update() {
        if (Input.anyKeyDown) {
            if (Input.GetKey(KeyCode.Return) && IsInputFieldValid(BonusInputText.text)) {
                SubmitLetters_Clicked();
            }
        }

        for (int i = 0; i < PlayerBar.transform.childCount; i++) {
            Text nameText = PlayerBar.transform.GetChild(i).transform.GetChild(0).gameObject.GetComponent<Text>();
            Text winningText = PlayerBar.transform.GetChild(i).transform.GetChild(1).gameObject.GetComponent<Text>();

            if (PlayerList.CurrentPlayer.Name.Equals(nameText.text)) {
                PlayerBar.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.yellow;
                nameText.color = Color.black;
                winningText.color = Color.black;
            } else {
                PlayerBar.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.clear;
                nameText.color = Color.white;
                winningText.color = Color.white;
            }

            winningText.text = PlayerList.Players[i].RoundWinnings.ToString("C0");
        }
    }

    public bool IsInputFieldValid(string inputText) {
        if (inputText.Length != 4) {
            return false;
        }

        int vowelCount = 0;
        int consonantCount = 0;

        foreach (char c in inputText) {
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