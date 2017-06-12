using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterGetter : MonoBehaviour {

    public Sprite[] LetterSprites;

    static List<Frequency> frequencies;

    public Sprite GetSprite(char c) {
        if (char.IsLetter(c)) {
            return LetterSprites[c - 65];
        }

        switch (c) {
            case '&':
                return LetterSprites[26];
            case '\'':
                return LetterSprites[27];
            case ':':
                return LetterSprites[28];
            case '.':
                return LetterSprites[29];
            case ',':
                return LetterSprites[30];
            case '-':
                return LetterSprites[31];
            case '!':
                return LetterSprites[32];
            case '?':
                return LetterSprites[33];
            default:
                return null;
        }
    }

    public Sprite GetRandomLetter() {
        if (frequencies == null) {
            frequencies = new List<Frequency>();
            frequencies.Add(new Frequency() { Letter = 'A', FrequencySum = 0.08167f });
            frequencies.Add(new Frequency() { Letter = 'B', FrequencySum = 0.09659f });
            frequencies.Add(new Frequency() { Letter = 'C', FrequencySum = 0.12441f });
            frequencies.Add(new Frequency() { Letter = 'D', FrequencySum = 0.16694f });
            frequencies.Add(new Frequency() { Letter = 'E', FrequencySum = 0.28766f });
            frequencies.Add(new Frequency() { Letter = 'F', FrequencySum = 0.30994f });
            frequencies.Add(new Frequency() { Letter = 'G', FrequencySum = 0.33009f });
            frequencies.Add(new Frequency() { Letter = 'H', FrequencySum = 0.39103f });
            frequencies.Add(new Frequency() { Letter = 'I', FrequencySum = 0.46069f });
            frequencies.Add(new Frequency() { Letter = 'J', FrequencySum = 0.46222f });
            frequencies.Add(new Frequency() { Letter = 'K', FrequencySum = 0.46994f });
            frequencies.Add(new Frequency() { Letter = 'L', FrequencySum = 0.51019f });
            frequencies.Add(new Frequency() { Letter = 'M', FrequencySum = 0.53425f });
            frequencies.Add(new Frequency() { Letter = 'N', FrequencySum = 0.60174f });
            frequencies.Add(new Frequency() { Letter = 'O', FrequencySum = 0.67681f });
            frequencies.Add(new Frequency() { Letter = 'P', FrequencySum = 0.69610f });
            frequencies.Add(new Frequency() { Letter = 'Q', FrequencySum = 0.69705f });
            frequencies.Add(new Frequency() { Letter = 'R', FrequencySum = 0.75692f });
            frequencies.Add(new Frequency() { Letter = 'S', FrequencySum = 0.82019f });
            frequencies.Add(new Frequency() { Letter = 'T', FrequencySum = 0.91075f });
            frequencies.Add(new Frequency() { Letter = 'U', FrequencySum = 0.93833f });
            frequencies.Add(new Frequency() { Letter = 'V', FrequencySum = 0.94811f });
            frequencies.Add(new Frequency() { Letter = 'W', FrequencySum = 0.97171f });
            frequencies.Add(new Frequency() { Letter = 'X', FrequencySum = 0.97321f });
            frequencies.Add(new Frequency() { Letter = 'Y', FrequencySum = 0.99295f });
            frequencies.Add(new Frequency() { Letter = 'Z', FrequencySum = 1f });
        }

        float rndFloat = Random.value;

        for (int i = 0; i < frequencies.Count; i++) {
            if (frequencies[i].FrequencySum >= rndFloat) {
                return LetterSprites[i];
            }
        }

        return null;
    }

    private class Frequency {
        public char Letter { get; set; }
        public float FrequencySum { get; set; }
    }
}
