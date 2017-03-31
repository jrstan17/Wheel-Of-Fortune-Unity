using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PuzzleFactory {

    const int REGULAR_ROUND_PUZZLE_SIZE_MIN = 16;
    const int BONUS_ROUND_PUZZLE_SIZE_MAX = 16;

    List<Puzzle> Puzzles;
    int previousPuzzleIndex = -1;

    public PuzzleFactory(TextAsset textAsset) {
        InitPuzzleList(textAsset);
    }

    public Puzzle NewPuzzle(RoundType roundType) {
        int randomIndex = -1;
        randomIndex = Random.Range(0, Puzzles.Count);
        while (previousPuzzleIndex == randomIndex || !IsRandomRoundPuzzleValid(randomIndex, roundType)) {
            randomIndex = Random.Range(0, Puzzles.Count);
        }
        previousPuzzleIndex = randomIndex;

        return Puzzles[randomIndex];
        //return new Puzzle("IF YOU DON'T LOVE ME I'LL KICK YOUR BUTT\tCategory\t10/16/2014", 1);
    }

    private bool IsRandomRoundPuzzleValid(int randomIndex, RoundType type) {
        string answer = Puzzles[randomIndex].Text;
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
            if (letters <= BONUS_ROUND_PUZZLE_SIZE_MAX) {
                return true;
            }
        }

        return false;
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
