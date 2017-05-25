using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyPress : MonoBehaviour {

    public GameObject RoundRunnerObject;
    public GameObject MenuObject;
    public GameObject BoardObject;
    public GameObject DebugInputObject;
    public GameObject Background;
    public SpinWheel BonusWheelSpin;

    private RoundRunner RoundRunner;
    internal bool isMenuActive = false;
    internal bool isWheelActive = false;
    internal bool isBonusWheelActive = false;
    internal InputField DebugInputField;
    internal static bool IsTimeForFreePlayDecision = false;
    internal static bool IsTimeForWildDecision = false;
    internal static bool IsTimeForMysteryDecision = false;
    internal MysteryWedgeLanded mysteryWedgeLanded;
    internal Coroutine MysteryWedgeCoroutine;

    internal string[] Splits;
    List<string> History = new List<string>();
    int HistoryIndex = 0;
    internal string CustomText = null;

    // Use this for initialization
    void Start() {
        RoundRunner = RoundRunnerObject.GetComponent<RoundRunner>();
        DebugInputField = DebugInputObject.GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update() {
        if (Input.anyKeyDown) {

            if (Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.BackQuote)) {
                if (DebugInputObject.activeInHierarchy) {
                    DebugInputObject.SetActive(false);
                } else {
                    DebugInputObject.SetActive(true);
                    DebugInputField.text = "";
                    EventSystem.current.SetSelectedGameObject(DebugInputObject, null);
                    DebugInputField.OnPointerClick(new PointerEventData(EventSystem.current));
                    return;
                }
            }

            if (!DebugInputObject.activeInHierarchy) {
                StartCoroutine(IfNoDebugField());
            } else {
                if (Input.GetKey(KeyCode.Return)) {
                    History.Add(DebugInputField.text);
                    HistoryIndex = History.Count - 1;
                    ParseDebugSplits(DebugInputField.text.Split(' '));
                    DebugInputObject.SetActive(false);
                } else if (Input.GetKey(KeyCode.Escape)) {
                    DebugInputObject.SetActive(false);
                } else if (Input.GetKey(KeyCode.UpArrow)) {
                    if (History.Count != 0) {
                        DebugInputField.text = History[HistoryIndex];

                        if (HistoryIndex != 0) {
                            HistoryIndex--;
                        }
                    }
                } else if (Input.GetKey(KeyCode.DownArrow)) {
                    DebugInputField.text = History[HistoryIndex];

                    if (HistoryIndex != History.Count - 1) {
                        HistoryIndex++;
                    }
                }
            }
        } else if (IsTimeForFreePlayDecision) {
            if (Input.GetKeyUp(KeyCode.Y)) {
                IsTimeForFreePlayDecision = false;
                RoundRunner.SajakText.text = "Play again, " + PlayerList.CurrentPlayer.Name + ".";
                PlayerList.CurrentPlayer.FreePlays--;

                if (PlayerList.CurrentPlayer.FreePlays == 0) {
                    RoundRunner.ItemManager.ToggleFreePlay(false);
                }

                RoundRunner.ToggleUIButtons();
            } else if (Input.GetKeyUp(KeyCode.N)) {
                IsTimeForFreePlayDecision = false;
                RoundRunner.GotoNextPlayer();
                RoundRunner.SajakText.text = "It's your turn, " + PlayerList.CurrentPlayer.Name + ".";
            }
        } else if (IsTimeForMysteryDecision) {
            if (Input.GetKeyUp(KeyCode.Alpha2) || Input.GetKeyUp(KeyCode.Keypad2)) {
                IsTimeForMysteryDecision = false;
                StopCoroutine(MysteryWedgeCoroutine);
                StartCoroutine(mysteryWedgeLanded.TakeChance());
            } else if (Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.Keypad1)) {
                IsTimeForMysteryDecision = false;
                StopCoroutine(MysteryWedgeCoroutine);
                mysteryWedgeLanded.DontTakeChance();
            }
        }
    }

    private void ParseDebugSplits(string[] splits) {
        Splits = splits;

        if (splits == null || splits.Length == 0) {
            return;
        }

        for (int i = 0; i < splits.Length; i++) {
            Splits[i] = Splits[i].ToUpper();
        }

        Reveal();
        RevealSajak();
        BackgroundColor();
        XYSpeed();
        RotateSpeed();
        SpinTime();
        SpinAngleRange();
        StartCoroutine(SpinTo());
        CustomPuzzle();
        GiveMoney();
        Bankrupt();
        GotoNextPlayer();
        GotoPrevPlayer();
        SolveTime();
        SolveFor();
        Rounds();
        BPuzzleMax();
        RPuzzleMin();
        ResetHighScores();
        AddHighScore();
        StopSounds();
        UseWheel();
        ResetWheel();
    }

    private void BackgroundColor() {
        if (Splits[0].Equals("BACKGROUNDCOLOR") && Splits.Length == 4) {
            int[] asInt = new int[4];
            for (int i = 0; i < Splits.Length - 1; i++) {
                bool isParsed = int.TryParse(Splits[i + 1], out asInt[i]);
                if (!isParsed || asInt[i] < 0 || asInt[i] > 255) {
                    return;
                }
            }

            Color color = new Color32((byte)asInt[0], (byte)asInt[1], (byte)asInt[2], 255);
            Background.gameObject.GetComponent<Renderer>().material.SetColor("_Color", color);
        }
    }

    private void Reveal() {
        if (Splits[0].Equals("REVEAL")) {
            if (Splits.Length == 1) {
                RoundRunner.Reveal_Clicked();
            } else {
                List<char> letters = new List<char>();
                foreach (char c in Splits[1]) {
                    letters.Add(c);
                    RoundRunner.UsedLetterText[c - 65].color = Constants.USED_LETTER_DISABLED_COLOR;
                }

                StartCoroutine(RoundRunner.BoardFiller.RevealLetters(letters));
                foreach (char c in letters) {
                    RoundRunner.UsedLetters.Add(c);
                }
            }
        }
    }

    private void CustomPuzzle() {
        if (Splits[0].Equals("CUSTOMPUZZLE") && Splits.Length > 1) {
            string text = "";

            for (int i = 1; i < Splits.Length; i++) {
                if (i != Splits.Length - 1) {
                    text += Splits[i] + " ";
                } else {
                    text += Splits[i];
                }
            }

            CustomText = text;
            RoundRunner.RoundNumber--;
            RoundRunner.NewBoard(false);
        }
    }

    private void ResetWheel() {
        if (Splits[0].Equals("RESETWHEEL")) {
            GameObject WheelBaseObject = RoundRunner.WheelCanvas.transform.GetChild(0).gameObject;
            RoundRunner.ResetWheel(WheelBaseObject, true);
        }
    }

    private void SolveFor() {
        if (Splits[0].Equals("SOLVEFOR") && Splits.Length == 2) {
            Player p = PlayerList.Get(Splits[1]);
            if (p == null) {
                return;
            }

            PlayerList.GotoPlayer(Splits[1]);
            StartCoroutine(RoundRunner.SolvedCorrectly());
        }
    }

    private void RevealSajak() {
        if (Splits[0].Equals("REVEALSAJAK")) {
            RoundRunner.SajakText.text = "Puzzle Solution: " + RoundRunner.Puzzle.Text;
        }
    }

    private void BPuzzleMax() {
        if (Splits[0].Equals("BPUZZLEMAX") && Splits.Length == 2) {
            int parsed = 0;
            bool isParsed = int.TryParse(Splits[1], out parsed);

            if (isParsed) {
                PuzzleFactory.BONUS_ROUND_PUZZLE_SIZE_MAX = parsed;
            }
        }
    }

    private void RPuzzleMin() {
        if (Splits[0].Equals("RPUZZLEMIN") && Splits.Length == 2) {
            int parsed = 0;
            bool isParsed = int.TryParse(Splits[1], out parsed);

            if (isParsed) {
                PuzzleFactory.REGULAR_ROUND_PUZZLE_SIZE_MIN = parsed;
            }
        }
    }

    private void SpinTime() {
        if (Splits[0].Equals("SPINTIME") && Splits.Length == 2) {
            float parsed = 0;
            bool isParsed = float.TryParse(Splits[1], out parsed);

            if (isParsed) {
                SpinWheel.SpinTime = parsed;
            }
        }
    }

    private void UseWheel() {
        if (Splits[0].Equals("USEWHEEL") && Splits.Length == 2) {
            int parsed = 0;
            bool isParsed = int.TryParse(Splits[1], out parsed);

            if (isParsed) {
                RoundRunner.WheelCanvas = RoundRunner.WheelCanvases[parsed - 1];
                Debug.Log("Using Wheel #" + (parsed));
            }
        }
    }

    private void SpinAngleRange() {
        if (Splits[0].Equals("SPINANGLERANGE") && Splits.Length == 3) {
            int parsed = 0;
            bool isParsed = int.TryParse(Splits[1], out parsed);
            int parsed2 = 0;
            bool isParsed2 = int.TryParse(Splits[2], out parsed2);

            if (isParsed && isParsed2) {
                SpinWheel.minAngle = parsed;
                SpinWheel.maxAngle = parsed2;
            }
        }
    }

    private void XYSpeed() {
        if (Splits[0].Equals("XYSPEED")) {
            if (Splits.Length == 2) {
                float parsed = 0;
                bool isParsed = float.TryParse(Splits[1], out parsed);

                if (isParsed) {
                    Scroller scroller = Background.GetComponent<Scroller>();
                    scroller.xYSpeed = parsed;
                }
            }
        }
    }

    private void RotateSpeed() {
        if (Splits[0].Equals("ROTATESPEED")) {
            if (Splits.Length == 2) {
                float parsed = 0;
                bool isParsed = float.TryParse(Splits[1], out parsed);

                if (isParsed) {
                    Scroller scroller = Background.GetComponent<Scroller>();
                    scroller.rotateSpeed = parsed;
                }
            }
        }
    }

    private void SolveTime() {
        if (Splits[0].Equals("SOLVETIME") && Splits.Length == 2) {
            float parsed = 0;
            bool isParsed = float.TryParse(Splits[1], out parsed);

            if (isParsed) {
                CountdownTimer.timePerChar = parsed;
            }
        }
    }

    private void GiveMoney() {
        if (Splits[0].Equals("GIVE")) {
            if (Splits.Length >= 3) {
                Player p = PlayerList.Get(Splits[1]);
                if (p == null) {
                    return;
                }

                int parsed = 0;
                bool isParsed = int.TryParse(Splits[2], out parsed);

                if (isParsed) {
                    p.RoundWinnings += parsed;
                    RoundRunner.ToggleUIButtons();
                    RoundRunner.SajakText.text = parsed.ToString("C0") + " given to " + p.Name + ".";
                } else if (Splits.Length == 4 && Splits[2].Equals("FREEPLAY")) {
                    isParsed = int.TryParse(Splits[3], out parsed);
                    if (isParsed) {
                        p.FreePlays += parsed;
                        RoundRunner.SajakText.text = parsed + " Free Play given to " + p.Name + ".";
                    }
                }
            }
        }
    }

    private void Bankrupt() {
        if (Splits[0].Equals("BANKRUPT")) {
            if (Splits.Length == 2) {
                Player p = PlayerList.Get(Splits[1]);
                if (p == null) {
                    return;
                }

                RoundRunner.OnBankrupt(p);
            }
        }
    }

    private void GotoNextPlayer() {
        if (Splits[0].Equals("NEXTPLAYER")) {
            RoundRunner.GotoNextPlayer();
        }
    }

    private void GotoPrevPlayer() {
        if (Splits[0].Equals("PREVPLAYER")) {
            int movement = PlayerList.Players.Count - 1;
            for (int i = 0; i < movement; i++) {
                RoundRunner.GotoNextPlayer();
            }
        }
    }

    private void Rounds() {
        if (Splits[0].Equals("ROUNDS")) {
            int parsed = 0;
            bool isParsed = int.TryParse(Splits[1], out parsed);

            if (isParsed) {
                RoundRunner.MaxRounds = parsed;
            }
        }
    }

    private void ResetHighScores() {
        if (Splits[0].Equals("RESETHS")) {
            HighScore.Reset();
        }
    }

    private void AddHighScore() {
        if (Splits[0].Equals("ADDHS") && Splits.Length == 3) {
            int parsed = 0;
            bool isParsed = int.TryParse(Splits[2], out parsed);

            if (isParsed) {
                string str = "";
                for (int i = 0; i < Splits[1].Length; i++) {
                    if (i != 0) {
                        str += char.ToLower(Splits[1][i]);
                    } else {
                        str += Splits[1][i];
                    }
                }

                HighScore.UpdateHighScores(str, parsed);
            }
        }
    }

    private void StopSounds() {
        if (Splits[0].Equals("STOPSOUNDS") && Splits.Length == 1) {
            RoundRunner.SFXAudioTracks.Stop();
            RoundRunner.MusicAudioTracks.Stop();
        }
    }

    private IEnumerator SpinTo() {
        if (Splits[0].Equals("SPINTO")) {
            if (Splits.Length == 2) {
                string removedUnderscores = "";
                Splits[1] = Splits[1].ToUpper();
                foreach (char c in Splits[1]) {
                    if (c == '_') {
                        removedUnderscores += ' ';
                    } else {
                        removedUnderscores += c;
                    }
                }

                SpinWheel spinWheel = RoundRunner.WheelCanvas.transform.GetChild(0).GetComponent<SpinWheel>();

                spinWheel.debugWedgeText = removedUnderscores;
                yield return StartCoroutine(spinWheel.Spin(true));
            }
        }
    }

    private IEnumerator IfNoDebugField() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (isWheelActive) {
                SpinWheel spinWheel = RoundRunner.WheelCanvas.transform.GetChild(0).gameObject.GetComponent<SpinWheel>();

                if (!isMenuActive && !spinWheel.HasSpun) {
                    yield return StartCoroutine(spinWheel.Spin(false));
                }
            } else if (isBonusWheelActive) {
                if (!isMenuActive && !BonusWheelSpin.HasSpun) {
                    yield return StartCoroutine(BonusWheelSpin.Spin(false));
                }
            } else {
                Button b = RoundRunner.RegularRoundButtonsObject.transform.GetChild(0).GetComponent<Button>();
                if (b.interactable == true) {
                    RoundRunner.Spin_Clicked();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.F2)) {
            RoundRunner.Reveal_Clicked();
            yield return 0;
        }

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (isMenuActive) {
                MenuObject.SetActive(false);
                isMenuActive = false;
            } else {
                MenuObject.SetActive(true);
                isMenuActive = true;
            }

            yield return 0;
        }

        if (RoundRunner.IsTimeForLetter && !IsTimeForWildDecision) {
            for (char i = 'a'; i <= 'z'; i++) {
                string strChar = (i.ToString());
                if (Input.GetKeyDown(strChar)) {
                    yield return StartCoroutine(RoundRunner.LetterPressed(i));
                }
            }
        }
    }
}
