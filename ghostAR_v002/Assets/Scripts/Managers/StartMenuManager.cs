using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour {
    
    public webCamScript wCS;

    public AudioManager soundMan { get; set; }

    public Light dircLight;
    public Light flashlight;

    public TextMesh darkModeText;

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
        }
        if (darkModeValue >= 1) { //dark Mode on
            dircLight.transform.localEulerAngles = new Vector3(-77f, dircLight.transform.localEulerAngles.y, dircLight.transform.localEulerAngles.z);
            dircLight.intensity = 0.1f;
            flashlight.gameObject.SetActive(true);
            darkModeText.text = "Dark Mode\nToggle: " + "On";
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
        SceneManager.LoadScene("gameplay");
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