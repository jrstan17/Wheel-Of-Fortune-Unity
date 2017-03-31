using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trilon {

    internal TrilonState State;
    internal char Letter;

    public Trilon(TrilonState state) {
        State = state;
    }

    public Trilon DeepCopy() {
        Trilon t = new Trilon(State);
        t.Letter = this.Letter;
        return t;
    }

    public void SetInUse(char letter) {
        Letter = char.ToUpper(letter);

        if (char.IsLetter(letter)) {
            State = TrilonState.Unrevealed;
        } else {
            State = TrilonState.Revealed;
        }
    }

    public bool Reveal(char letter) {
        if (Letter == char.ToUpper(letter)) {
            State = TrilonState.Revealed;
            return true;
        }

        return false;
    }

    public void Reveal() {
        if (State == TrilonState.Unrevealed) {
            State = TrilonState.Revealed;
        }
    }

    public void SetNotInUse() {
        State = TrilonState.NotInUse;
    }

    public override string ToString() {
        if (State == TrilonState.NotInUse) {
            return "*";
        } else if (State == TrilonState.Revealed) {
            return Letter.ToString();
        } else {
            //return Letter.ToString();
            if (char.IsLetter(Letter)) {
                return "#";
            } else {
                return Letter.ToString();
            }
        }
    }
}
