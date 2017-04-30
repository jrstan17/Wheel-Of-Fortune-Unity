using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGrabBag {

    private static List<int> Bag;

	private static void Init() {
        Bag = new List<int>();
        List<int> temp = new List<int>();

        for (int i = SpinWheel.minAngle; i <= SpinWheel.maxAngle; i++) {
            temp.Add(i);
        }

        while (temp.Count != 0) {
            int i = Random.Range(0, temp.Count);
            Bag.Add(temp[i]);
            temp.RemoveAt(i);
        }
    }

    public static int Grab() {
        if (Bag == null || Bag.Count == 0) {
            Init();
        }

        int toReturn = Bag[0];
        Bag.RemoveAt(0);
        return toReturn;
    }
}
