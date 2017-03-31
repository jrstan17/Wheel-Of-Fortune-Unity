using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardFiller : MonoBehaviour {

    public List<Row> Rows;

    List<Trilon> Trilons;
    Data data;
    Puzzle Puzzle;

    public void InitBoard(Puzzle Puzzle) {
        this.Puzzle = Puzzle;

        GameObject dataHolder = GameObject.FindGameObjectWithTag("DataHolder");
        data = dataHolder.GetComponent<Data>();

        ClearBoard();

        Rows = new List<Row>();
        Rows.Add(new Row(12));
        Rows.Add(new Row(14));
        Rows.Add(new Row(14));
        Rows.Add(new Row(12));

        string[] splits = Puzzle.Text.Split(' ');
        string[] remainders = null;

        remainders = Rows[1].AddAnswer(splits);

        if (remainders.Length != 0) {
            remainders = Rows[2].AddAnswer(remainders);
        }

        if (remainders.Length != 0) {
            if (Random.Range(0, 2) == 0) {
                this.Clear();
                remainders = null;

                remainders = Rows[1].AddAnswer(splits);
                remainders = Rows[2].AddAnswer(remainders);
                remainders = Rows[3].AddAnswer(remainders);

                if (remainders.Length != 0) {
                    this.Clear();
                    remainders = null;

                    remainders = Rows[0].AddAnswer(splits);
                    remainders = Rows[1].AddAnswer(remainders);
                    remainders = Rows[2].AddAnswer(remainders);
                    remainders = Rows[3].AddAnswer(remainders);
                }
            } else {
                this.Clear();
                remainders = null;

                remainders = Rows[0].AddAnswer(splits);
                remainders = Rows[1].AddAnswer(remainders);
                remainders = Rows[2].AddAnswer(remainders);


                if (remainders.Length != 0) {
                    this.Clear();
                    remainders = null;

                    remainders = Rows[0].AddAnswer(splits);
                    remainders = Rows[1].AddAnswer(remainders);
                    remainders = Rows[2].AddAnswer(remainders);
                    remainders = Rows[3].AddAnswer(remainders);
                }
            }
        }

        Trilons = new List<Trilon>();
        foreach (Row r in Rows) {
            foreach (Trilon t in r.Trilons) {
                Trilons.Add(t);
            }
        }

        FillBoard();
    }

    public void FillBoard() {
        for(int i = 0; i < Trilons.Count; i++) {
            string letter = Trilons[i].Letter.ToString();
            data.Letters[i].text = letter;

            if (Trilons[i].State != TrilonState.NotInUse) {
                if (char.IsLetter(Trilons[i].Letter)) {
                    data.Letters[i].color = Color.white;
                } else {
                    data.Letters[i].color = Color.black;
                }

                data.Screens[i].color = Color.white;
            }
        }
    }

    public void ClearBoard() {
        for (int i = 0; i < data.Screens.Count; i++) {
            data.Screens[i].color = new Color32(0, 128, 0, 255);
            //data.Letters[i].text = "";
        }
    }

    public void RevealBoard() {
        for (int i = 0; i < Trilons.Count; i++) {
            if (!data.Letters[i].text.Equals("")) {
                data.Letters[i].color = Color.black;
                Trilons[i].Reveal();
            }
        }
    }

    public void RevealLetter(char c) {
        c = char.ToUpper(c);

        for(int i = 0; i < Trilons.Count; i++) {
            if (data.Letters[i].text.Equals(c.ToString())) {
                data.Letters[i].color = Color.black;
                Trilons[i].Reveal(c);
            }
        }
    }

    public void Clear() {
        foreach (Row r in Rows) {
            r.Clear();
        }
    }
}
