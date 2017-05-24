using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    public Sprite[] Sprites;
    public SpriteRenderer[] Renderers;

    public void ToggleFreePlay(bool enable) {
        if (enable) {
            Renderers[0].sprite = Sprites[1];
        } else {
            Renderers[0].sprite = Sprites[0];
        }
    }

    public void ToggleWild(bool enable) {
        if (enable) {
            Renderers[1].sprite = Sprites[3];
        } else {
            Renderers[1].sprite = Sprites[2];
        }
    }

    public void TogglePrize(bool enable) {
        if (enable) {
            Renderers[2].sprite = Sprites[5];
        } else {
            Renderers[2].sprite = Sprites[4];
        }
    }

    public void ToggleMillion(bool enable) {
        if (enable) {
            Renderers[3].sprite = Sprites[7];
        } else {
            Renderers[3].sprite = Sprites[6];
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
