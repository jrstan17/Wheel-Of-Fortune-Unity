using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoadingAnim : MonoBehaviour {

    public SpriteRenderer[] Periods;
    public SpriteRenderer[] PeriodScreens;
    public Sprite PeriodSprite;

	public IEnumerator BeginAnimation() {
        while (gameObject.activeSelf) {
            TurnAllScreensToBlank();
            yield return Wait();
            TurnScreenToBlue(0);
            yield return Wait();
            TurnScreenToPeriod(0);
            yield return Wait();
            TurnScreenToBlue(1);
            yield return Wait();
            TurnScreenToPeriod(1);
            yield return Wait();
            TurnScreenToBlue(2);
            yield return Wait();
            TurnScreenToPeriod(2);
            yield return Wait();
        }
    }

    private void TurnScreenToBlue(int index) {
        Periods[index].sprite = null;
        PeriodScreens[index].color = Color.blue;
    }

    private void TurnScreenToPeriod(int index) {
        Periods[index].sprite = PeriodSprite;
        PeriodScreens[index].color = Color.white;
    }

    private void TurnScreenToBlank(int index) {
        Periods[index].sprite = null;
        PeriodScreens[index].color = Color.white;
    }

    private void TurnAllScreensToBlank() {
        foreach(SpriteRenderer sr in Periods) {
            sr.sprite = null;
        }

        foreach (SpriteRenderer sr in PeriodScreens) {
            sr.color = Color.white;
        }
    }

    private IEnumerator Wait() {
        yield return new WaitForSeconds(1.5f);
    }
}
