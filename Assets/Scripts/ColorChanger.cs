using UnityEngine;
using System.Collections;

public class ColorChanger : MonoBehaviour {
    public Color FrontStartColor;
    public Color FrontFinishColor;
    public Color BackStartColor;
    public Color BackFinishColor;
    public float AverageSpeed;

    private SpriteRenderer sr;
    private TextMesh textMesh;
    private float speed;

    // Use this for initialization
    void Start () {
        sr = gameObject.GetComponent<SpriteRenderer>();
        textMesh = gameObject.transform.GetChild(0).GetComponent<TextMesh>();
        speed = PlusMinusRandomPercent(AverageSpeed, 0.1f);
    }
	
	// Update is called once per frame
	void Update () {
        sr.color = Color.Lerp(BackStartColor, BackFinishColor, Mathf.PingPong(Time.time * speed, 1));
        if (textMesh != null) {
            textMesh.color = Color.Lerp(FrontStartColor, FrontFinishColor, Mathf.PingPong(Time.time * speed, 1));
        }
    }

    float PlusMinusRandomPercent(float num, float maxPercent) {
        float percent = Random.Range(0, maxPercent);
        int pos = Random.Range(0, 1);

        if (pos == 1) {
            num *= (1 + percent);
        } else {
            num *= (1 - percent);
        }

        return num;
    }
}
