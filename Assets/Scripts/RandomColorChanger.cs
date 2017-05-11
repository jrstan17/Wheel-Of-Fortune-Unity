using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomColorChanger : MonoBehaviour {

    public Text[] TextObjects;
    internal Image Image;
    public MeshRenderer MeshRenderer;
    public SpriteRenderer Sprite;
    public float Speed;
    public byte Alpha = 255;

    Material material;
    internal Color32 start;
    internal Color32 finish;
    internal static bool isStarted = false;
    internal float elapsedTime = 0;

	// Use this for initialization
	void Start () {
        start = GetRandomColor();
        finish = GetRandomColor();

        if (MeshRenderer != null) {
            material = MeshRenderer.materials[0];
        }
    }
	
	public void StartColorChange() {
        isStarted = true;
        StartCoroutine(Next());
    }

    public void StopColorChange() {
        isStarted = false;
    }

    private IEnumerator Next() {
        while (isStarted) {
            float r = Mathf.Lerp(start.r, finish.r, elapsedTime);
            float g = Mathf.Lerp(start.g, finish.g, elapsedTime);
            float b = Mathf.Lerp(start.b, finish.b, elapsedTime);
            Color32 nextColor = new Color32((byte)r, (byte)g, (byte)b, Alpha);

            if (TextObjects.Length != 0) {
                foreach (Text t in TextObjects) {
                    t.color = nextColor;
                }
            }

            if (Image != null) {
                Image.color = nextColor;
            }

            if (Sprite != null) {
                Sprite.color = nextColor;
            }

            if (material != null) {
                material.color = nextColor;
            }

            elapsedTime += (Speed / 1000);

            if (elapsedTime > 1) {
                elapsedTime = 0;
                start = finish;
                finish = GetRandomColor();
            }

            yield return new WaitForFixedUpdate();
        }        
    }

    private Color32 GetRandomColor() {
        return new Color32((byte)Random.Range(0, 256), (byte)Random.Range(0, 256), (byte)Random.Range(0, 256), Alpha);
    }
}
