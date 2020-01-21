using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class ComboSystem : MonoBehaviour {
    #region Variables
    /// <summary>
    /// The Text component where the combo points will display.
    /// </summary>
    [Tooltip("The Text component where the combo points will display.")]
    public Text pointDisplayText;
    /// <summary>
    /// The shadow component attached to the point display.
    /// </summary>
    [Tooltip("The shadow component attached to the point display.")]
    public Shadow pointDisplayShadow;
    /// <summary>
    /// The point flavor Text component that does not change.
    /// </summary>
    [Tooltip("The point flavor Text component that does not change.")]
    public Text pointFlavorText;
    /// <summary>
    /// The shadow component attached to the point flavor text.
    /// </summary>
    [Tooltip("The shadow component attached to the point flavor text.")]
    public Shadow pointFlavorShadow;
    /// <summary>
    /// The Text component that displays the best combo the player has gotten.
    /// </summary>
    [Tooltip("The Text component that displays the best combo the player has gotten.")]
    public Text bestComboText;
    /// <summary>
    /// The shadow component attached to the best combo Text.
    /// </summary>
    [Tooltip("The shadow component attached to the best combo Text.")]
    public Shadow bestComboShadow;
    /// <summary>
    /// The image that represents that time the player has to get another combo point.
    /// </summary>
    [Tooltip("The image that represents that time the player has to get another combo point.")]
    public Image timeBarImage;
    /// <summary>
    /// The outline component attached to the time bar.
    /// </summary>
    [Tooltip("The outline component attached to the time bar.")]
    public Outline timeBarOutline;

    [Space(8)]

    /// <summary>
    /// The time the player has to get another hit in the combo.
    /// </summary>
    [Tooltip("The time the player has to get another hit in the combo.")]
    public float comboTime = 3.0f;

    /// <summary>
    /// The time the player has left to increase the combo.
    /// </summary>
    private float timeLeft;

    /// <summary>
    /// The current score within the current combo.
    /// </summary>
    public int ComboScore { get; set; }

    /// <summary>
    /// The Best Combo that the player has currently aquired.
    /// </summary>
    public int BestComboValue { get; set; }
    #endregion

    private void Start() {
        ComboScore = 0;
    }

    private void Update() {
        TestTestTest();
        ComboDisplayManagement();
        TimerBarBehavior();
    }

    void TestTestTest() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            IncreaseComboByOne();
        }
        if (Input.GetKeyDown(KeyCode.V)) {
            InterruptCombo();
        }
    }

    void TimerBarBehavior() {
        if (timeLeft > 0) {
            timeLeft -= Time.deltaTime;
            timeBarImage.fillAmount = timeLeft / comboTime;
        }
        if (timeLeft <= 0) {
            ResetCombo();
        }
    }

    void ComboDisplayManagement() {
        pointDisplayText.text = ComboScore.ToString("00");
        if (ComboScore >= 5) {
            ChangeAlphaInDisplays(1.0f);
        } else if (ComboScore == 4) {
            ChangeAlphaInDisplays(0.8f);
        } else if (ComboScore == 3) {
            ChangeAlphaInDisplays(0.6f);
        } else if (ComboScore == 2) {
            ChangeAlphaInDisplays(0.4f);
        } else if (ComboScore == 1) {
            ChangeAlphaInDisplays(0.2f);
        } else if (ComboScore <= 0) {
            ChangeAlphaInDisplays(0.1f);
        }
    }

    void ChangeAlphaInDisplays(float newAlpha) {
        Color tempA = pointDisplayText.color;
        Color tempB = pointDisplayShadow.effectColor;
        Color tempC = pointFlavorText.color;
        Color tempD = pointFlavorShadow.effectColor;
        Color tempE = timeBarImage.color;
        Color tempF = timeBarOutline.effectColor;
        Color tempG = bestComboText.color;
        Color tempH = bestComboShadow.effectColor;

        tempA.a = newAlpha;
        tempB.a = newAlpha;
        tempC.a = newAlpha;
        tempD.a = newAlpha;
        tempE.a = newAlpha;
        tempF.a = newAlpha;
        tempG.a = newAlpha;
        tempH.a = newAlpha;
        
        pointDisplayText.color = tempA;
        pointDisplayShadow.effectColor = tempB;
        pointFlavorText.color = tempC;
        pointFlavorShadow.effectColor = tempD;
        timeBarImage.color = tempE;
        timeBarOutline.effectColor = tempF;
        bestComboText.color = tempG;
        bestComboShadow.effectColor = tempH;
    }

    /// <summary>
    /// Increase Combo Score by One.
    /// </summary>
    public void IncreaseComboByOne () {
        ComboScore++;
        timeBarImage.fillAmount = 1.0f;
        timeLeft = comboTime;
        EvaluateBestCombo();
    }

    void ResetCombo () {
        ComboScore = 0;
        ChangeAlphaInDisplays(0.1f);
    }

    void EvaluateBestCombo () {
        if ( BestComboValue < ComboScore ) {
            bestComboText.text = "Best Combo: " + ComboScore.ToString("00");
            BestComboValue = ComboScore;
        }
    }

    /// <summary>
    /// Interupt the current ongoing combo.
    /// </summary>
    public void InterruptCombo () {
        timeLeft = 0;
        timeBarImage.fillAmount = 0.0f;
        ResetCombo();
        EvaluateBestCombo();
    }
}
