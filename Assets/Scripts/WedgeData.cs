using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WedgeData {
    public int Value;
    public string Text;
    public WedgeType WedgeType;

    public WedgeData(int value, string text, WedgeType type) {
        Value = value;
        Text = text;
        WedgeType = type;
    }
}
