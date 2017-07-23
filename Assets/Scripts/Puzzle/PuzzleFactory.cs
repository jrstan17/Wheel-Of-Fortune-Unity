using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PuzzleFactory {

    public static int REGULAR_ROUND_PUZZLE_SIZE_MIN = 16;
    public static int BONUS_ROUND_PUZZLE_SIZE_MAX = 16;
	public static float BONUS_MAX_RSTLNE_RATIO = 0.3f;
    const int TIMEOUT = 1000000;

    List<Puzzle> Puzzles;
    int previousPuzzleIndex = -1;

    public PuzzleFactory(TextAsset textAsset) {
        InitPuzzleList(textAsset);
    }

    public Puzzle NewPuzzle(RoundType roundType) {
        int randomIndex = -1;
        randomIndex = Random.Range(0, Puzzles.Count);

        int i = 1;
        while (previousPuzzleIndex == randomIndex || !IsRandomRoundPuzzleValid(randomIndex, roundType) || i > TIMEOUT) {
            randomIndex = Random.Range(0, Puzzles.Count);
            i++;
        }

        if (i > TIMEOUT) {
            Debug.LogError("Puzzle Generation Timeout!");
            return null;
        }

        previousPuzzleIndex = randomIndex;

        return Puzzles[randomIndex];
    }

    public Puzzle NewPuzzle(string text) {
        text = text.ToUpper();
        return new Puzzle(text + "\tCustom Puzzle\t10/16/2014", 1);
    }

    public bool IsRandomRoundPuzzleValid(int randomIndex, RoundType type) {
        string answer = Puzzles[randomIndex].Text;
		return IsRandomRoundPuzzleValid(answer, type);
    }

	public bool IsRandomRoundPuzzleValid(string answer, RoundType type){
		int letters = 0;
		foreach (char c in answer) {
			if (char.IsLetter(c)) {
				letters++;
			}
		}

		if (type == RoundType.Regular) {
			if (letters >= REGULAR_ROUND_PUZZLE_SIZE_MIN) {
				return true;
			}
		} else if (type == RoundType.Bonus) {
			if (letters <= BONUS_ROUND_PUZZLE_SIZE_MAX && IsValidLetterRatio(answer, Utilities.RSTLNE, BONUS_MAX_RSTLNE_RATIO)) {
				return true;
			}
		}

		return false;
	}

    internal bool IsValidLetterRatio(string answer, List<char> lettersToCheck, float maxRatioAllowed) {
        List<char> answerLetters = new List<char>();
        foreach(char c in answer) {
            if (char.IsLetter(c)) {
                answerLetters.Add(c);
            }
        }

        int sum = 0;
        int total = 0;
        foreach(char c in answerLetters) {
            if (lettersToCheck.Contains(c)) {
                sum++;
            }

            total++;
        }

        float ratio = (float) sum / total;
        Debug.Log("RSTLNE: " + sum + " / Letters: " + total + " / Ratio: " + ratio.ToString("P"));

		return (ratio <= maxRatioAllowed && ratio != 0);
    }

    internal Puzzle NewPuzzle(int puzzleIndex) {
        previousPuzzleIndex = puzzleIndex;
        return Puzzles[puzzleIndex];
    }

    private void InitPuzzleList(TextAsset textAsset) {
        if (Puzzles == null) {
            int count = 1;
            Puzzles = new List<Puzzle>();
            string text = textAsset.text;
            string[] textSplits = textAsset.text.Split('\n');

            foreach(string str in textSplits) {
                Puzzles.Add(new Puzzle(str, count));
                count++;
            }
        }
    }
}
