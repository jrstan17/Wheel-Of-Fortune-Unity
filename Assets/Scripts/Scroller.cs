using UnityEngine;
using System.Collections;

public class Scroller : MonoBehaviour {

    public float xYSpeed;
    public float rotateSpeed;

    Vector2 offset;

    // Use this for initialization
    void Start() {
        int toggle = Random.Range(0, 2);
        if (toggle == 1) {
            rotateSpeed = -rotateSpeed;
        }
        toggle = Random.Range(0, 2);
        if (toggle == 1) {
            xYSpeed = -xYSpeed;
        }
    }

    // Update is called once per frame
    void Update() {
        offset = new Vector2(Time.time * xYSpeed, Time.time * xYSpeed * 2f);
        gameObject.GetComponent<Renderer>().material.mainTextureOffset = offset;
        gameObject.transform.Rotate(new Vector3(0, 0, rotateSpeed));
    }
}
