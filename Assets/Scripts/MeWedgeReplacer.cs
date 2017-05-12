using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeWedgeReplacer : MonoBehaviour {

    float TimeStart = 0;
    public float TimeToJason = 300;
    public SpriteRenderer[] Bankrupts;
    public Sprite JasonSprite;

    bool isTimerGoing = false;

    public void StartJasonTimer() {
        TimeStart = Time.time;
        isTimerGoing = true;
    }
	
	// Update is called once per frame
	void Update () {
		if (isTimerGoing) {
            if (Time.time >= TimeStart + TimeToJason) {
                isTimerGoing = false;

                foreach(SpriteRenderer sr in Bankrupts) {
                    if (!sr.isVisible) {
                        sr.sprite = JasonSprite;
                        return;
                    }
                }
            }
        }
	}
}
