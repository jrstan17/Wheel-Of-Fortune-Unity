using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entry {
    internal string Name;
    internal int Value;

    public Entry(string tabSplit) {
        string[] splits = tabSplit.Split(' ');
        Name = splits[0];
        Value = int.Parse(splits[1]);
    }

    public Entry(string name, int value) {
        Name = name;
        Value = value;
    }
}
