using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LetterGetter : MonoBehaviour {

    public Sprite[] LetterSprites;

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
        int index = Random.Range(0, 26);
        return LetterSprites[index];
    }
}
