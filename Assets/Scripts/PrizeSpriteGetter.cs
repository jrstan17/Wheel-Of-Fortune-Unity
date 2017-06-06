using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizeSpriteGetter : MonoBehaviour {

    public Sprite[] PrizeSprites;

	public Sprite Get(int prizeNumber) {
        foreach(Sprite sprite in PrizeSprites) {
            if (sprite.name.Equals(prizeNumber.ToString())) {
                return sprite;
            }
        }

        return PrizeSprites[0];
    }
}
