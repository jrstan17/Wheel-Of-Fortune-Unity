using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class Utilities {

    public static readonly List<char> NonLetters = FillLetters(new char[] { '?', '!', ',', '\'', '&', '-', '.', ':' });
    public static readonly List<char> RSTLNE = FillLetters(new char[] { 'R', 'S', 'T', 'L', 'N', 'E' });

    private static List<char> FillLetters(char[] chars) {
        List<char> toReturn = new List<char>();
        foreach (char c in chars) {
            toReturn.Add(c);
        }

        return toReturn;
    }

    public static bool IsVowel(char c) {
        c = char.ToUpper(c);
        return (c == 'A' || c == 'E' || c == 'I' || c == 'O' || c == 'U');
    }

    //public static void SetFocus(Selectable obj) {
    //    EventSystem.current.SetSelectedGameObject(obj.gameObject, null);
    //    obj.OnPointerUp(new PointerEventData(EventSystem.current));
    //}
}