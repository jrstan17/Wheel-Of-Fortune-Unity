using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PrizeFactory : MonoBehaviour{

    List<Prize> Prizes;
    PrizeSpriteGetter PrizeSpriteGetter;

    private void Start() {
        PrizeSpriteGetter = gameObject.GetComponent<PrizeSpriteGetter>();
    }

    public Prize GetRandom() {
        return Prizes[Random.Range(0, Prizes.Count)];
    }

    public void InitPrizeList(TextAsset textAsset) {
        if (Prizes == null) {
            Prizes = new List<Prize>();
            string text = textAsset.text;
            string[] textSplits = textAsset.text.Split('\n');

            foreach (string str in textSplits) {
                if (!str.Equals("") && str[0] != '@') {
                    Prizes.Add(new Prize(str, PrizeSpriteGetter));
                }
            }
        }
    }
}