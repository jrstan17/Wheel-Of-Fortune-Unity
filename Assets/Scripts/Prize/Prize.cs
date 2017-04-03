using UnityEngine;

class Prize {
        public string Text { get; set; }
        public int Value { get; set; }

        private Prize(string text, int value) {
            Text = text;
            Value = value;
        }

        public Prize(string line) {
            string[] splits = line.Split('\t');
            Text = splits[0];            

            int tempValue = int.Parse(splits[1]);
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
            return new Prize(Text, Value);
        }
    }