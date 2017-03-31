using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour {

    public List<GameObject> Trilons;
    public List<GameObject> LetterObjects;

    internal List<TextMesh> Letters;
    internal List<SpriteRenderer> Screens;

	// Use this for initialization
	void Start () {
        Letters = new List<TextMesh>();
        Screens = new List<SpriteRenderer>();
        FillLetters();
        FillScreens();        
	}

    void FillScreens() {
        foreach (GameObject obj in Trilons) {
            SpriteRenderer screen = obj.transform.GetChild(0).GetComponent<SpriteRenderer>();
            Screens.Add(screen);
        }
    }

    void FillLetters() {
        foreach(GameObject obj in LetterObjects) {
            Letters.Add(obj.GetComponent<TextMesh>());
        }
    }
	
	// Update is called once per frame
	void Update () {

    }
}
