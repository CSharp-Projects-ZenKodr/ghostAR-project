using System.Collections;

using UnityEngine;

public class HostileGhosterGhost : MonoBehaviour, IGhost, IPlayerHitable {
    #region Variables
    //========================Enums===============================
    #region Enums
    public enum FadeStatus { FadedOut, Fading, FadedIn };
    public FadeStatus currentFadeStatus;
    #endregion
    //========================Public-Variables====================
    #region Public Variables
    [Space(8)]
    [Header("Components Needed")]
    /// <summary>
    /// The value at which the Hostile will fade.
    /// </summary>
    [Tooltip("The value at which the Hostile will fade.")]
    [Range(0.0f, 1.0f)]
    public float fadeValue;

    [Space(8)]

    [Header("Movement")]
    /// <summary>
    /// The maximum value at which the Hostile will move.
    /// </summary>
    [Tooltip("The maximum value at which the Hostile will move.")]
    public float maximumMovementSpeed;
    /// <summary>
    /// The value at which the Hostile will accelerate.
    /// </summary>
    [Tooltip("The value at which the Hostile will accelerate.")]
    public float accelerationValue;
    /// <summary>
    /// The time that the Hostile will move around.
    /// </summary>
    [Tooltip("The time that the Hostile will move around.")]
    public float moveAroundTime;
    /// <summary>
    /// The value that the Hostile will damage the player.
    /// </summary>
    [Tooltip("The value that the Hostile will damage the player.")]
    public int damageValue;
    [Space(8)]
    [Header("Points")]
    /// <summary>
    /// The points awarded to the player if they hit this Hostile.
    /// </summary>
    [Tooltip("The points awarded to the player if they hit this Hostile.")]
    public int hitPoints;
    /// <summary>
    /// The points awarded to the player if they kill this Hostile
    /// </summary>
    [Tooltip("The points awarded to the player if they kill this Hostile.")]
    public int killPoints;

    [Space(8)]

    /// <summary>
    /// The Prefab that holds the Claw Scratch.
    /// </summary>
    [Tooltip("The Prefab that holds the Claw Scratch.")]
    public GameObject clawScratchPrefab;
    /// <summary>
    /// The Animator attached to the Hostile Model.
    /// </summary>
    [Tooltip("The Animator attached to the Hostile Model.")]
    public Animator ghostAnimator;
    /// <summary>
    /// The MeshRenderer attached to the hostile.
    /// </summary>
    [Tooltip("The MeshRenderer attached to the hostile.")]
    public SkinnedMeshRenderer ghostRenderer;
    /// <summary>
    /// The color the Hostile will change to when hit.
    /// </summary>
    [Tooltip("The color the Hostile will change to when hit.")]
    public Color hitColor = Color.white;

    /// <summary>
    /// The Combo System in the scene.
    /// </summary>
    public ComboSystem comSystem { get; set; }
    /// <summary>
    /// The Health component attached to the Player.
    /// </summary>
    public Health playerHealth { get; set; }
    /// <summary>
    /// The Health component attached to the Hostile.
    /// </summary>
    public Health ghostHealth { get; set; }
    /// <summary>
    /// The GameObject thats holds the player and their data.
    /// </summary>
    public GameObject playerObject { get; set; }
    /// <summary>
    /// The GameManager in the scene.
    /// </summary>
    public GameManager gameMan { get; set; }
    #endregion
    //========================Private-Variables===================
    #region Private Variables
    /// <summary>
    /// The full color of the Hostile.
    /// </summary>
    public Color fullColor { get; set; }
    /// <summary>
    /// The Main Canvas within the scene.
    /// </summary>
    private GameObject mainCanvas;
    /// <summary>
    /// The RectTransform attached to the Main Canvas.
    /// </summary>
    private RectTransform canvasRecTran;
    /// <summary>
    /// The graphic that dislays when Hostile is scaring the player.
    /// </summary>
    private ScareFade scareVisual;
    /// <summary>
    /// Return true if the Hostile is fading in.
    /// </summary>
    private bool fadingIn = true;
    /// <summary>
    /// Return true if the player is moving.
    /// </summary>
    private bool move = false;
    /// <summary>
    /// Return true to invert the direction that the Hostile moves.
    /// </summary>
    private bool invertTurnDirection = false;
    /// <summary>
    /// Return true if the Hostile is in its attack phase.
    /// </summary>
    private bool attackPhase = false;
    /// <summary>
    /// Return true if the Hostile is able to attack.
    /// </summary>
    private bool ableToAttack = true;
    /// <summary>
    /// Return true if the Hostile to fade out and die.
    /// </summary>
    private bool fadingOut = false;
    /// <summary>
    /// The spped that the Hostile is currently traveling at.
    /// </summary>
    private float currentSpeed = 0.0f;
    /// <summary>
    /// The Audio clip of the scare audio.
    /// </summary>
    private AudioSource scareClip;
    /// <summary>
    /// The Audio Manager in the scene.
    /// </summary>
    private AudioManager soundMan;
    #endregion
    #endregion

    //========================Methods=============================
    #region Methods
    private void Start() {
        #region Getting and Setting Important Data
        //=======================Getting================================
        #region Getting
        playerObject = GameObject.FindGameObjectWithTag("Player");
        comSystem = GameObject.FindGameObjectWithTag("Combo").GetComponent<ComboSystem>();
        ghostHealth = GetComponent<Health>();
        gameMan = playerObject.GetComponent<GameManager>();
        playerHealth = playerObject.GetComponent<Health>();
        mainCanvas = GameObject.FindGameObjectWithTag("Main Canvas");
        canvasRecTran = mainCanvas.GetComponent<RectTransform>();
        scareVisual = GameObject.FindObjectOfType<ScareFade>();
        scareClip = transform.GetChild(1).GetComponent<AudioSource>();
        //floatClip = transform.GetChild(1).GetComponent<AudioSource>();
        soundMan = GameObject.FindObjectOfType<AudioManager>();
        #endregion
        //=======================Setting================================
        #region Setting
        fullColor = new Color(0.8f, 0.8f , 0.8f, 0.8f);
        ghostHealth.invincible = true;
        #endregion
        #endregion
    }

    private void Update() {
        transform.LookAt(playerObject.transform);
        if (fadingIn) {
            if (currentFadeStatus == FadeStatus.FadedIn) {
                ghostHealth.invincible = false;
                PickDirection();
                move = true;
                StartCoroutine(ContinueMoving());
                fadingIn = false;
            }
        }
        if (fadingIn) {
            FadeModelIn();
        }
        CheckingFadeStatus();
        RestrictingValues();
        MovingAroundThePlayer();
        if (attackPhase) {
            HostileAttack();
        }
        GhostDeath();
        if (fadingOut) {
            FadeModelOut();
            if (currentFadeStatus == FadeStatus.FadedOut) {
                gameMan.IncreaseScore(killPoints);
                soundMan.Play("Gain Point");
                gameMan.IncreaseKillCountByOne();
                Destroy(gameObject);
            }
        }

        ghostAnimator.SetBool("Dead", ghostHealth.IsDead);
    }

    void GhostDeath() {
        if (ghostHealth.IsDead) {
            StopAllCoroutines();
            move = false;
            currentSpeed = 0.0f;
            ableToAttack = false;
            attackPhase = false;
            ghostHealth.invincible = true;
            fadingOut = true;
        }
    }

    public void CheckingFadeStatus() {
        float renAlpha = ghostRenderer.material.color.a;

        if (renAlpha <= 0) {
            currentFadeStatus = FadeStatus.FadedOut;
        }
        if (renAlpha > 0 && renAlpha < 1) {
            currentFadeStatus = FadeStatus.Fading;
        }
        if (renAlpha >= fullColor.a) {
            currentFadeStatus = FadeStatus.FadedIn;
        }
    }

    void HostileAttack () {
        if (currentSpeed <= 0) {
            if (ableToAttack) {
                StartCoroutine(AttackThePlayer());
                ableToAttack = false;
            }
        }
    }

    public void FadeModelIn () {
        Color temp = ghostRenderer.material.color;

        temp.a += fadeValue * Time.deltaTime;

        ghostRenderer.material.color = temp;
    }

    public void FadeModelOut () {
        Color temp = ghostRenderer.material.color;

        temp.a -= fadeValue * 2 * Time.deltaTime;

        ghostRenderer.material.color = temp;
    }

    public void PickDirection () {
        float ranPick = Random.Range(0.0f, 100.0f);

        if (ranPick < 50.0f) {
            invertTurnDirection = false;
        } else if (ranPick >= 50.0f) {
            invertTurnDirection = true;
        }
    }

    public void MovingAroundThePlayer () {
        if (move) {
            if (currentFadeStatus == FadeStatus.FadedIn) {
                if (currentSpeed < maximumMovementSpeed) {
                    currentSpeed += accelerationValue * Time.deltaTime;
                }
            }
        } else {
            if (currentSpeed > 0) {
                currentSpeed -= accelerationValue * Time.deltaTime;
            }
        }
        if (!invertTurnDirection) {
            transform.RotateAround(playerObject.transform.position, Vector3.up, currentSpeed * Time.deltaTime);
        } else {
            transform.RotateAround(playerObject.transform.position, Vector3.down, currentSpeed * Time.deltaTime);
        }
    }

    public void RestrictingValues() {
        if (ghostRenderer.material.color.a > fullColor.a) {
            ghostRenderer.material.color = fullColor;
        }
        if (ghostRenderer.material.color.a < Color.clear.a) {
            ghostRenderer.material.color = Color.clear;
        }
        if (currentSpeed >= maximumMovementSpeed) {
            currentSpeed = maximumMovementSpeed;
        }
        if (currentSpeed <= 0) {
            currentSpeed = 0.0f;
        }
    }

    public void OnPlayerHit() {
        if (!gameMan._PlayerIsDead) {
            if (!ghostHealth.invincible) {
                StopAllCoroutines();
                StartCoroutine(ShakeAndDamage());
            }
        }
    }
    #endregion

    #region Coroutines
    IEnumerator ContinueMoving () {
        yield return new WaitForSeconds(moveAroundTime);
        move = false;
        attackPhase = true;
        StopCoroutine(ContinueMoving());
    }

    IEnumerator AttackThePlayer () {
        yield return new WaitForSeconds(1.0f);
        #region Attacking or Scaring
        float ranPick = Random.Range(0f, 100f);

        if (ranPick >= 10f) {//Attack
            ghostAnimator.SetTrigger("Attack");
            yield return new WaitForSeconds(0.5f);
            GameObject a = Instantiate(clawScratchPrefab, mainCanvas.transform);
            a.transform.localPosition = new Vector2(Random.Range(0, canvasRecTran.anchoredPosition.x), Random.Range(0, canvasRecTran.anchoredPosition.y));
            playerHealth.DamageObject(damageValue);
            soundMan.Play("Take Damage");
            comSystem.InterruptCombo();
        }
        if (ranPick < 10f) {
            ghostAnimator.SetTrigger("Scare");
            scareClip.Play();
            yield return new WaitForSeconds(0.5f);
            scareVisual.ShowTrue();
        }
        #endregion
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(ContinueMoving());
        PickDirection();
        move = true;
        ableToAttack = true;
        attackPhase = false;
        StopCoroutine(AttackThePlayer());
    }

    IEnumerator ShakeAndDamage() {
        //Hard Halt
        ghostHealth.DamageObject(20);
        ghostHealth.invincible = true;
        StopCoroutine(ContinueMoving());
        move = false;
        currentSpeed = 0.0f;
        ableToAttack = false;
        attackPhase = false;
        //Trigger Damage Amimation
        ghostAnimator.SetTrigger("Damaged");
        //Damage Hostile
        ghostRenderer.material.color = hitColor;
        //Give Points
        gameMan.IncreaseScore(hitPoints);
        soundMan.Play("Gain Point");
        //Visualize Combo
        comSystem.IncreaseComboByOne();
        //Reset color
        yield return new WaitForSeconds(0.75f);
        ghostRenderer.material.color = fullColor;
        //Start Moving (Smooth)
        ghostHealth.invincible = false;
        PickDirection();
        ableToAttack = true;
        move = true;
        StartCoroutine(ContinueMoving());
        StopCoroutine(ShakeAndDamage());
    }
    #endregion
}