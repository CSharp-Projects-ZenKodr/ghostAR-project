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
    [Tooltip ("The Health class of the player object.")]
    public Health playerHealth;

    /// <summary>
    /// "The Animator component attached to the DeathScreenItems GameObject."
    /// </summary>
    [Tooltip("The Animator component attached to the DeathScreenItems GameObject.")]
    public Animator deathScreenItemsAnimator;

    /// <summary>
    /// The Text component where the score will display.
    /// </summary>
    [Tooltip("The Text component where the score will display.")]
    public Text scoreValueText;

    [Tooltip ("The Text component where the high score will display.")]
    public Text highScoreValueText;

    /// <summary>
    /// The Text component where the player's health will display.
    /// </summary>
    [Tooltip("The Text component where the player's health will display.")]
    public Text healthDisplay;

    /// <summary>
    /// Player's current score.
    /// </summary>
    public static int Score { get; set; }
    #endregion
    #endregion

    private void Awake()
    {
        #region Getting Important Data
        if (Instance != null)
        {
            Debug.LogError("GameManager instance already set.");
            return;
        } else
        {
            Instance = this;
            Debug.Log("GameManager instance set to " + name);
        }
        highScoreValueText.text = PlayerPrefs.GetInt("HighScore").ToString("000000");
        #endregion
    }

    private void Update()
    {
        UpdateHealthDisplay();
        BringUpDeathItems();
    }

    void UpdateHealthDisplay()
    {
        healthDisplay.text = playerHealth.CurrentHealth + "   \n/" + playerHealth.maxHealth;
    }

    void BringUpDeathItems()
    {
        if (playerHealth.IsDead)
        {
            Debug.Log("Player has died.  Bringing up Death Items.");
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
    public void IncreaseScore (int scoreIncreaseValue)
    {
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
    /// Takes player back to the Start Menu.
    /// </summary>
    public void GoToStartMenu ()
    {
        SceneManager.LoadScene("startMenu");
    }

    /// <summary>
    /// Saves the score to a PlayerPref int called "HighScore" if the player got a high score.
    /// </summary>
    public void SaveScore ()
    {
        if (Score > PlayerPrefs.GetInt("HighScore")) {
            Text highScoreLabel = highScoreValueText.transform.parent.gameObject.GetComponent<Text>();

            highScoreLabel.text += " (NEW!)";

            PlayerPrefs.SetInt("HighScore", Score);
        }
    }
    #endregion
}