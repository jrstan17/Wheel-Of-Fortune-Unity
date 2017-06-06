using UnityEngine;

public class Prize {
    public int Number { get; set; }
    public string Text { get; set; }
    public string SajakText { get; set; }
    public int Value { get; set; }
    public Sprite Sprite { get; set; }

    private Prize(string text, string sajakText, int value) {
        Text = text;
        SajakText = sajakText;
        Value = value;
    }

    public Prize(string line, PrizeSpriteGetter spriteGetter) {
        string[] splits = line.Split('\t');

        Number = int.Parse(splits[0]);
        Sprite = spriteGetter.Get(Number);

        Text = splits[1];
        SajakText = splits[2];

        int tempValue = 3500;
        int wholePercentage = Random.Range(0, 10);
        double decimalPercentage = Random.value;
        double percentage = wholePercentage + decimalPercentage;
        percentage /= 100;

        bool isNeg;
        if (Random.Range(0, 2) == 0) {
            isNeg = true;
        } else {
            isNeg = false;
        }

        if (isNeg) {
            percentage = 1 - percentage;
        } else {
            percentage += 1;
        }

        Value = (int)System.Math.Round(tempValue * percentage);
    }

    public Prize DeepCopy() {
        return new Prize(Text, SajakText, Value);
    }
}