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
    public GameObject NextRoundCanvas;
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
    public WedgeController WedgeController;

    internal PuzzleFactory factory;
    internal static Puzzle Puzzle;
    internal GameObject WheelCanvas;
    internal List<Text> PlayerWinningsTexts;
    internal Text SajakText;
    internal List<Text> UsedLetterText = new List<Text>();
    internal List<char> UsedLetters = new List<char>();
    internal Text CategoryText;
    internal AudioTracks AudioTracks;
    internal bool IsBonusRound = false;
    internal bool IsTimeForLetter = false;
    internal int VowelPurchaseCost = 250;
    internal bool ShouldBeVowel = false;
    internal int RoundNumber = 0;
    internal int MaxRounds = 0;
    internal bool NotifiedOfRemainingLetters = false;
    internal bool IsRoundEnded = false;
    internal BoardFiller BoardFiller;

    internal static WedgeData CurrentWedge;
    internal Coroutine coroutine;

    void Start() {
        PlayerList.Players.Add(new Player("Jason"));
        PlayerList.Players.Add(new Player("Philip"));
        PlayerList.Players.Add(new Player("David"));
        PlayerList.Players.Add(new Player("Leslie"));
        PlayerList.RandomizePlayers();

        MaxRounds = PlayerList.Players.Count + 1;

        GameObject panel = GameObject.FindGameObjectWithTag("PlayerPanel");
        PlayerWinningsTexts = new List<Text>();
        foreach (Player p in PlayerList.Players) {
            GameObject panelClone = Instantiate(panel);
            Text NameText = panelClone.transform.GetChild(0).GetComponent<Text>();
            NameText.text = p.Name;
            Text WinningsText = panelClone.transform.GetChild(1).GetComponent<Text>();
            PlayerWinningsTexts.Add(WinningsText);
            panelClone.transform.SetParent(PlayerBar.transform, false);
        }

        factory = new PuzzleFactory(DataTextFile);
        BoardFiller = gameObject.GetComponent<BoardFiller>();

        foreach (GameObject obj in UsedLetterObjects) {
            Text text = obj.GetComponent<Text>();
            UsedLetterText.Add(text);
        }

        CategoryText = CategoryTextObject.GetComponent<Text>();
        AudioTracks = AudioSource.GetComponent<AudioTracks>();
        SajakText = SajackPanel.transform.GetChild(0).GetComponent<Text>();

        NewBoard(false);
    }

    public void NewBoard(bool isBonus) {
        RoundNumber++;
        IsRoundEnded = false;
        NotifiedOfRemainingLetters = false;

        IsBonusRound = isBonus;
        ShouldBeVowel = false;
        GotoNextPlayer();

        SetRoundColors();

        foreach (Text text in UsedLetterText) {
            text.color = Constants.USED_LETTER_ENABLED_COLOR;
        }
        UsedLetters = new List<char>();

        if (isBonus) {
            BonusRoundRunner brn = GameObject.FindGameObjectWithTag("BonusRoundRunner").GetComponent<BonusRoundRunner>();
            StartCoroutine(brn.Run());
            return;
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
        SajakText.text = "The category is " + Puzzle.Category;
        if (char.IsLetter(Puzzle.Category[Puzzle.Category.Length - 1])) {
            SajakText.text += ".";
        }

        bool success = BoardFiller.InitBoard();
        if (!success) {
            return;
        }

        Debug.Log(GetWheelIndex());
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

        if (toReturn >= WheelCanvases.Length) {
            return toReturn / 2;
        }

        if (toReturn == WheelCanvases.Length - 1 && RoundNumber != MaxRounds) {
            return WheelCanvases.Length - 2;
        } else {
            return toReturn;
        }
    }

    internal void SetRoundColors() {
        int colorIndex;

        if (RoundNumber == Utilities.RoundColors.Count + 1) {
            colorIndex = 0;
        } else {
            colorIndex = RoundNumber - 1;
        }

        SetRoundColorsImplementation(colorIndex);
    }

    internal void SetRoundColors(int colorIndex) {
        SetRoundColorsImplementation(colorIndex);
    }

    private void SetRoundColorsImplementation(int colorIndex) {
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

        BoardFiller.ScreenColor = screen;
        BoardFiller.RefreshBoardColor();
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

    public IEnumerator SolvedCorrectly() {
        IsRoundEnded = true;
        SolveCanvas.GetComponent<Canvas>().enabled = false;

        if (PlayerList.CurrentPlayer.RoundWinnings < 1000) {
            PlayerList.CurrentPlayer.RoundWinnings = 1000;
        }

        AudioTracks.Play("round_win");
        StartCoroutine(BoardFiller.RevealBoard());

        string pieceOne = Utilities.RandomString(new string[] { "Congratulations", "Absolutely", "Great job", "Fantastic", "Extraordinary" });

        Debug.Log(pieceOne);

        SajakText.text = pieceOne + ", " + PlayerList.CurrentPlayer.Name + ". ";

        SajakText.text += "You've won " + PlayerList.CurrentPlayer.RoundWinnings.ToString("C0") + " for Round " + RoundNumber + "!";

        yield return new WaitForSeconds(5f);

        PlayerList.TransferRoundToTotalForCurrentPlayer();
        HighlightCurrentlyWinningPlayerText();

        SajakText.text = "Here are the totals won so far.";
        yield return new WaitForSeconds(3.5f);

        if (RoundNumber != MaxRounds) {
            SajakText.text = PlayerList.WinningPlayer().Name + " is currently in the lead with " + PlayerList.WinningPlayer().TotalWinnings.ToString("C0") + "!";
        } else {
            SajakText.text = PlayerList.WinningPlayer().Name + ", you have won with " + PlayerList.WinningPlayer().TotalWinnings.ToString("C0") + "! You're going to the Bonus Round!";
        }

        Text buttonText = NextRoundCanvas.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>();
        buttonText.text = "CONTINUE TO\nROUND " + (RoundNumber + 1);

        NextRoundCanvas.SetActive(true);
        SolveCanvas.SetActive(false);
        SolveCanvas.GetComponent<Canvas>().enabled = true;
    }

    public void HighlightCurrentlyWinningPlayerText() {
        int indexHighest = 0;
        int maxMoney = 0;
        for (int i = 0; i < PlayerList.Players.Count; i++) {
            Player p = PlayerList.Players[i];
            p.RoundWinnings = 0;
            p.FreePlays = 0;
            PlayerWinningsTexts[i].text = p.TotalWinnings.ToString("C0");
            if (p.TotalWinnings >= maxMoney) {
                maxMoney = p.TotalWinnings;
                indexHighest = i;
            }
        }
        for (int i = 0; i < PlayerList.Players.Count; i++) {
            Text nameText = PlayerBar.transform.GetChild(i).transform.GetChild(0).gameObject.GetComponent<Text>();
            Text winningText = PlayerBar.transform.GetChild(i).transform.GetChild(1).gameObject.GetComponent<Text>();

            if (i == indexHighest) {
                PlayerBar.transform.GetChild(indexHighest).gameObject.GetComponent<Image>().color = SajakText.color;
                nameText.color = Color.black;
                winningText.color = Color.black;
            } else {
                PlayerBar.transform.GetChild(i).gameObject.GetComponent<Image>().color = Color.clear;
                nameText.color = new Color32(255, 255, 255, 125);
                winningText.color = new Color32(255, 255, 255, 125);
            }
        }
    }

    public void SolvedIncorrectly(bool isOutOfTime) {
        SolveCanvas.SetActive(false);
        AudioTracks.Play("buzzer");
        string pre = "I'm sorry, " + PlayerList.CurrentPlayer.Name + ". ";
        string chance = Utilities.RandomString(new string[] { " try.", " chance.", "n opportunity." });

        if (isOutOfTime) {
            string yes = pre + "You're out of time.";
            string no = yes + " Let's give " + PlayerList.NextPlayersName() + " a" + chance;
            StartCoroutine(AskIfFreePlay(yes, no));
        } else {
            string statement = Utilities.RandomString(new string[] { "That is incorrect.", "That is not correct.", "That's not right.", "You didn't solve it correctly.", "That's not the right answer." });
            string yes = pre + statement;
            string no = yes + " Let's give " + PlayerList.NextPlayersName() + " a" + chance;
            StartCoroutine(AskIfFreePlay(yes, no));
        }
    }

    public void BonusTextField_Changed() {
        string toReturn = "";

        foreach (char c in BonusInputText.text) {
            toReturn += char.ToUpper(c);
        }

        BonusInputText.text = toReturn;
    }

    public void Continue_Clicked() {
        NextRoundCanvas.SetActive(false);
        NewBoard(false);
    }

    public void Exit_Clicked() {
        Application.Quit();
    }

    public void Reveal_Clicked() {
        MenuCanvas.SetActive(false);
        KeyPress.isMenuActive = true;
        AudioTracks.Play("round_win");
        StartCoroutine(BoardFiller.RevealBoard());
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

        BonusRoundRunner brn = GameObject.FindGameObjectWithTag("BonusRoundRunner").GetComponent<BonusRoundRunner>();
        StartCoroutine(brn.LettersSubmitted(inputedList));
    }

    public void WheelWindowClosed() {
        Debug.Log(CurrentWedge.Text);

        ToggleUIButtonsParsing("all", false);
        WedgeType CurrentType = CurrentWedge.WedgeType;

        if (CurrentType == WedgeType.Bankrupt) {
            OnBankrupt(PlayerList.CurrentPlayer);
            GotoNextPlayer();
            SajakText.text += " It's your turn, " + PlayerList.CurrentPlayer.Name + ".";
        } else if (CurrentType == WedgeType.LoseATurn) {
            AudioTracks.Play("buzzer");
            string yes = "You've lost your turn, " + PlayerList.CurrentPlayer.Name + ".";
            string no = yes + " It's now your turn, " + PlayerList.NextPlayersName() + ".";
            StartCoroutine(AskIfFreePlay(yes, no));
        } else if (CurrentType == WedgeType.Million) {
            //AudioTracks.Play("buzzer");
            SajakText.text = "Million Dollar Wedge! Now make your guess count!";            
            IsTimeForLetter = true;
        } else if (CurrentType == WedgeType.Prize) {
            SajakText.text = "Prize Wedge! Now make your guess count!";
            IsTimeForLetter = true;
        } else if (CurrentType == WedgeType.HighAmount) {
            SajakText.text = CurrentWedge.Value.ToString("C0") + "! Now make your guess count!";
            IsTimeForLetter = true;
        } else if (CurrentType == WedgeType.TenThousand) {
            SajakText.text = "Oh Wow! " + CurrentWedge.Value.ToString("C0") + "! Good Luck!";
            IsTimeForLetter = true;
        } else if (CurrentType == WedgeType.FreePlay) {
            SajakText.text = "You have yourself a Free Play, " + PlayerList.CurrentPlayer.Name + ". The current value is " + CurrentWedge.Value + ".";
            PlayerList.CurrentPlayer.FreePlays++;
            IsTimeForLetter = true;
        } else {
            IsTimeForLetter = true;
            SajakText.text = CurrentWedge.Value + ".";
        }
    }

    public void OnBankrupt(Player p) {
        AudioTracks.Play("bankrupt");
        SajakText.text = "You're bankrupt, " + p.Name + ". I'm so sorry.";
        p.RoundWinnings = 0;
        p.RoundPrize = null;
        p.HasMillionWedge = false;
        p.FreePlays = 0;
    }

    public void GotoNextPlayer() {
        PlayerList.GotoNextPlayer();
        ToggleUIButtons();
    }

    public void ToggleUIButtons() {
        ToggleUIButtonsParsing("all", false);

        if (KeyPress.IsTimeForFreePlayDecision) {
            return;
        }

        if (PlayerList.CurrentPlayer.RoundWinnings >= VowelPurchaseCost) {
            ToggleUIButtonsParsing("all", true);
        } else {
            ToggleUIButtonsParsing("spin solve", true);
        }

        if (BoardFiller.PuzzleContainsOnly(LetterType.Both)) {
            ToggleUIButtonsParsing("spin buy", false);
            ToggleUIButtonsParsing("solve", true);
        } else if (BoardFiller.PuzzleContainsOnly(LetterType.Vowel)) {
            ToggleUIButtonsParsing("spin", false);
        } else if (BoardFiller.PuzzleContainsOnly(LetterType.Consonant)) {
            ToggleUIButtonsParsing("buy", false);
        }
    }

    public void ToggleUIButtonsParsing(string buttons, bool enable) {
        string[] splits = buttons.Split(' ');

        foreach (string str in splits) {
            if (str.Equals("spin")) {
                Button b = RegularRoundButtonsObject.transform.GetChild(0).GetComponent<Button>();
                b.interactable = enable;
            } else if (str.Equals("buy")) {
                Button b = RegularRoundButtonsObject.transform.GetChild(1).GetComponent<Button>();
                b.interactable = enable;
            } else if (str.Equals("solve")) {
                Button b = RegularRoundButtonsObject.transform.GetChild(2).GetComponent<Button>();
                b.interactable = enable;
            } else if (str.Equals("all")) {
                for (int i = 0; i < RegularRoundButtonsObject.transform.childCount; i++) {
                    Button b = RegularRoundButtonsObject.transform.GetChild(i).GetComponent<Button>();
                    b.interactable = enable;
                }
            }
        }
    }

    public int FindHowManyToReveal(List<char> letters) {
        for (int i = 0; i < letters.Count; i++) {
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

            bool IsVowel = Utilities.IsVowel(letter);

            if (!UsedLetters.Contains(letter) && (!ShouldBeVowel && !IsVowel || ShouldBeVowel && IsVowel)) {

                BoardFiller.LettersRevealed = FindHowManyToReveal(letters);
                trilonsRevealed = BoardFiller.LettersRevealed;

                if (!UsedLetters.Contains(letter) && trilonsRevealed > 0) {
                    if (trilonsRevealed == 1) {
                        SajakText.text = "There is 1 " + char.ToUpper(letter);
                    } else {
                        SajakText.text = "There are " + trilonsRevealed + " " + char.ToUpper(letter) + "'s";
                    }

                    int totalValue = 0;
                    if (!IsVowel) {
                        totalValue = CurrentWedge.Value * trilonsRevealed;
                        PlayerList.CurrentPlayer.RoundWinnings += totalValue;

                        SajakText.text += " for a value of " + totalValue.ToString("C0");
                        if (CurrentWedge.WedgeType == WedgeType.HighAmount) {
                            SajakText.text += "!";
                        } else {
                            SajakText.text += ".";
                        }
                    } else {
                        SajakText.text += ".";
                    }                    
                } else {
                    AudioTracks.Play("buzzer");
                    yield return StartCoroutine(AskIfFreePlay("There are no " + char.ToUpper(letter) + "'s.", "There are no " + char.ToUpper(letter) + "'s. It's your turn, " + PlayerList.NextPlayersName() + "."));                    
                }

                UsedLetters.Add(letter);
                UsedLetterText[letter - 97].color = Constants.USED_LETTER_DISABLED_COLOR;
                if (!KeyPress.IsTimeForFreePlayDecision && trilonsRevealed > 0) {
                    yield return StartCoroutine(BoardFiller.RevealLetters(letters));

                    if (CurrentWedge.WedgeType == WedgeType.Prize && !PlayerList.CurrentPlayer.HasPrize()) {
                        PlayerList.CurrentPlayer.RoundPrize = new Prize("A Trip to Spain and Portugal	7800");
                        WedgeController.RemovePrizeWedge(GetWheelIndex());
                        SajakYouGotSomethingGood(PlayerList.CurrentPlayer.Name + " picks up the Prize wedge!");
                    } else if (CurrentWedge.WedgeType == WedgeType.Million && !PlayerList.CurrentPlayer.HasMillionWedge) {
                        PlayerList.CurrentPlayer.HasMillionWedge = true;
                        WedgeController.RemoveMillionWedge(GetWheelIndex());
                        SajakYouGotSomethingGood(PlayerList.CurrentPlayer.Name + " picks up the One Million wedge!");
                    }
                }
                ToggleUIButtons();
            } else {
                if (UsedLetters.Contains(letter)) {
                    string yes = "I'm sorry, " + PlayerList.CurrentPlayer.Name + ". " + char.ToUpper(letter) + " has already been used.";
                    string no = yes + " It's now " + PlayerList.NextPlayersName() + "'s turn.";
                    yield return StartCoroutine(AskIfFreePlay(yes, no));
                } else {
                    string yes = "I'm sorry, " + PlayerList.CurrentPlayer.Name + ", but that's the incorrect letter type.";
                    string no = yes + " It's now " + PlayerList.NextPlayersName() + "'s turn.";
                    yield return StartCoroutine(AskIfFreePlay(yes, no));
                }

                AudioTracks.Play("buzzer");                
            }

            ShouldBeVowel = false;
        }

        yield return 0;
    }

    private void SajakYouGotSomethingGood(string sajakText) {
        AudioTracks.Play("pq");
        SajakText.text = sajakText;
    }

    private IEnumerator AskIfFreePlay(string sajakFreePlayExists, string sajakFreePlayDoesNotExist) {
        if (PlayerList.CurrentPlayer.FreePlays != 0) {
            SajakText.text = sajakFreePlayExists;
            yield return new WaitForSeconds(3f);
            SajakForFreePlayQuestion();
            KeyPress.IsTimeForFreePlayDecision = true;
            ToggleUIButtonsParsing("all", false);
        } else {
            GotoNextPlayer();
            SajakText.text = sajakFreePlayDoesNotExist;
        }
    }

    void SajakForFreePlayQuestion() {
        int freePlays = PlayerList.CurrentPlayer.FreePlays;

        if (freePlays == 1) {
            SajakText.text = "You have a Free Play. Would you like to use it now? (Y/N)";
        } else if (freePlays > 1) {
            SajakText.text = "You have " + freePlays + " Free Plays. Would you like to use one now? (Y/N)";
        }
    }

    void Update() {
        if (!IsRoundEnded) {
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