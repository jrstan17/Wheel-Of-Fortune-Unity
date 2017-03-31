using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row {

    internal List<Trilon> Trilons;

    public Row(int trilonNumber) {
        Trilons = new List<Trilon>();

        for (int i = 0; i < trilonNumber; i++) {
            Trilons.Add(new Trilon(TrilonState.NotInUse));
        }
    }

    public int Size() {
        return Trilons.Count;
    }

    public bool IsGreaterThanLength(string str) {
        return (Trilons.Count > str.Length);
    }

    public bool CanFit(string answer) {
        return (answer.Length <= Trilons.Count);
    }

    public void Clear() {
        for (int i = 0; i < Trilons.Count; i++) {
            Trilons[i] = new Trilon(TrilonState.NotInUse);
        }
    }

    public string[] AddAnswer(string[] words) {
        int maxIndex = MaxIndexForRow(words);

        if (maxIndex != -1) {
            int trilon = FindStartingIndex(maxIndex, words);

            for (int i = 0; i <= maxIndex; i++) {
                for (int j = 0; j < words[i].Length; j++) {
                    Trilons[trilon].SetInUse(words[i][j]);
                    trilon++;
                }

                if (i != maxIndex) {
                    trilon++;
                }
            }
        }

        List<string> toReturn = new List<string>();
        for (int i = maxIndex + 1; i < words.Length; i++) {
            toReturn.Add(words[i]);
        }

        return toReturn.ToArray();
    }

    private int FindStartingIndex(int maxIndex, string[] words) {
        string answer = "";
        for (int i = 0; i <= maxIndex; i++) {
            answer += words[i];

            if (i != maxIndex) {
                answer += ' ';
            }
        }

        return (Size() - answer.Length) / 2;
    }

    private int MaxIndexForRow(string[] words) {
        int count = 0;

        for (int i = 0; i < words.Length; i++) {
            count += words[i].Length;

            if (count > Size()) {
                return i - 1;
            }

            if (i != words.Length - 1) {
                count++;
            }
        }

        return words.Length - 1;
    }

    private int CharCount(string[] strings) {
        int count = 0;

        foreach (string str in strings) {
            count += str.Length;
        }

        count += strings.Length - 1;

        return count;
    }

    public List<Trilon> GetTrilons() {
        List<Trilon> trilons = new List<Trilon>();
        foreach (Trilon t in Trilons) {
            trilons.Add(t);
        }

        return trilons;
    }

    public int Reveal(char letter) {
        int count = 0;

        foreach (Trilon t in Trilons) {
            if (t.Reveal(letter)) {
                count++;
            }
        }

        return count;
    }

    public void RevealAll() {
        foreach (Trilon t in Trilons) {
            t.Reveal();
        }
    }

    public override string ToString() {
        string toReturn = "";
        foreach (Trilon t in Trilons) {
            toReturn += t.ToString();
        }
        return toReturn;
    }
}
