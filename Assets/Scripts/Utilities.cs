using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class Utilities {

    public static readonly List<char> NonLetters = FillLetters(new char[] { '?', '!', ',', '\'', '&', '-', '.', ':' });
    public static readonly List<char> RSTLNE = FillLetters(new char[] { 'R', 'S', 'T', 'L', 'N', 'E' });
    public static readonly List<char> Vowels = FillLetters(new char[] { 'A', 'E', 'I', 'O', 'U' });
    public static readonly List<char> Consonants = FillLetters(new char[] { 'B', 'C', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'V', 'W', 'X', 'Y', 'Z' });
    public static readonly List<Color32> RoundColors = FillColors();

    private static List<char> FillLetters(char[] chars) {
        List<char> toReturn = new List<char>();
        foreach (char c in chars) {
            toReturn.Add(c);
        }

        return toReturn;
    }

    private static List<Color32> FillColors() {
        List<Color32> toReturn = new List<Color32>();
        toReturn.Add(new Color32(0, 70, 0, 255)); //green        
        toReturn.Add(new Color32(0, 70, 70, 255)); //indigo
        toReturn.Add(new Color32(70, 0, 70, 255)); //purple
        toReturn.Add(new Color32(70, 70, 0, 255)); //yellow
        toReturn.Add(new Color32(128, 128, 128, 255)); //white
        toReturn.Add(new Color32(70, 0, 0, 255)); //red
        toReturn.Add(new Color32(0, 70, 70, 255)); //indigo
        toReturn.Add(new Color32(70, 0, 70, 255)); //purple
        toReturn.Add(new Color32(70, 70, 0, 255)); //yellow
        toReturn.Add(new Color32(128, 128, 128, 255)); //white
        toReturn.Add(new Color32(70, 0, 0, 255)); //red
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