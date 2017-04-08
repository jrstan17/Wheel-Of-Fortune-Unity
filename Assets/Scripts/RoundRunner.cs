using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoundRunner : MonoBehaviour {

    public TextAsset DataTextFile;
    public List<GameObject> UsedLetterObjects;
    public GameObject[] WheelCanvases;
    public GameObject MenuCanvas;
    public GameObject SolveCanvas;
    public GameObject CategoryTextObject;
    public KeyPress KeyPress;
    public GameObject AudioSource;
    public GameObject RegularRoundButtonsObject;
    public GameObject BonusRoundButtonsObject;
    public InputField BonusInputText;
    public GameObject PlayerBar;
    public GameObject SajackPanel;
    public GameObject Background;

    private PuzzleFactory factory;
    internal static Puzzle Puzzle;
    internal BoardFiller boardFiller;
    internal GameObject WheelCanvas;
    internal Text SajakText;
    internal List<Text> UsedLetterText = new List<Text>();
    internal List<char> UsedLetters = new List<char>();
    private Text CategoryText;
    internal AudioTracks AudioTracks;
    internal bool IsBonusRound = false;
    internal bool IsTimeForLetter = false;
    internal int VowelPurchaseCost = 250;
    internal bool ShouldBeVowel = false;
    internal int RoundNumber = 0;
    internal int MaxRounds = 0;
    internal bool NotifiedOfRemainingLetters = false;

    internal static WedgeData CurrentWedge;
    private IEnumerator coroutine;

    void Start() {
        List<Player> Players = PlayerList.Players;
        Players.Add(new Player("Jason"));
        //Players.Add(new Player("Philip"));
        Players.Add(new Player("David"));
        //Players.Add(new Player("Leslie"));

        MaxRounds = Players.Count + 1;

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
        SajakText = SajackPanel.transform.GetChild(0).GetComponent<Text>();
        boardFiller.AudioTracks = AudioTracks;

        WheelCanvas = WheelCanvases[GetWheelIndex()];
        SpinWheel spinWheel = WheelCanvas.transform.GetChild(0).transform.GetChild(0).GetComponent<SpinWheel>();
        spinWheel.Randomize();

        NewBoard(false);
    }

    public void NewBoard(bool isBonus) {
        RoundNumber++;
        NotifiedOfRemainingLetters = false;

        IsBonusRound = isBonus;
        ShouldBeVowel = false;
        GotoNextPlayer();

        SetRoundColors();

        PlayerList.TransferRoundToTotalForCurrentPlayer();
        foreach (Player p in PlayerList.Players) {
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
            PlayerBar.SetActive(false);
            Puzzle = factory.NewPuzzle(RoundType.Bonus);
            EventSystem.current.SetSelectedGameObject(BonusInputText.gameObject);
        } else {
            RegularRoundButtonsObject.SetActive(true);
            BonusRoundButtonsObject.SetActive(false);
            PlayerBar.SetActive(true);
            ToggleUIButtonsParsing("all", false);

            if (KeyPress.CustomText == null) {
                Puzzle = factory.NewPuzzle(RoundType.Regular);
            } else {
                Puzzle = factory.NewPuzzle(KeyPress.CustomText);
                KeyPress.CustomText = null;
            }

            AudioTracks.Play("reveal");
        }

        CategoryText.text = Puzzle.Category;
        SajakText.text = "The category is " + Puzzle.Category + ".";

        boardFiller.InitBoard();
        WheelCanvas = WheelCanvases[GetWheelIndex()];
        SpinWheel spinWheel = WheelCanvas.transform.GetChild(0).transform.GetChild(0).GetComponent<SpinWheel>();
        spinWheel.Randomize();

        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    private int GetWheelIndex() {
        if (RoundNumber == 1) {
            return 0;
        } else if (RoundNumber == MaxRounds) {
            return WheelCanvases.Length - 1;
        }

        float WheelsPerRound = (float)(WheelCanvases.Length - 2) / (MaxRounds - 2);
        int toReturn = (int)Mathf.Round(WheelsPerRound * RoundNumber);

        if (toReturn == WheelCanvases.Length - 1 && RoundNumber != MaxRounds) {
            return WheelCanvases.Length - 2;
        } else {
            return toReturn;
        }        
    }

    private void SetRoundColors() {
        int colorIndex;

        if (RoundNumber == Utilities.RoundColors.Count + 1) {
            colorIndex = 0;
        } else {
            colorIndex = RoundNumber - 1;
        }

        Color color = Utilities.RoundColors[colorIndex];
        Background.gameObject.GetComponent<Renderer>().material.SetColor("_Color", color);

        SajackPanel.GetComponent<Image>().color = new Color(color.r, color.g, color.b, 0.5f);

        Color32 bright = new Color32(0, 0, 0, 255);
        if (color.r != 0) {
            bright.r = 255;
        }
        if (color.g != 0) {
            bright.g = 255;
        }
        if (color.b != 0) {
            bright.b = 255;
        }

        SajakText.color = bright;

        Color32 screen = new Color32(0, 0, 0, 255);
        if (color.r != 0) {
            screen.r = 128;
        }
        if (color.g != 0) {
            screen.g = 128;
        }
        if (color.b != 0) {
            screen.b = 128;
        }

        boardFiller.ScreenColor = screen;
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

    public void SolvedCorrectly() {
        SolveCanvas.SetActive(false);
        AudioTracks.Play("round_win");
        boardFiller.RevealBoard();

        string pieceOne = Utilities.RandomString(new string[] { "Congratulations", "Absolutely", "Great job", "Fantastic", "Extraordinary" });

        SajakText.text = pieceOne + ", " + PlayerList.CurrentPlayer.Name + ". ";

        SajakText.text += "You've won " + PlayerList.CurrentPlayer.RoundWinnings.ToString("C0") + " for this round! Onto Round " + (RoundNumber + 1) + "!";

        coroutine = WaitForNewRound(7f);
        StartCoroutine(coroutine);        
    }

    public IEnumerator WaitForNewRound(float time) {
        yield return new WaitForSeconds(time);
        NewBoard(false);
    }

    public void SolvedIncorrectly(bool isOutOfTime) {        
        SolveCanvas.SetActive(false);
        AudioTracks.Play("buzzer");
        SajakText.text = "I'm sorry, " + PlayerList.CurrentPlayer.Name + ". ";
        GotoNextPlayer();

        if (isOutOfTime) {
            SajakText.text += "You're out of time. Let's give " + PlayerList.CurrentPlayer.Name + " a";
        } else {
            string statement = Utilities.RandomString(new string[] { "That is incorrect.", "That is not correct.", "That's not right.", "You didn't solve it correctly.", "That's not the right answer." });
            SajakText.text +=  statement + " Let's give " + PlayerList.CurrentPlayer.Name + " a";
        }

        string chance = Utilities.RandomString(new string[] { " try.", " chance.", "n opportunity." });
        SajakText.text += chance;
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
        ShouldBeVowel = false;
        KeyPress.isWheelActive = true;
        WheelCanvas.SetActive(true);
    }

    public void Solve_Clicked() {
        ToggleUIButtonsParsing("all", false);
        SolveCanvas.SetActive(true);
    }

    public void Buy_Clicked() {
        ToggleUIButtonsParsing("all", false);
        IsTimeForLetter = true;
        ShouldBeVowel = true;
        PlayerList.CurrentPlayer.RoundWinnings -= VowelPurchaseCost;
    }

    public void WheelWindowClosed() {
        Debug.Log(CurrentWedge.Text);

        ToggleUIButtonsParsing("all", false);
        WedgeType CurrentType = CurrentWedge.WedgeType;

        if (CurrentType == WedgeType.Bankrupt) {
            AudioTracks.Play("bankrupt");
            SajakText.text = "You're bankrupt, " + PlayerList.CurrentPlayer.Name + ". I'm so sorry.";
            PlayerList.CurrentPlayer.RoundWinnings = 0;
            GotoNextPlayer();
            SajakText.text += " It's your turn, " + PlayerList.CurrentPlayer.Name + ".";
        } else if (CurrentType == WedgeType.LoseATurn) {
            AudioTracks.Play("buzzer");
            SajakText.text = "You've lost your turn, " + PlayerList.CurrentPlayer.Name + ".";
            GotoNextPlayer();
            SajakText.text += " It's now your turn, " + PlayerList.CurrentPlayer.Name + ".";
        } else if (CurrentType == WedgeType.HighAmount) {
            SajakText.text = CurrentWedge.Value.ToString("C0") + "! Now make your guess count!";
            IsTimeForLetter = true;
        } else if (CurrentType == WedgeType.TenThousand) {
            SajakText.text = "Oh Wow! " + CurrentWedge.Value.ToString("C0") + "! Good Luck!";
            IsTimeForLetter = true;
        } else if (CurrentType == WedgeType.FreePlay) {
            SajakText.text = "You have yourself a Free Play, " + PlayerList.CurrentPlayer.Name + ". The current value is " + CurrentWedge.Value + ".";
            IsTimeForLetter = true;
        } else {
            IsTimeForLetter = true;
            SajakText.text = CurrentWedge.Value + ".";
        }
    }

    public void GotoNextPlayer() {
        PlayerList.GotoNextPlayer();
        ToggleUIButtons();
    }

    public void ToggleUIButtons() {
        ToggleUIButtonsParsing("all", false);

        if (PlayerList.CurrentPlayer.RoundWinnings >= VowelPurchaseCost) {
            ToggleUIButtonsParsing("all", true);
        } else {
            ToggleUIButtonsParsing("spin solve", true);
        }

        if (boardFiller.PuzzleContainsOnly(LetterType.Both)) {
            ToggleUIButtonsParsing("spin buy", false);
            ToggleUIButtonsParsing("solve", true);
        } else if (boardFiller.PuzzleContainsOnly(LetterType.Vowel)) {
            ToggleUIButtonsParsing("spin", false);
        } else if (boardFiller.PuzzleContainsOnly(LetterType.Consonant)) {
            ToggleUIButtonsParsing("buy", false);
        }
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

    public int FindHowManyToReveal(List<char> letters) {
        for(int i = 0; i < letters.Count; i++) {
            letters[i] = char.ToUpper(letters[i]);
        }

        int toReturn = 0;
        for (int i = 0; i < letters.Count; i++) {
            for (int j = 0; j < Puzzle.Text.Length; j++) {
                if (letters[i] == Puzzle.Text[j]) {
                    toReturn++;
                }
            }
        }

        return toReturn;
    }

    public IEnumerator LetterPressed(char letter) {
        if (IsTimeForLetter) {
            IsTimeForLetter = false;
            int trilonsRevealed = 0;

            List<char> letters = new List<char>();
            letters.Add(letter);

            //if it's a consonant
            if (!ShouldBeVowel && !Utilities.IsVowel(letter) && !UsedLetters.Contains(letter)) {

                BoardFiller.LettersRevealed = FindHowManyToReveal(letters);
                trilonsRevealed = BoardFiller.LettersRevealed;

                if (!UsedLetters.Contains(letter) && trilonsRevealed > 0) {
                    UsedLetterText[letter - 97].color = Constants.USED_LETTER_DISABLED_COLOR;
                    int totalValue = CurrentWedge.Value * trilonsRevealed;
                    PlayerList.CurrentPlayer.RoundWinnings += totalValue;

                    if (trilonsRevealed == 1) {
                        SajakText.text = "There is 1 " + char.ToUpper(letter);
                    } else {
                        SajakText.text = "There are " + trilonsRevealed + " " + char.ToUpper(letter) + "'s";
                    }

                    SajakText.text += " for a value of " + totalValue.ToString("C0");
                    if (CurrentWedge.WedgeType == WedgeType.HighAmount) {
                        SajakText.text += "!";
                    } else {
                        SajakText.text += ".";
                    }
                } else {
                    GotoNextPlayer();
                    SajakText.text = "There are no " + char.ToUpper(letter) + "'s. It's your turn, " + PlayerList.CurrentPlayer.Name + ".";
                    AudioTracks.Play("buzzer");
                }

                UsedLetters.Add(letter);
                yield return StartCoroutine(boardFiller.RevealLetters(letters));
            } //if it's a vowel
            else if (ShouldBeVowel && Utilities.IsVowel(letter) && !UsedLetters.Contains(letter)) {
                yield return StartCoroutine(boardFiller.RevealLetters(letters));
                trilonsRevealed = BoardFiller.LettersRevealed;

                if (!UsedLetters.Contains(letter) && trilonsRevealed > 0) {
                    UsedLetterText[letter - 97].color = Constants.USED_LETTER_DISABLED_COLOR;

                    if (trilonsRevealed == 1) {
                        SajakText.text = "There is 1 " + char.ToUpper(letter);
                    } else {
                        SajakText.text = "There are " + trilonsRevealed + " " + char.ToUpper(letter) + "'s";
                    }

                    if (CurrentWedge.WedgeType == WedgeType.HighAmount) {
                        SajakText.text += "!";
                    } else {
                        SajakText.text += ".";
                    }
                } else {
                    GotoNextPlayer();
                    SajakText.text = "There are no " + char.ToUpper(letter) + "'s. It's your turn, " + PlayerList.CurrentPlayer.Name + ".";
                    AudioTracks.Play("buzzer");
                }

                UsedLetters.Add(letter);
            } else {
                SajakText.text = "I'm sorry, " + PlayerList.CurrentPlayer.Name + ". " + char.ToUpper(letter) + " has already been used. ";
                GotoNextPlayer();
                SajakText.text += "It's now " + PlayerList.CurrentPlayer.Name + "'s turn.";
                AudioTracks.Play("buzzer");
            }

            ShouldBeVowel = false;
        }

        yield return 0;
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

            if (PlayerList.CurrentPlayer != null && PlayerList.CurrentPlayer.Name.Equals(nameText.text)) {
                PlayerBar.transform.GetChild(i).gameObject.GetComponent<Image>().color = SajakText.color;
                nameText.color = Color.black;
                winningText.color = Color.black;
            } else {
                PlayerBar.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.clear;
                nameText.color = new Color32(255, 255, 255, 125);
                winningText.color = new Color32(255, 255, 255, 125);
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