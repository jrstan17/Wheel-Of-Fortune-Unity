using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerList {
    public static List<Player> Players = new List<Player>();

    public static Player CurrentPlayer;
    private static int CurrentIndex = 0;

    public static Player Get(string name) {
        foreach (Player p in Players) {
            if (p.Name.Equals(name)) {
                return p;
            }
        }

        return null;
    }

    public static void GotoNextPlayer() {
        if (Players != null && Players.Count != 0) {
            if (CurrentPlayer != null) {
                if (CurrentIndex + 1 == Players.Count) {
                    CurrentIndex = 0;
                } else {
                    CurrentIndex++;
                }
            }

            CurrentPlayer = Players[CurrentIndex];
        }
    }
}
