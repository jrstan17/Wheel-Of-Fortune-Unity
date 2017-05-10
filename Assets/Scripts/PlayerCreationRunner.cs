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
    public Text PlayerPrefab;
    public RoundRunner RoundRunner;
    public Texture2D CursorTexture;
    public Button AddPlayerButton;

    private List<string> names = new List<string>();
    private List<Text> textboxes = new List<Text>();

    void Start() {
        Cursor.SetCursor(CursorTexture, new Vector2(0, 0), CursorMode.ForceSoftware);
        EventSystem.current.SetSelectedGameObject(AddPlayerField.gameObject, null);
    }

    void Update() {
        if (Input.GetKey(KeyCode.Return)) {
            AddPlayerButton.onClick.Invoke();
        }
    }

    public void AddPlayer_Clicked() {
        EventSystem.current.SetSelectedGameObject(AddPlayerButton.gameObject, null);

        if (IsNameValid(AddPlayerField.text)) {
            Text textClone = Instantiate(PlayerPrefab);
            textClone.color = new Color32(0, 255, 0, 255);
            textClone.text = AddPlayerField.text;
            textClone.text = InsureFirstLetterUppercase(textClone.text);
            textClone.raycastTarget = true;
            textClone.transform.SetParent(PlayerBar.transform, false);
            AddPlayerField.text = "";
            names.Add(textClone.text);
            textboxes.Add(textClone);

            PlayerList.Players.Add(new Player(textClone.text));
        }

        EventSystem.current.SetSelectedGameObject(AddPlayerField.gameObject, null);
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
        StartCoroutine(RandomizePlayers());
    }

    public IEnumerator RandomizePlayers() {
        float start = Time.time;
        float randomizeDuration = 5f;
        
        while (Time.time < (start + randomizeDuration)) {
            yield return StartCoroutine(RandomizePlayerLoop());
        }

        PlayerList.RandomizePlayers();
        RoundRunner.Initialize();
    }

    private IEnumerator RandomizePlayerLoop() {
        List<string> temp = new List<string>();

        List<string> namesCopy = new List<string>();
        foreach (string str in names) {
            namesCopy.Add(str);
        }

        for (int i = 0; i < names.Count; i++) {
            int index = Random.Range(0, namesCopy.Count);
            temp.Add(namesCopy[index]);
            namesCopy.RemoveAt(index);
        }

        for (int i = 0; i < textboxes.Count; i++) {
            textboxes[i].text = temp[i];
        }

        yield return new WaitForSeconds(0.1f);
    }
}
