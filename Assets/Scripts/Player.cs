using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    public string Name { get; set; }
    public int RoundWinnings { get; set; }
    public int TotalWinnings { get; set; }
    internal Prize RoundPrize { get; set; }
    internal List<Prize> TotalPrizes { get; set; }
    public int FreePlays { get; set; }
    public bool HasMillionWedge { get; set; }

    public Player(string name) {
        Name = name;
        RoundWinnings = 0;
        TotalWinnings = 0;
        TotalPrizes = new List<Prize>();
        HasMillionWedge = false;
        FreePlays = 0;
    }

    public void MovePrizeToBank() {
        TotalPrizes.Add(RoundPrize.DeepCopy());
        RoundPrize = null;
    }

    public bool HasPrize() {
        return (RoundPrize != null);
    }

    public int CurrentRoundValue() {
        return RoundWinnings + RoundPrize.Value;
    }

    public override bool Equals(object obj) {
        if (!(obj is Player)) {
            return false;
        }

        Player toCompare = (Player)obj;

        return (toCompare.Name.ToUpper().Equals(this.Name.ToUpper()));
    }

    public override int GetHashCode() {
        return base.GetHashCode();
    }
}
