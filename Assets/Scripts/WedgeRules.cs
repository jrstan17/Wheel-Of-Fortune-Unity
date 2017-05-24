using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WedgeRules : MonoBehaviour {

	public static bool RoundUsesWedge(RoundRunner runner, WedgeType type) {
        int currentRound = runner.RoundNumber;
        int maxRounds = runner.MaxRounds;
        int currentWheel = runner.wheelGetter.Get(currentRound);

        if (type == WedgeType.Million) {
            return currentRound != maxRounds;
        }

        if (type == WedgeType.Wild) {
            return currentWheel == 0 || currentWheel == 1; 
        }

        if (type == WedgeType.TenThousand) {
            return currentWheel == runner.WheelCanvases.Length - 1;
        }

        return false;
    }

    public static int GetWedgeChangeIndex(string ruleName, GameObject wheelBaseObject) {
        WedgeChangeContainer[] containers = wheelBaseObject.GetComponents<WedgeChangeContainer>();

        for (int i = 0; i < containers.Length; i++) {
            if (containers[i].RuleName.Equals(ruleName)) {
                return i;
            }
        }

        return -1;
    }
}
