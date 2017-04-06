using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    public string Name { get; set; }
    public int RoundWinnings { get; set; }
    public int TotalWinnings { get; set; }
    internal List<Prize> RoundPrizes { get; set; }
    internal List<Prize> TotalPrizes { get; set; }
    public int FreePlays { get; set; }
    public bool HasMillionWedge { get; set; }

    public Player(string name) {
        Name = name;
        RoundWinnings = 0;
        TotalWinnings = 0;
        RoundPrizes = new List<Prize>();
        TotalPrizes = new List<Prize>();
        HasMillionWedge = false;
        FreePlays = 0;
    }

    public void MovePrizesToBank() {
        foreach (Prize p in RoundPrizes) {
            TotalPrizes.Add(p.DeepCopy());
        }

        RoundPrizes.Clear();
    }

    public int CurrentRoundValue() {
        int value = RoundWinnings;

        foreach (Prize p in RoundPrizes) {
            value += p.Value;
        }

        return value;
    }

    public override bool Equals(object obj) {
        if (!(obj is Player)) {
            return false;
        }

        Player toCompare = (Player)obj;
        return (toCompare.Name.Equals(this.Name));
    }
}
