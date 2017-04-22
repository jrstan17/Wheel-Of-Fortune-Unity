using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomColorChanger : MonoBehaviour {

    public Text[] TextObjects;
    public SpriteRenderer[] SpriteObjects;
    public float Speed;

    internal Color32 start;
    internal Color32 finish;
    internal static bool isStarted = false;
    internal float elapsedTime = 0;
    internal List<SpriteRenderer> Subset;

	// Use this for initialization
	void Start () {
        start = GetRandomColor();
        finish = GetRandomColor();

        Subset = new List<SpriteRenderer>();
    }
	
	public void StartColorChange() {
        isStarted = true;
        StartCoroutine(Next());
    }

    public void SetSubset(List<Trilon> trilons) {
        foreach(SpriteRenderer sr in SpriteObjects) {
            Subset.Add(sr);
        }
    }

    public void StopColorChange() {
        isStarted = false;
    }

    private IEnumerator Next() {
        while (isStarted) {
            float r = Mathf.Lerp(start.r, finish.r, elapsedTime);
            float g = Mathf.Lerp(start.g, finish.g, elapsedTime);
            float b = Mathf.Lerp(start.b, finish.b, elapsedTime);
            Color32 nextColor = new Color32((byte)r, (byte)g, (byte)b, 255);

            if (TextObjects.Length != 0) {
                foreach (Text t in TextObjects) {
                    t.color = nextColor;
                }
            }

            if (Subset != null && Subset.Count != 0) {
                foreach (SpriteRenderer sr in Subset) {
                    sr.color = nextColor;
                }
            }

            elapsedTime += (Speed / 1000);

            if (elapsedTime > 1) {
                elapsedTime = 0;
                start = finish;
                finish = GetRandomColor();
            }

            yield return new WaitForEndOfFrame();
        }        
    }

    private static Color32 GetRandomColor() {
        return new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), 255);
    }
}
