using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour {

    public Sprite[] Sprites;
    public Image[] ButtonImages;

    internal Coroutine wildCardFlash;
    public static float FLASH_SPEED = 0.5f;

    public void ToggleFreePlay(bool enable) {
        if (enable) {
            ButtonImages[0].sprite = Sprites[1];
        } else {
            ButtonImages[0].sprite = Sprites[0];
        }
    }

    public void TogglePrize(bool enable) {
        if (enable) {
            ButtonImages[2].sprite = Sprites[5];
        } else {
            ButtonImages[2].sprite = Sprites[4];
        }
    }

    public void ToggleMillion(bool enable) {
        if (enable) {
            ButtonImages[3].sprite = Sprites[7];
        } else {
            ButtonImages[3].sprite = Sprites[6];
        }
    }

    public void ToggleWild(IconState state) {
        if (wildCardFlash != null) {
            StopCoroutine(wildCardFlash);
            wildCardFlash = null;
        }

        if (state == IconState.Enabled) {
            ButtonImages[1].sprite = Sprites[3];
        } else if (state == IconState.Disabled) {
            ButtonImages[1].sprite = Sprites[2];
        } else if (state == IconState.Flashing) {
            wildCardFlash = StartCoroutine(FlashCoroutine());
        }
    }

    public IEnumerator FlashCoroutine() {
        while (true) {
            if (ButtonImages[1].sprite == Sprites[3]) {
                ButtonImages[1].sprite = Sprites[2];
            } else {
                ButtonImages[1].sprite = Sprites[3];
            }

            yield return new WaitForSeconds(FLASH_SPEED);
        }
    }

    public void WildCard_Clicked() {
        if (KeyPress.IsTimeForWildDecision) {
            RoundRunner RoundRunner = GetComponent<RoundRunner>();

            RoundRunner.ToggleUIButtonsParsing("all", false);

            RoundRunner.SajakText.text = "Please select another consonant for " + RoundRunner.CurrentWedge.Value.ToString("N0") + ".";
            PlayerList.CurrentPlayer.Wilds--;

            if (WedgeRules.RoundUsesWedge(RoundRunner, WedgeType.Wild)) {
                GameObject WheelBaseObject = RoundRunner.WheelCanvas.transform.GetChild(0).gameObject;
                int index = WedgeRules.GetWedgeChangeIndex("wild", WheelBaseObject);
                WedgeChangeContainer wildChange =
                    WheelBaseObject.GetComponents<WedgeChangeContainer>()[index];
                wildChange.ToggleBefore();
            }

            if (PlayerList.CurrentPlayer.Wilds == 0) {
                ToggleWild(IconState.Disabled);
            } else {
                ToggleWild(IconState.Enabled);
            }

            KeyPress.IsTimeForWildDecision = false;
            RoundRunner.IsTimeForLetter = true;
        }
    }
}
