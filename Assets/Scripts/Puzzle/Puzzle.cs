﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle {

    internal string Text;
    internal string Category;
    internal DateTime Airdate;
    internal int Number;
    internal string TextLettersOnly;
    internal string Punctuation;

    public Puzzle(string rawLine, int number) {
        string[] splits = rawLine.Split('\t');

        Text = splits[0];

        foreach(char c in Text) {
            if (char.IsLetter(c)) {
                TextLettersOnly += c;
            } else {
                Punctuation += c;
            }
        }

        Category = splits[1];
        Airdate = DateTime.Parse(splits[2]);
        Number = number;
    }

    public bool HasNonLetters() {
        foreach(char c in Text) {
            if (!char.IsLetter(c) && c != ' ') {
                return true;
            }
        }

        return false;
    }
}
