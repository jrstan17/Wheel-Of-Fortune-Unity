using Assets.Scripts.Enums;
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
    internal RoundRunner RoundRunner;
    internal Color32 ScreenColor;

    internal static int LettersRevealed = 0;
	internal static bool ExpressRemainSoundAlreadySounded = false;

    private IEnumerator coroutine;

    void Start() {
        RoundRunner = gameObject.GetComponent<RoundRunner>();
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

                        if (type == LetterType.Neither) {
                            return false;
                        }
                    } else if (!Utilities.IsVowel(t.Letter) && !hasConsonants) {
                        hasConsonants = true;

                        if (type == LetterType.Neither) {
                            return false;
                        }
                    }
                }

                if (hasConsonants && hasVowels) {
                    break;
                }
            }

            return ((type == LetterType.Vowel && hasVowels && !hasConsonants) ||
                (type == LetterType.Consonant && !hasVowels && hasConsonants) ||
                (type == LetterType.Both && hasVowels && hasConsonants) ||
                (type == LetterType.Neither && !hasVowels && !hasConsonants));
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
            RoundRunner.SajakText.text = GetSajakForPunctuation();
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
                RoundRunner.UsedLetterText[letter - 65].color = Constants.USED_LETTER_DISABLED_COLOR;
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

        if (revealData.Count == 0 && !RoundRunner.IsBonusRound) {
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

		RoundRunner.ToggleUIButtonsParsing("all", false);
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

        if (!PuzzleContainsOnly(LetterType.Both) && !RoundRunner.KeyPress.expressWedgeLanded.IsExpressRunning) {
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

		if (RoundRunner.KeyPress.expressWedgeLanded.IsExpressRunning && PuzzleContainsOnly(LetterType.Vowel) || PuzzleContainsOnly(LetterType.Consonant)) {

			if (!ExpressRemainSoundAlreadySounded) {
				AudioTracks.Play("remains");
				ExpressRemainSoundAlreadySounded = true;
			}
			if (PuzzleContainsOnly(LetterType.Consonant)) {
				RoundRunner.ToggleUIButtonsParsing("buy", false);
			}
		}

        yield return 0;
    }

    public void Clear() {
        foreach (Row r in Rows) {
            r.Clear();
        }
    }

    private string GetSajakForPunctuation() {
        PunctuationLibrary library = new PunctuationLibrary();
        library.Add(PunctuationType.Ampersand);
        library.Add(PunctuationType.Apostrophe);
        library.Add(PunctuationType.Colon);
        library.Add(PunctuationType.Comma);
        library.Add(PunctuationType.ExclamationPoint);
        library.Add(PunctuationType.Hyphen);
        library.Add(PunctuationType.Period);
        library.Add(PunctuationType.QuestionMark);

        foreach (char c in RoundRunner.Puzzle.Punctuation) {
            switch (c) {
                case '&':
                    library.Increase(PunctuationType.Ampersand);
                    break;
                case ',':
                    library.Increase(PunctuationType.Comma);
                    break;
                case ':':
                    library.Increase(PunctuationType.Colon);
                    break;
                case '-':
                    library.Increase(PunctuationType.Hyphen);
                    break;
                case '?':
                    library.Increase(PunctuationType.QuestionMark);
                    break;
                case '!':
                    library.Increase(PunctuationType.ExclamationPoint);
                    break;
                case '\'':
                    library.Increase(PunctuationType.Apostrophe);
                    break;
                case '.':
                    library.Increase(PunctuationType.Period);
                    break;
                default:
                    break;
            }
        }

        string toReturn = "But first, let's reveal ";

        if (library.GetTypesOfMoreThanQuantity(0).Count > 1) {
            toReturn += "some punctuation.";
            return toReturn;
        }

        List<PunctuationType> types = library.GetTypesOfMoreThanQuantity(0);

        string typeStr = "";

        if (types[0] == PunctuationType.Ampersand) {
            typeStr = "ampersand";
        } else if (types[0] == PunctuationType.Apostrophe) {
            typeStr = "apostrophe";
        } else if (types[0] == PunctuationType.Colon) {
            typeStr = "colon";
        } else if (types[0] == PunctuationType.Comma) {
            typeStr = "comma";
        } else if (types[0] == PunctuationType.ExclamationPoint) {
            typeStr = "exclamation point";
        } else if (types[0] == PunctuationType.Hyphen) {
            typeStr = "hyphen";
        } else if (types[0] == PunctuationType.Period) {
            typeStr = "period";
        } else if (types[0] == PunctuationType.QuestionMark) {
            typeStr = "question mark";
        }

        types = library.GetTypesOfMoreThanQuantity(2);
        if (types.Count != 0) {
            typeStr += "s";
            toReturn += "some " + typeStr;
        }

        types = library.GetTypesOfQuantity(2);
        if (types.Count != 0) {
            typeStr += "s";
            toReturn += "a couple " + typeStr;
        }

        types = library.GetTypesOfQuantity(1);
        if (types.Count != 0) {
            if (types[0] == PunctuationType.Ampersand) {
                toReturn += "a lovely " + typeStr;
            } else if (Utilities.IsVowel(typeStr[0])) {
                toReturn += "an " + typeStr;
            } else {
                toReturn += "a " + typeStr;
            }
        }

        toReturn += ".";
        return toReturn;
    }

    private class PunctuationLibrary {
        private List<PuncQty> puncQty = new List<PuncQty>();

        public void Add(PunctuationType type) {
            puncQty.Add(new PuncQty() { Type = type, Quantity = 0 });
        }

        public void Increase(PunctuationType type) {
            foreach (PuncQty pq in puncQty) {
                if (pq.Type == type) {
                    pq.Quantity++;
                }
            }
        }

        public List<PunctuationType> GetTypesOfQuantity(int quantity) {
            List<PunctuationType> types = new List<PunctuationType>();

            foreach (PuncQty pq in puncQty) {
                if (pq.Quantity == quantity) {
                    types.Add(pq.Type);
                }
            }

            return types;
        }

        public List<PunctuationType> GetTypesOfMoreThanQuantity(int quantity) {
            List<PunctuationType> types = new List<PunctuationType>();

            foreach (PuncQty pq in puncQty) {
                if (pq.Quantity > quantity) {
                    types.Add(pq.Type);
                }
            }

            return types;
        }

        public int Quantity(PunctuationType type) {
            foreach (PuncQty pq in puncQty) {
                if (pq.Type == type) {
                    return pq.Quantity;
                }
            }

            return -1;
        }

        private class PuncQty {
            public PunctuationType Type { get; set; }
            public int Quantity { get; set; }
        }
    }

    private class RevealData {
        public int Index { get; set; }
        public Sprite LetterSprite { get; set; }
    }
}