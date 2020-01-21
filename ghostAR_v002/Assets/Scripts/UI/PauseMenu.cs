using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    #region Variables
    /// <summary>
    /// Returns true if the game is paused/
    /// </summary>
    public static bool GameIsPaused = false;

    /// <summary>
    /// The GameObject that hold the data for the UI.
    /// </summary>
    [Tooltip("The GameObject that hold the data for the UI.")]
    public GameObject pauseMenuUI;
    /// <summary>
    /// The Web Cam Script that is in the scene.
    /// </summary>
    [Tooltip("The Web Cam Script that is in the scene.")]
    public webCamScript wCS;
    /// <summary>
    /// The Image that holds the pause circle to be filled.
    /// </summary>
    [Tooltip("The Image that holds the pause circle to be filled.")]
    public Image pauseCircle;
    /// <summary>
    /// The Image that holds the pause bars that fade in.
    /// </summary>
    [Tooltip("The Image that holds the pause bars that fade in.")]
    public Image pauseBars;
    #endregion

    void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPaused) {
                Resume();
            } else {
                Pause();
            }
        }

        HoldToPause();
	}

    void HoldToPause() {
        if (!GameIsPaused) {
            pauseCircle.fillAmount = PlayerHit.PressHoldTime;

            Color temp = pauseBars.color;
            temp.a = PlayerHit.PressHoldTime;
            pauseBars.color = temp;
        }

        if (PlayerHit.PressHoldTime >= 1f) {
            Pause();
        }
    }

    #region Public Methods
    private void Pause() {
        pauseCircle.fillAmount = 1f;
        pauseBars.color = Color.white;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    /// <summary>
    /// Loads the Main Menu scene.
    /// </summary>
    public void LoadMainMenu () {
        Time.timeScale = 1f;
        wCS.TurnOffEverything();
        SceneManager.LoadScene("startMenu");
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void Resume() {
        pauseCircle.fillAmount = 0f;
        pauseBars.color = new Color(1f, 1f, 1f, 0f);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }
    #endregion
}