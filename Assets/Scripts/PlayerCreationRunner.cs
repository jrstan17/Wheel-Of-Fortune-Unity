using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text;

public class PlayerCreationRunner : MonoBehaviour {

    public const int MAX_PLAYERS = 6;

    public InputField AddPlayerField;
    public GameObject PlayerBar;
    public Button PlayerPrefab;
    public RoundRunner RoundRunner;
    public Button AddPlayerButton;
    public Button StartButton;

    private List<Button> playerButtons = new List<Button>();
    private List<string> names = new List<string>();

    void Start() {        
        EventSystem.current.SetSelectedGameObject(AddPlayerField.gameObject, null);
    }

    void Update() {
        if (Input.GetKey(KeyCode.Return)) {
            AddPlayerButton.onClick.Invoke();
        }
    }

    public bool ToggleStartButton() {
        if (playerButtons.Count > 1) {
            StartButton.interactable = true;
        } else {
            StartButton.interactable = false;
        }

        return StartButton.interactable;
    }

    public void AddPlayer_Clicked() {
        EventSystem.current.SetSelectedGameObject(AddPlayerButton.gameObject, null);

        if (IsNameValid(AddPlayerField.text)) {
            Button playerButton = Instantiate(PlayerPrefab);
            playerButton.gameObject.SetActive(true);
            Text playerText = playerButton.transform.GetChild(0).GetComponent<Text>();
            playerText.color = new Color32(0, 255, 0, 255);
            playerText.text = AddPlayerField.text;
            playerText.text = InsureFirstLetterUppercase(playerText.text);
            playerText.raycastTarget = true;
            playerButton.transform.SetParent(PlayerBar.transform, false);
            AddPlayerField.text = "";
            playerButtons.Add(playerButton);
            ToggleStartButton();
        }

        EventSystem.current.SetSelectedGameObject(AddPlayerField.gameObject, null);
    }

    public void Name_Clicked(GameObject obj, PointerEventData ped) {
        foreach (Button b in playerButtons) {
            b.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            b.transform.GetChild(0).GetComponent<Text>().color = new Color32(128, 128, 128, 255);
        }

        Button button = obj.GetComponent<Button>();
        Text text = obj.transform.GetChild(0).GetComponent<Text>();
        button.GetComponent<Image>().color = new Color32(0, 0, 0, 255);
        text.color = new Color32(200, 200, 200, 255);

        AddPlayerField.text = text.text;
    }

    public void RemovePlayer_Clicked() {
        for (int i = 0; i < PlayerBar.transform.childCount; i++) {
            GameObject go = PlayerBar.transform.GetChild(i).gameObject;
            if (go.transform.GetChild(0).GetComponent<Text>().text.Equals(AddPlayerField.text)) {
                AddPlayerField.text = "";
                playerButtons.Remove(go.GetComponent<Button>());
                Destroy(go);

                if (!ToggleStartButton()) {
                    EventSystem.current.SetSelectedGameObject(AddPlayerField.gameObject, null);
                }

                return;
            }
        }
    }

    private string InsureFirstLetterUppercase(string name) {
        if (char.IsUpper(name[0])) {
            return name;
        }

        StringBuilder sb = new StringBuilder();

        sb.Append(char.ToUpper(name[0]));

        for (int i = 1; i < name.Length; i++) {
            sb.Append(name[i]);
        }

        return sb.ToString();
    }

    private bool IsNameValid(string name) {
        return !(name == null || name.Length == 0);
    }

    public void StartGame_Clicked() {
        foreach (Button b in playerButtons) {
            names.Add(b.GetComponentInChildren<Text>().text);
        }

        StartCoroutine(RandomizePlayers());
    }

    public IEnumerator RandomizePlayers() {
        float start = Time.time;
        float randomizeDuration = 5f;

        while (Time.time < (start + randomizeDuration)) {
            yield return StartCoroutine(RandomizePlayerLoop());
        }

        foreach(string name in names) {
            PlayerList.Players.Add(new Player(name));
        }

        PlayerList.RandomizePlayers();
        RoundRunner.Initialize();
    }

    private IEnumerator RandomizePlayerLoop() {
        List<string> temp = new List<string>();

        List<string> namesCopy = new List<string>();
        foreach(string str in names) {
            namesCopy.Add(str);
        }

        for (int i = 0; i < names.Count; i++) {
            int index = Random.Range(0, namesCopy.Count);
            temp.Add(namesCopy[index]);
            namesCopy.RemoveAt(index);
        }

        for (int i = 0; i < playerButtons.Count; i++) {
            playerButtons[i].transform.GetChild(0).GetComponent<Text>().text = temp[i];
        }

        yield return new WaitForSeconds(0.25f);
    }
}
