using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
/// <summary>
/// Unity Behavior's eventsystem needs a:
/// 1a) collider and phyics raycaster component on the camera(for 3d gameobject) or
/// 1b) a graphics raycaster component on the Canvas (for UI)
/// 2) a monobehavior script (like the one below)
/// 3) and an eventsystem gameobject (added by default when you create a UI->Canvas, 
///     only one can exist in the scene) to work
/// 
/// This script simply catches click events related to the object and passes it to where 
/// you need it to go
/// </summary>
public class OnClickRelay : MonoBehaviour, IPointerClickHandler {

    public PlayerCreationRunner pcr;

    public void OnPointerClick(PointerEventData eventData) {
        //passes data to your button controller (or whater you need to know about the
        //button that was pressed), informing that this game object was pressed.
        pcr.Name_Clicked(gameObject, eventData);
    }
}
