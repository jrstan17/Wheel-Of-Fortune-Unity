using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour {

    private bool loadScene = false;

    // Updates once per frame
    void Update() {
        if (!loadScene) {
            loadScene = true;
            StartCoroutine(gameObject.GetComponent<GameLoadingAnim>().BeginAnimation());
            StartCoroutine(LoadNewScene());
        }
    }


    // The coroutine runs on its own at the same time as Update() and takes an integer indicating which scene to load.
    IEnumerator LoadNewScene() {
        // Start an asynchronous operation to load the scene that was passed to the LoadNewScene coroutine.
        AsyncOperation async = SceneManager.LoadSceneAsync(0);

        // While the asynchronous operation to load the new scene is not yet complete, continue waiting until it's done.
        while (!async.isDone) {
            yield return null;
        }
    }
}
