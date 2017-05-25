using System.Collections;
using System.Collections.Generic;

public class WheelGetter {

    private List<int> wheelsData = new List<int>();

    public void Init(int rounds, int wheels) {
        rounds--;
        wheels--;

        double wheelsPerRound = (double)wheels / rounds;

        double wheelsAddend = 0;
        for (int i = 0; i < rounds; i++) {
            wheelsData.Add((int)wheelsAddend);
            wheelsAddend += wheelsPerRound + 0.0000001;
        }

        wheelsData.Add(wheels);
    }

    public int Get(int currentRound) {
        return wheelsData[currentRound - 1];
    }

    public void Set(int currentRound, int wheelNumber) {
        wheelsData[currentRound - 1] = wheelNumber - 1;
    }
}
