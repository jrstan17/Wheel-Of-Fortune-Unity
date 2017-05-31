using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardFiller : MonoBehaviour {

    public List<SpriteRenderer> Screens;
    public List<SpriteRenderer> Letters;
    LetterGetter LetterGetter;

    public List<Row> Rows;
    public List<Trilon> Trilons;
    public AudioTracks AudioTracks;
    internal RoundRunner RR;
    internal Color32 ScreenColor;

    internal static int LettersRevealed = 0;

    private IEnumerator coroutine;

    void Start() {
        RR = gameObject.GetComponent<RoundRunner>();
        LetterGetter = GetComponent<LetterGetter>();
    }

    public bool InitBoard() {
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
        if (Trilons != null) {
            for (int i = 0; i < Trilons.Count; i++) {
                Letters[i].color = ScreenColor;
                Screens[i].color = ScreenColor;
            }
        }
    }

    public void FillBoard() {
        List<int> InUseIndexes = new List<int>();
        for (int i = 0; i < Trilons.Count; i++) {
            Letters[i].sprite = LetterGetter.GetSprite(Trilons[i].Letter);
            Letters[i].color = ScreenColor;

            if (Trilons[i].State != TrilonState.NotInUse) {
                InUseIndexes.Add(i);
            }
        }

        coroutine = RevealWhiteScreens(0.025f, InUseIndexes);
        StartCoroutine(coroutine);
    }

    public bool PuzzleContainsOnly(LetterType type) {
        bool hasConsonants = false;
        bool hasVowels = false;

        if (Trilons != null) {
            foreach (Trilon t in Trilons) {
                if (t.State == TrilonState.Unrevealed) {
                    if (Utilities.IsVowel(t.Letter) && !hasVowels) {
                        hasVowels = true;
                    } else if (!Utilities.IsVowel(t.Letter) && !hasConsonants) {
                        hasConsonants = true;
                    }
                }

                if (hasConsonants && hasVowels) {
                    break;
                }
            }

            return ((type == LetterType.Vowel && hasVowels && !hasConsonants) ||
                (type == LetterType.Consonant && !hasVowels && hasConsonants) ||
                (type == LetterType.Both && hasVowels && hasConsonants));
        }

        return false;
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
            Letters[i].color = Color.white;
            Screens[i].color = Color.white;
            yield return new WaitForSeconds(time);
        }

        yield return new WaitForSeconds(2f);

        if (RoundRunner.Puzzle.HasNonLetters()) {
            float start = Time.time;
            RR.SajakText.text = "But first, let's reveal some punctuation.";
            yield return StartCoroutine(RevealLetters(Utilities.NonLetters));

            if (Time.time < start + 4) {
                yield return new WaitForSeconds(start + 4 - Time.time);
            }
        }

        if (!RR.IsBonusRound) {
            RR.SajakText.text = "Start us off with a spin, " + PlayerList.CurrentPlayer.Name + ".";
            RR.ToggleUIButtons();
        }
    }

    public IEnumerator RevealRSTLNE() {
        yield return StartCoroutine(RevealLetters(Utilities.RSTLNE));
    }

    public void ClearBoard() {
        for (int i = 0; i < Screens.Count; i++) {
            Screens[i].color = ScreenColor;
            Letters[i].color = ScreenColor;
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
            Letters[i].color = Color.black;
            Screens[i].color = Color.white;
            yield return new WaitForSeconds(time);
        }
    }

    public IEnumerator RevealLetters(List<char> letters) {
        for (int i = 0; i < letters.Count; i++) {
            letters[i] = char.ToUpper(letters[i]);
        }

        foreach (char letter in letters) {
            if (char.IsLetter(letter)) {
                RR.UsedLetterText[letter - 65].color = Constants.USED_LETTER_DISABLED_COLOR;
            }
        }


        List<RevealData> revealData = new List<RevealData>();
        for (int i = 0; i < letters.Count; i++) {
            for (int j = 0; j < Trilons.Count; j++) {
                if (Trilons[j].Letter == letters[i]) {
                    revealData.Add(new RevealData() { Index = j, LetterSprite = LetterGetter.GetSprite(letters[i]) });
                }
            }
        }

        if (revealData.Count == 0 && !RR.IsBonusRound) {
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

        LettersRevealed = revealData.Count;

        yield return StartCoroutine(WaitForLetter(1f, letters, revealData));
    }

    private IEnumerator WaitForLetter(float waitTime, List<char> letters, List<RevealData> revealData) {
        foreach (RevealData rd in revealData) {
            int i = rd.Index;
            Sprite sprite = rd.LetterSprite;

            AudioTracks.Play("ding");

            Screens[i].color = Color.blue;
            Letters[i].color = Color.blue;

            yield return new WaitForSeconds(1f);

            Letters[i].sprite = sprite;
            Letters[i].color = Color.black;
            Screens[i].color = Color.white;

            yield return new WaitForSeconds(waitTime);
        }

        if (!PuzzleContainsOnly(LetterType.Both)) {
            if (PuzzleContainsOnly(LetterType.Vowel) && !RR.NotifiedOfRemainingLetters && !RR.IsBonusRound) {
                AudioTracks.Play("remains");
                RR.SajakText.text = "Only vowels remain!";
                RR.NotifiedOfRemainingLetters = true;
                RR.ToggleUIButtons();

                foreach (char c in Utilities.Consonants) {
                    if (!RR.UsedLetters.Contains(c)) {
                        RR.UsedLetters.Add(c);
                    }
                    RR.UsedLetterText[c - 65].color = Constants.USED_LETTER_DISABLED_COLOR;
                }
            } else if (PuzzleContainsOnly(LetterType.Consonant) && !RR.NotifiedOfRemainingLetters && !RR.IsBonusRound) {
                AudioTracks.Play("remains");
                RR.SajakText.text = "Only consonants remain!";
                RR.NotifiedOfRemainingLetters = true;
                RR.ToggleUIButtons();

                foreach (char c in Utilities.Vowels) {
                    if (!RR.UsedLetters.Contains(c)) {
                        RR.UsedLetters.Add(c);
                    }
                    RR.UsedLetterText[c - 65].color = Constants.USED_LETTER_DISABLED_COLOR;
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

    private class RevealData {
        public int Index { get; set; }
        public Sprite LetterSprite { get; set; }
    }
}