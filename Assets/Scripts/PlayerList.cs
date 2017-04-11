using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerList {
    public static List<Player> Players = new List<Player>();

    public static Player CurrentPlayer;
    private static int CurrentIndex = 0;

    public static Player Get(string name) {
        foreach (Player p in Players) {
            if (p.Name.ToUpper().Equals(name.ToUpper())) {
                return p;
            }
        }

        return null;
    }

    public static void RandomizePlayers() {
        List<Player> temp = new List<Player>();

        while (Players.Count != 0) {
            int i = Random.Range(0, Players.Count);
            temp.Add(Players[i]);
            Players.RemoveAt(i);
        }

        Players = temp;
    }

    public static void TransferRoundToTotalForCurrentPlayer() {
        CurrentPlayer.TotalWinnings += CurrentPlayer.RoundWinnings;
        CurrentPlayer.RoundWinnings = 0;
    }

    public static string NextPlayersName() {
        int i = CurrentIndex;

        if (i + 1 == Players.Count) {
            i = 0;
        } else {
            i++;
        }

        return Players[i].Name;
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
