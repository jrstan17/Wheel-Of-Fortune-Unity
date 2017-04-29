using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardFiller : MonoBehaviour {

    public List<Row> Rows;
    public List<Trilon> Trilons;
    public AudioTracks AudioTracks;
    internal RoundRunner RoundRunner;
    internal Color32 ScreenColor;

    internal static int LettersRevealed = 0;

    Data data;

    private IEnumerator coroutine;

    void Start() {
        RoundRunner = GameObject.FindGameObjectWithTag("RoundRunner").GetComponent<RoundRunner>();
    }

    public bool InitBoard() {
        GameObject dataHolder = GameObject.FindGameObjectWithTag("DataHolder");
        data = dataHolder.GetComponent<Data>();

        ClearBoard();
        bool fillRowSuccess = FillRows();

        if (!fillRowSuccess) {
            Debug.Log("Fill Board Failure!");
            Rows = new List<Row>();
            ClearBoard();
            return false;
        }

        Trilons = new List<Trilon>();
        foreach (Row r in Rows) {
            foreach (Trilon t in r.Trilons) {
                Trilons.Add(t);
            }
        }

        FillBoard();
        return true;
    }

    public bool FillRows() {
        Rows = new List<Row>();
        Rows.Add(new Row(12));
        Rows.Add(new Row(14));
        Rows.Add(new Row(14));
        Rows.Add(new Row(12));

        string[] splits = RoundRunner.Puzzle.Text.Split(' ');
        string[] remainders = null;

        remainders = Rows[1].AddAnswer(splits);

        if (remainders.Length != 0) {
            remainders = Rows[2].AddAnswer(remainders);
        }

        if (remainders.Length != 0) {
            if (Random.Range(0, 2) == 0) {
                this.Clear();
                remainders = null;

                remainders = Rows[1].AddAnswer(splits);
                remainders = Rows[2].AddAnswer(remainders);
                remainders = Rows[3].AddAnswer(remainders);

                if (remainders.Length != 0) {
                    this.Clear();
                    remainders = null;

                    remainders = Rows[0].AddAnswer(splits);
                    remainders = Rows[1].AddAnswer(remainders);
                    remainders = Rows[2].AddAnswer(remainders);
                    remainders = Rows[3].AddAnswer(remainders);
                }
            } else {
                this.Clear();
                remainders = null;

                remainders = Rows[0].AddAnswer(splits);
                remainders = Rows[1].AddAnswer(remainders);
                remainders = Rows[2].AddAnswer(remainders);


                if (remainders.Length != 0) {
                    this.Clear();
                    remainders = null;

                    remainders = Rows[0].AddAnswer(splits);
                    remainders = Rows[1].AddAnswer(remainders);
                    remainders = Rows[2].AddAnswer(remainders);
                    remainders = Rows[3].AddAnswer(remainders);
                }
            }
        }

        if (remainders.Length != 0) {
            return false;
        } else {
            return true;
        }
    }

    public void RefreshBoardColor() {
        if (Trilons != null && data != null) {
            for (int i = 0; i < Trilons.Count; i++) {
                data.Letters[i].color = ScreenColor;
                data.Screens[i].color = ScreenColor;
            }
        }
    }

    public void FillBoard() {
        List<int> InUseIndexes = new List<int>();
        for (int i = 0; i < Trilons.Count; i++) {
            string letter = Trilons[i].Letter.ToString();
            data.Letters[i].text = letter;
            data.Letters[i].color = ScreenColor;

            if (Trilons[i].State != TrilonState.NotInUse) {
                InUseIndexes.Add(i);
            }
        }

        coroutine = RevealWhiteScreens(0.025f, InUseIndexes);
        StartCoroutine(coroutine);
    }

    public bool PuzzleContainsOnly(LetterType type) {
        if (Trilons != null) {
            foreach (Trilon t in Trilons) {
                if (type == LetterType.Vowel) {
                    if (t.State == TrilonState.Unrevealed && !Utilities.IsVowel(t.Letter)) {
                        return false;
                    }
                } else if (type == LetterType.Consonant) {
                    if (t.State == TrilonState.Unrevealed && Utilities.IsVowel(t.Letter)) {
                        return false;
                    }
                } else if (type == LetterType.Both) {
                    if (t.State == TrilonState.Unrevealed) {
                        return false;
                    }
                }
            }
        } else {
            return false;
        }

        return true;
    }

    public bool IsPuzzleFullyRevealed() {
        if (Trilons != null) {
            foreach (Trilon t in Trilons) {
                if (t.State == TrilonState.Unrevealed) {
                    return false;
                }
            }
        } else {
            return false;
        }

        return true;
    }

    private IEnumerator RevealWhiteScreens(float time, List<int> InUseIndexes) {
        foreach (int i in InUseIndexes) {
            data.Letters[i].color = Color.white;
            data.Screens[i].color = Color.white;
            yield return new WaitForSeconds(time);
        }

        yield return new WaitForSeconds(2f);

        if (RoundRunner.Puzzle.HasNonLetters()) {
            float start = Time.time;
            RoundRunner.SajakText.text = "But first, let's reveal some punctuation.";
            yield return StartCoroutine(RevealLetters(Utilities.NonLetters));

            if (Time.time < start + 4) {
                yield return new WaitForSeconds(start + 4 - Time.time);
            }
        }

        if (!RoundRunner.IsBonusRound) {
            RoundRunner.SajakText.text = "Start us off with a spin, " + PlayerList.CurrentPlayer.Name + ".";
            RoundRunner.ToggleUIButtons();
        }
    }

    public IEnumerator RevealRSTLNE() {
        yield return StartCoroutine(RevealLetters(Utilities.RSTLNE));
    }

    public void ClearBoard() {
        for (int i = 0; i < data.Screens.Count; i++) {
            data.Screens[i].color = ScreenColor;
            data.Letters[i].color = ScreenColor;
        }
    }

    public IEnumerator RevealBoard() {
        List<int> LetterIndexes = new List<int>();
        for (int i = 0; i < Trilons.Count; i++) {
            if (Trilons[i].State == TrilonState.Unrevealed) {
                LetterIndexes.Add(i);
            }
        }

        coroutine = RevealingTimer(0.025f, LetterIndexes);
        yield return StartCoroutine(coroutine);
    }

    public IEnumerator RevealingTimer(float time, List<int> indexes) {
        foreach (int i in indexes) {
            Trilons[i].Reveal();
            data.Letters[i].color = Color.black;
            data.Screens[i].color = Color.white;
            yield return new WaitForSeconds(time);
        }
    }

    public IEnumerator RevealLetters(List<char> letters) {
        for (int i = 0; i < letters.Count; i++) {
            letters[i] = char.ToUpper(letters[i]);
        }

        foreach (char letter in letters) {
            if (char.IsLetter(letter)) {
                RoundRunner.UsedLetterText[letter - 65].color = Constants.USED_LETTER_DISABLED_COLOR;
            }
        }

        List<int> LetterIndexes = new List<int>();
        for (int i = 0; i < letters.Count; i++) {
            for (int j = 0; j < Trilons.Count; j++) {
                if (data.Letters[j].text.Equals(letters[i].ToString())) {
                    LetterIndexes.Add(j);
                }
            }
        }

        if (LetterIndexes.Count == 0 && !RoundRunner.IsBonusRound) {
            AudioTracks.Play("buzzer");
            yield return 0;
        }

        for (int i = 0; i < letters.Count; i++) {
            foreach (Trilon t in Trilons) {
                if (t.Letter == letters[i]) {
                    t.Reveal(letters[i]);
                }
            }
        }

        LettersRevealed = LetterIndexes.Count;

        yield return StartCoroutine(WaitForLetter(1f, letters, LetterIndexes));
    }

    private IEnumerator WaitForLetter(float waitTime, List<char> letters, List<int> Indexes) {
        foreach (int i in Indexes) {
            AudioTracks.Play("ding");

            data.Letters[i].color = Color.blue;
            data.Screens[i].color = Color.blue;

            yield return new WaitForSeconds(1f);

            data.Letters[i].color = Color.black;
            data.Screens[i].color = Color.white;

            yield return new WaitForSeconds(waitTime);
        }

        if (!PuzzleContainsOnly(LetterType.Both)) {
            if (PuzzleContainsOnly(LetterType.Vowel) && !RoundRunner.NotifiedOfRemainingLetters && !RoundRunner.IsBonusRound) {
                AudioTracks.Play("remains");
                RoundRunner.SajakText.text = "Only vowels remain!";
                RoundRunner.NotifiedOfRemainingLetters = true;
                RoundRunner.ToggleUIButtons();

                foreach (char c in Utilities.Consonants) {
                    if (!RoundRunner.UsedLetters.Contains(c)) {
                        RoundRunner.UsedLetters.Add(c);
                    }
                    RoundRunner.UsedLetterText[c - 65].color = Constants.USED_LETTER_DISABLED_COLOR;
                }
            } else if (PuzzleContainsOnly(LetterType.Consonant) && !RoundRunner.NotifiedOfRemainingLetters && !RoundRunner.IsBonusRound) {
                AudioTracks.Play("remains");
                RoundRunner.SajakText.text = "Only consonants remain!";
                RoundRunner.NotifiedOfRemainingLetters = true;
                RoundRunner.ToggleUIButtons();

                foreach (char c in Utilities.Vowels) {
                    if (!RoundRunner.UsedLetters.Contains(c)) {
                        RoundRunner.UsedLetters.Add(c);
                    }
                    RoundRunner.UsedLetterText[c - 65].color = Constants.USED_LETTER_DISABLED_COLOR;
                }
            }
        }

        yield return 0;
    }

    public void Clear() {
        foreach (Row r in Rows) {
            r.Clear();
        }
    }
}
