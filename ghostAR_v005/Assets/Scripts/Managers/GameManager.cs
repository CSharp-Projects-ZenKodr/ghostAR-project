using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    #region Variables
    #region Public Variables
    /// <summary>
    /// Instance of the one and only GameManager.
    /// </summary>
    public static GameManager Instance { get; private set; }
    /// <summary>
    /// The Health class of the player object.
    /// </summary>
    [Tooltip("The Health class of the player object.")]
    public Health playerHealth;
    /// <summary>
    /// "The Animator component attached to the DeathScreenItems GameObject."
    /// </summary>
    [Tooltip("The Animator component attached to the DeathScreenItems GameObject.")]
    public Animator deathScreenItemsAnimator;
    /// <summary>
    /// The WebCamScript in the scene.
    /// </summary>
    [Tooltip("The WebCamScript in the scene.")]
    public webCamScript wCS;
    /// <summary>
    /// The Text component where the score will display.
    /// </summary>
    [Tooltip("The Text component where the score will display.")]
    public Text scoreValueText;
    /// <summary>
    /// The Text component where the high score will display.
    /// </summary>
    [Tooltip("The Text component where the high score will display.")]
    public Text highScoreValueText;
    /// <summary>
    /// The Text component where the kill score will display.
    /// </summary>
    [Tooltip("The Text component where the kill score will display.")]
    public Text killScoreValueText;
    /// <summary>
    /// The Text component where the player's health will display.
    /// </summary>
    [Tooltip("The Text component where the player's health will display.")]
    public Text healthDisplay;
    /// <summary>
    /// The Text component where the player's kill count will display.
    /// </summary>
    [Tooltip("The Text component where the player's kill count will display.")]
    public Text killDisplay;
    /// <summary>
    /// The Directional Light within the scene.
    /// </summary>
    [Tooltip("The Directional Light within the scene.")]
    public Light dircLight;
    /// <summary>
    /// The flashlight the player will need to see.
    /// </summary>
    [Tooltip("The flashlight the player will need to see.")]
    public Light flashlight;

    /// <summary>
    /// Returns true if player is dead.
    /// </summary>
    public bool _PlayerIsDead { get; set; }

    /// <summary>
    /// The AudioManager in the scene.
    /// </summary>
    public AudioManager soundMan { get; set; }

    /// <summary>
    /// Player's current score.
    /// </summary>
    public static int Score { get; set; }
    /// <summary>
    /// Player's kill count.
    /// </summary>
    public static int KillCount { get; set; }
    #endregion

    /// <summary>
    /// The value that decides whether the player is in Dark Mode or not.
    /// </summary>
    private int darkModeValue;
    /// <summary>
    /// Return true if sound has been played.
    /// </summary>
    private bool played = false;
    #endregion

    private void Awake() {
        KillCount = 0;
        Score = 0;
        _PlayerIsDead = false;

        soundMan = FindObjectOfType<AudioManager>();

        #region Getting Important Data
        if (Instance != null) {
            Debug.LogError("GameManager instance already set.");
            return;
        } else {
            Instance = this;
            Debug.Log("GameManager instance set to " + name);
        }
        highScoreValueText.text = PlayerPrefs.GetInt("HighScore").ToString("000000");
        killScoreValueText.text = PlayerPrefs.GetInt("KillScore").ToString();
        #endregion
    }

    private void Update() {
        UpdateHealthDisplay();
        UpdateKillCountDisplay();
        BringUpDeathItems();

        darkModeValue = PlayerPrefs.GetInt("DarkMode");
        if (darkModeValue <= 0) { //dark Mode off
            dircLight.transform.localEulerAngles = new Vector3(50f, dircLight.transform.localEulerAngles.y, dircLight.transform.localEulerAngles.z);
            dircLight.intensity = 1f;
            flashlight.gameObject.SetActive(false);
        }
        if (darkModeValue >= 1) { //dark Mode on
            dircLight.transform.localEulerAngles = new Vector3(-77f, dircLight.transform.localEulerAngles.y, dircLight.transform.localEulerAngles.z);
            dircLight.intensity = 0.1f;
            flashlight.gameObject.SetActive(true);
        }
    }

    void UpdateHealthDisplay() {
        healthDisplay.text = playerHealth.CurrentHealth + "   \n/" + playerHealth.maxHealth;
    }

    void UpdateKillCountDisplay () {
        killDisplay.text = KillCount.ToString();
    }

    void BringUpDeathItems() {
        if (playerHealth.IsDead) {
            if (!played) {
                soundMan.Play("End Song");
                played = true;
            }
            Debug.Log("Player has died.  Bringing up Death Items.");
            _PlayerIsDead = true;
            deathScreenItemsAnimator.SetTrigger("showItems");
            SaveScore();
        }
    }

    #region Public Methods
    /// <summary>
    /// Increases the score by whatever value the method is fed.
    /// </summary>
    /// <param name="scoreIncreaseValue">
    /// (int) The value the score increases by.
    /// </param>
    public void IncreaseScore (int scoreIncreaseValue)  {
        Score += scoreIncreaseValue;
        scoreValueText.text = Score.ToString("000000");
        Debug.Log("Score has increased!\nNew Score is " + Score.ToString());
    }

    /// <summary>
    /// Decreases the score by whatever value the method is fed
    /// </summary>
    /// <param name="scoreDecreaseScore">
    /// (int) The value the score decreases by.
    /// </param>
    public void DecreaseScore (int scoreDecreaseScore) {
        Score -= scoreDecreaseScore;
        scoreValueText.text = Score.ToString("000000");
        Debug.Log("Score has decreased!\nNew Score is " + Score.ToString());

    }

    /// <summary>
    /// Increase the player's kill count by one.
    /// </summary>
    public void IncreaseKillCountByOne () {
        KillCount++;
    }

    /// <summary>
    /// Takes player back to the Start Menu.
    /// </summary>
    public void GoToStartMenu () {
        wCS.TurnOffEverything();
        SceneManager.LoadScene("startMenu");
    }

    /// <summary>
    /// Saves the score to a PlayerPref int called "HighScore" if the player got a high score.
    /// </summary>
    public void SaveScore ()  {
        if (Score > PlayerPrefs.GetInt("HighScore")) {
            highScoreValueText.text = Score.ToString();

            Text highScoreLabel = highScoreValueText.transform.parent.gameObject.GetComponent<Text>();

            highScoreLabel.text += " (NEW!)";

            PlayerPrefs.SetInt("HighScore", Score);
        }
        if (KillCount > PlayerPrefs.GetInt("KillScore")) {
            killScoreValueText.text = KillCount.ToString();

            Text killFlavor = killScoreValueText.transform.parent.gameObject.GetComponent<Text>();

            killFlavor.text += " (NEW!)";

            PlayerPrefs.SetInt("KillScore", KillCount);
        }
    }
    #endregion
}