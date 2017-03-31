using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WedgeData : MonoBehaviour {
    public int Value;
    public string Text;
    public WedgeType WedgeType;

    private TextMesh textMesh;

	// Use this for initialization
	void Start () {
        gameObject.GetComponentInChildren<TextMesh>().text = Text;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
