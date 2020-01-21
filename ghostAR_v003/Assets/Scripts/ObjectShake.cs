using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class ObjectShake : MonoBehaviour {
    #region Variables
    /// <summary>
    /// The Amount the object will shake.
    /// </summary>
    [Tooltip("The Amount the object will shake.")]
    public float shakeAmount = 5.0f;
    /// <summary>
    /// Return true to have the object shake.
    /// </summary>
    private bool shaking = false;
    /// <summary>
    /// The original position of the GameObject;
    /// </summary>
    private Vector3 originalPos;
    #endregion

    private void Update() {
        ShakeObj();
    }

    private void ShakeObj() {
        if (shaking) {
            Vector3 newPos = originalPos + Random.insideUnitSphere * (Time.deltaTime * shakeAmount);

            transform.position = newPos;
        }
    }

    /// <summary>
    /// Turns the shaking either on or off.
    /// </summary>
    /// <param name="ShakeBool">
    /// (bool) A bool that decides whether the shaking will be turned on or off.
    /// </param>
    public void ShakeMe(bool ShakeBool) {
        originalPos = transform.position;
        shaking = ShakeBool;
        if (!shaking) {
            transform.position = originalPos;
        }
    }
}