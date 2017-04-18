using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Populator : MonoBehaviour {

    public Text[] Names;
    public Text[] Values;

    public void Refresh() {
        List<Entry> Entries = HighScore.Entries;

        for (int i = 0; i < Entries.Count; i++) {
            Names[i].text = (i + 1) + ")";

            if (i + 1 < 10) {
                Names[i].text += "\t\t";
            } else {
                Names[i].text += "\t";
            }

            Names[i].text += Entries[i].Name;
            Values[i].text = Entries[i].Value.ToString("C0");
        }
    }
}
