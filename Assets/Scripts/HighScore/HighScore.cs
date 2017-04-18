using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class HighScore {

    public static List<Entry> Entries;

    public HighScore() {
        string raw = PlayerPrefs.GetString("highscores");
        if (raw.Length == 0) {
            Reset();
        } else {
            FillEntries(raw);
        }
    }

    public static void Reset() {
        string raw = "Jon 100000\tAshley 90000\tJonah 80000\tSarah 70000\tAdric 60000\tDonna 50000\tSteven 40000\tZoe 30000\tTurlough 20000\tSusan 10000";
        PlayerPrefs.SetString("highscores", raw);
        PlayerPrefs.Save();
        FillEntries(raw);
    }

    private static void FillEntries(string raw) {
        Entries = new List<Entry>();
        string[] tabSplits = raw.Split('\t');
        foreach (string str in tabSplits) {
            Entries.Add(new Entry(str));
        }
    }

    internal static int UpdateHighScores(string name, int moneyWon) {
        bool madeHighScore = false;
        int place = 0;

        for (int i = 0; i < Entries.Count; i++) {
            if (Entries[i].Value < moneyWon) {
                Entry newScore = new Entry(name, moneyWon);
                Entries.Insert(i, newScore);
                place = i + 1;
                madeHighScore = true;
                break;
            }
        }

        if (madeHighScore) {
            Entries.RemoveAt(Entries.Count - 1);
            string newHighScores = MakeIntoString();
            PlayerPrefs.SetString("highscores", newHighScores);
            PlayerPrefs.Save();
        }

        return place;
    }

    internal static string MakeIntoString() {
        StringBuilder toReturn = new StringBuilder();

        for (int i = 0; i < Entries.Count; i++) {
            toReturn.Append(Entries[i].Name);
            toReturn.Append(" ");
            toReturn.Append(Entries[i].Value);

            if (i != Entries.Count - 1) {
                toReturn.Append("\t");
            }
        }

        return toReturn.ToString();
    }

    public static string GetOrdinalSuffix(int n) {
        if (n == 1) {
            return "st";
        } else if (n == 2) {
            return "nd";
        } else if (n == 3) {
            return "rd";
        } else {
            return "th";
        }
    }
}
