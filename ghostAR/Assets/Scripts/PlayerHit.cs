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
    /// The Camera in the scene.
    /// </summary>
    [Tooltip("The Camera in the scene.")]
    public Camera cam;

    /// <summary>
    /// The Canvas in the scene.
    /// </summary>
    [Tooltip("The Canvas in the scene.")]
    public Canvas sceneCanvas;

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

            if (touch.phase == TouchPhase.Ended)
            {
                GameObject spawnedObj = Instantiate(hitGraphic, sceneCanvas.transform);
                spawnedObj.transform.position = touch.position;

                Ray ray = cam.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    GameObject targetHit = hit.collider.gameObject;

                    IPlayerHitable hitFunc = targetHit.GetComponent<IPlayerHitable>();

                    hitFunc.OnPlayerHit();
                }
            }

            
        }
    }
}