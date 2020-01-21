using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour {

    private void Start() {
        PlayerPrefs.GetInt("ViewedTutorial");
    }

    /// <summary>
    /// Loads the gameplay scene.
    /// </summary>
    public void LoadGame () {
        PlayerPrefs.SetInt("ViewedTutorial", 20);
        SceneManager.LoadScene("gameplay");
    }
}