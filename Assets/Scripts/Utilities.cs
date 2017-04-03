using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

class Utilities {

    public static Random rnd = new Random();

    public static bool IsVowel(char c) {
        c = char.ToUpper(c);
        return (c == 'A' || c == 'E' || c == 'I' || c == 'O' || c == 'U');
    }

    //public static void SetFocus(Selectable obj) {
    //    EventSystem.current.SetSelectedGameObject(obj.gameObject, null);
    //    obj.OnPointerUp(new PointerEventData(EventSystem.current));
    //}
}