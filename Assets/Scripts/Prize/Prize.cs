using UnityEngine;

class Prize {
    public string Text { get; set; }
    public string SajakText { get; set; }
    public int Value { get; set; }

    private Prize(string text, string sajakText, int value) {
        Text = text;
        SajakText = sajakText;
        Value = value;
    }

    public Prize(string line) {
        string[] splits = line.Split('\t');
        Text = splits[0];
        SajakText = splits[1];

        int tempValue = int.Parse(splits[2]);
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