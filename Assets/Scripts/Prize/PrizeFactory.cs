using System.Collections.Generic;
using System.IO;
using UnityEngine;

class PrizeFactory {

    List<Prize> Prizes;
    public PrizeFactory(TextAsset textAsset) {
        InitPrizeList(textAsset);
    }

    public Prize GetRandom() {
        return Prizes[Random.Range(0, Prizes.Count)];
    }

    private void InitPrizeList(TextAsset textAsset) {
        if (Prizes == null) {
            Prizes = new List<Prize>();
            string text = textAsset.text;
            string[] textSplits = textAsset.text.Split('\n');

            foreach (string str in textSplits) {
                Prizes.Add(new Prize(str));
            }
        }
    }
}