using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalfBoardLetterRandomizer : MonoBehaviour {

    public LetterGetter LetterGetter;
    public float RandomizingSpeed;
    public SpriteRenderer[] LetterRenderers;

    Coroutine coroutine;

    public void StartRandomizing() {
        coroutine = StartCoroutine(RandomizingCoroutine());
    }

    public void StopRandomizing() {
        StopCoroutine(coroutine);
    }

    public IEnumerator RandomizingCoroutine() {
        while (true) {
            foreach (SpriteRenderer sr in LetterRenderers) {
                if (sr.sprite == null) {
                    StopRandomizing();
                }

                sr.sprite = LetterGetter.GetRandomLetter();
            }

            yield return new WaitForSeconds(RandomizingSpeed);
        }
    }
}
