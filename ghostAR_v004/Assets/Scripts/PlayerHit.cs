using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class PlayerHit : MonoBehaviour {

    #region Variables
    /// <summary>
    /// The prefab of the hitGraphic object that will spawn on player touch.
    /// </summary>
    [Tooltip("The prefab of the hitGraphic object that will spawn on player touch.")]
    public GameObject hitGraphic;
    /// <summary>
    /// The prefab of the missGraphic that will spawn on player touch.
    /// </summary>
    [Tooltip("The prefab of the missGraphic that will spawn on player touch.")]
    public GameObject missGraphic;
    /// <summary>
    /// The Camera in the scene.
    /// </summary>
    [Tooltip("The Camera in the scene.")]
    public Camera cam;
    /// <summary>
    /// The Canvas in the scene.
    /// </summary>
    [Tooltip("The Canvas in the scene.")]
    public Canvas sceneCanvas;

    /// <summary>
    /// Returns the amount of time the current Touch has been pressed onto the screen;
    /// </summary>
    public static float PressHoldTime { get; set; }
    #endregion

    private void Update ()
    {
        PlayerTouch();
    }

    void PlayerTouch()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            #region On Touch Ended
            if (touch.phase == TouchPhase.Ended) {
                GameObject spawnedObj = null;

                Ray ray = cam.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (!PauseMenu.GameIsPaused) {
                    if (Physics.Raycast(ray, out hit)) { //If the player hit something
                        GameObject targetHit = hit.collider.gameObject;

                        IPlayerHitable hitFunc = targetHit.GetComponent<IPlayerHitable>();

                        hitFunc.OnPlayerHit();

                        spawnedObj = Instantiate(hitGraphic, sceneCanvas.transform);
                    } else {
                        spawnedObj = Instantiate(missGraphic, sceneCanvas.transform);
                    }
                    AudioSource spawnedAudio = spawnedObj.GetComponent<AudioSource>();

                    spawnedAudio.pitch = Random.Range(0.9f, 1.25f);

                    spawnedObj.transform.position = touch.position;
                }

                PressHoldTime = 0f;
            }
            #endregion

            #region On Touch Stay
            if ((touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)) {
                if (PressHoldTime < 1f) {
                    PressHoldTime += Time.deltaTime;
                }
                if (PressHoldTime >= 1f) {
                    PressHoldTime = 1f;
                }
            }
            #endregion
        }
    }
}