using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusWedgeData : MonoBehaviour {

    public BonusWheelRandomizer Randomizer;
    public Text Text;
    internal int PrizeAmount;

	// Use this for initialization
	void Start () {
        int index = Random.Range(0, Randomizer.BonusAmounts.Count);
        PrizeAmount = Randomizer.BonusAmounts[index];
        Randomizer.BonusAmounts.Remove(index);
	}

    void OnTriggerEnter2D(Collider2D col) {
        BonusRoundRunner.PrizeAmount = PrizeAmount;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
