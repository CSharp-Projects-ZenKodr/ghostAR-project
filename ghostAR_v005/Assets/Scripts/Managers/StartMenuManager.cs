using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour {
    /// <summary>
    /// The Web Cam Script in the scene.
    /// </summary>
    [Tooltip("The Web Cam Script in the scene.")]
    public webCamScript wCS;

    /// <summary>
    /// The Audio Manager within the scene.
    /// </summary>
    public AudioManager soundMan { get; set; }

    /// <summary>
    /// The directional light that will change in the scene.
    /// </summary>
    [Tooltip("The directional light that will change in the scene.")]
    public Light dircLight;
    /// <summary>
    /// The Player's flashlight.
    /// </summary>
    [Tooltip("The Player's flashlight.")]
    public Light flashlight;

    /// <summary>
    /// The Text on the Dark Mode Toggle button.
    /// </summary>
    [Tooltip("The Text on the Dark Mode Toggle button.")]
    public TextMesh darkModeText;
    /// <summary>
    /// The text that tells the player to turn on their flashlight.
    /// </summary>
    [Tooltip("The text that tells the player to turn on their flashlight.")]
    public GameObject flashlightNotice;

    /// <summary>
    /// The value that decides wether the player is in Dark Mode or not.
    /// </summary>
    private int darkModeValue;

    private void Start() {
        soundMan = FindObjectOfType<AudioManager>();
    }

    private void Update() {
        darkModeValue = PlayerPrefs.GetInt("DarkMode");
        //darkModeValue = 1;
        if (darkModeValue <= 0) { //dark Mode off
            dircLight.transform.localEulerAngles = new Vector3(50f, dircLight.transform.localEulerAngles.y, dircLight.transform.localEulerAngles.z);
            dircLight.intensity = 1f;
            flashlight.gameObject.SetActive(false);
            darkModeText.text = "Dark Mode\nToggle: " + "Off";
            flashlightNotice.SetActive(false);
        }
        if (darkModeValue >= 1) { //dark Mode on
            dircLight.transform.localEulerAngles = new Vector3(-77f, dircLight.transform.localEulerAngles.y, dircLight.transform.localEulerAngles.z);
            dircLight.intensity = 0.1f;
            flashlight.gameObject.SetActive(true);
            darkModeText.text = "Dark Mode\nToggle: " + "On";
            flashlightNotice.SetActive(true);
        }
    }

    #region Public Methods
    /// <summary>
    /// Something happens when logo is hit
    /// </summary>
    public void HitLogo ()
    {
        Debug.Log("Logo Hit.");
        soundMan.Play("Button Boop");
    }

    /// <summary>
    /// Takes the player to the gameplay scene.
    /// </summary>
    public void GoToGame ()
    {
        wCS.TurnOffEverything();
        if (PlayerPrefs.GetInt("ViewedTutorial") < 10) {
            SceneManager.LoadScene("blankScene");
        } else {
            SceneManager.LoadScene("gameplay");
        }
        //SceneManager.LoadScene("blankScene");
        soundMan.Play("Button Boop");
    }

    /// <summary>
    /// Toggles the flashlight either on or off.
    /// </summary>
    public void DarkModeToggle () {
        soundMan.Play("Button Boop");
        if (darkModeValue <= 0) { //If Dark Mode Off
            PlayerPrefs.SetInt("DarkMode", 1);
        } else if (darkModeValue >= 1) { //If Dark Mode On
            PlayerPrefs.SetInt("DarkMode", 0);
        }
    }

    /// <summary>
    /// Quits the application
    /// </summary>
    public void QuitGame ()
    {
        Application.Quit();
        soundMan.Play("Button Boop");
    }
    #endregion
}