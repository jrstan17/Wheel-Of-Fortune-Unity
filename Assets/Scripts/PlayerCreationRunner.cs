using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerCreationRunner : MonoBehaviour {

    public InputField AddPlayerField;
    public GameObject PlayerBar;
    public Text PlayerPrefab;
    public RoundRunner RoundRunner;

	public void AddPlayer_Clicked() {
        PlayerList.Players.Add(new Player(AddPlayerField.text));
        Text textClone = Instantiate(PlayerPrefab);
        textClone.color = new Color32(0, 255, 0, 255);
        textClone.text = AddPlayerField.text;
        textClone.transform.SetParent(PlayerBar.transform, false);
        AddPlayerField.text = "";
        EventSystem.current.SetSelectedGameObject(AddPlayerField.gameObject, null);
    }

    public void StartGame_Clicked() {
        PlayerList.RandomizePlayers();
        RoundRunner.Initialize();
    }

    public IEnumerator RandomizePlayers() {
        List<string> names = new List<string>();
        //TODO
        yield return 0;
    }
}
