using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ScareFade : MonoBehaviour {
    /// <summary>
    /// The speed at which the graphic will fade in.
    /// </summary>
    [Tooltip("The speed at which the graphic will fade in.")]
    public float fadeInSpeed;
    /// <summary>
    /// The speed at which the graphic will fade out.
    /// </summary>
    [Tooltip("The speed at which the graphic will fade out.")]
    public float fadeOutSpeed;
    /// <summary>
    /// The amount of time the script will wait to make ShowGraphic false.
    /// </summary>
    [Tooltip("The amount of time the script will wait to make ShowGraphic false.")]
    public float WaitTime;

    public enum MathFunctions { ADD, SUBTRACT }

    /// <summary>
    /// Return true if the Scare Graphic is to be seen.
    /// </summary>
    public static bool showGraphic = false;

    /// <summary>
    /// The Image attached to the attached GameObject.
    /// </summary>
    private Image scareImage;

    private void Start() {
        scareImage = GetComponent<Image>();
    }

    private void Update() {
        if (showGraphic) {
            ChangeGraphicAlpha(fadeInSpeed, MathFunctions.ADD);
            StartCoroutine(ShowFalse());
        } else {
            ChangeGraphicAlpha(fadeOutSpeed, MathFunctions.SUBTRACT);
        }
    }

    void ChangeGraphicAlpha (float changeValue, MathFunctions functionApplied) {
        Color temp = scareImage.color;

        if (functionApplied == MathFunctions.ADD) {
            temp.a += changeValue * Time.deltaTime;
        } else if (functionApplied == MathFunctions.SUBTRACT) {
            temp.a -= changeValue * Time.deltaTime;
        }

        #region Restricting Values
        if (temp.a >= 1f) {
            temp.a = 1f;
        }
        if (temp.a <= 0f) {
            temp.a = 0f;
        }
        #endregion

        scareImage.color = temp;
    }

    /// <summary>
    /// Sets Show Graphic to true;
    /// </summary>
    public void ShowTrue () {
        showGraphic = true;
    }

    IEnumerator ShowFalse() {
        yield return new WaitForSeconds(WaitTime);
        showGraphic = false;
        StopCoroutine(ShowFalse());
    }
}