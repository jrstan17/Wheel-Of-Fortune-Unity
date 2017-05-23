using System.Collections;
using System.Collections.Generic;

public class WheelGetter {

	public static int Get(int currentRound, int totalWheels, int totalRounds) {
        if (totalRounds == 1) {
            return 0;
        }

        int roundsPerWheelBase = (totalRounds - 1) / (totalWheels - 1);

        List<Range> ranges = new List<Range>();

        for (int i = 0; i < totalRounds - 1; i++) {

        }

        //ranges.Add(new Range() { startRound = 1, stopRound = 2, wheel = 3 });

    }

    private class Range {
        public int startRound = 0;
        public int stopRound = 0;
        public int wheel = 0;
    }
}
