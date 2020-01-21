using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuManager : MonoBehaviour {
    
    public webCamScript wCS;

    #region Public Methods
    /// <summary>
    /// Something happens when logo is hit
    /// </summary>
    public void HitLogo ()
    {
        Debug.Log("Logo Hit.");
    }

    /// <summary>
    /// Takes the player to the gameplay scene.
    /// </summary>
    public void GoToGame ()
    {
        wCS.TurnOffEverything();
        SceneManager.LoadScene("gameplay");
    }

    /// <summary>
    /// Toggles the flashlight either on or off.
    /// </summary>
    public void FlashlightToggle ()
    {
        Debug.Log("Flashlight toggled.");
    }

    /// <summary>
    /// Quits the application
    /// </summary>
    public void QuitGame ()
    {
        Application.Quit();
    }
    #endregion
}