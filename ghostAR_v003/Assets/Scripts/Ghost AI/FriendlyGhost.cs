using System.Collections;

using UnityEngine;

public class FriendlyGhost : MonoBehaviour, IGhost, IPlayerHitable {
    #region Variables
    //===========================Enums====================================
    public enum FadeStatus { FadedOut, Fading, FadedIn };
    public FadeStatus currentFadeStatus;
    //============================Public-Variables===================================
    #region Public Variables
    /// <summary>
    /// The Hostile Prefab this friendly will spawn when it is killed.
    /// </summary>
    [Tooltip("The Hostile Prefab this friendly will spawn when it is killed.")]
    public GameObject hostileToSpawn;
    /// <summary>
    /// The value at which the ghost model will fade.
    /// </summary>
    [Tooltip("The value at which the ghost model will fade.")]
    public float fadeValue;
    /// <summary>
    /// The maximum value that the friendly will move at.
    /// </summary>
    [Tooltip("The maximum value that the friendly will move at.")]
    public float maximumMovementValue;
    /// <summary>
    /// The value at which the friendly will accelerate at.
    /// </summary>
    [Tooltip("The value at which the friendly will accelerate at.")]
    public float accerationValue;
    /// <summary>
    /// The amount of time the friendly will roam around before disappearing.
    /// </summary>
    [Tooltip("The amount of time the friendly will roam around before disappearing.")]
    public float roamTime;
    /// <summary>
    /// The value at which the friendly will heal the player.
    /// </summary>
    [Tooltip("The value at which the friendly will heal the player.")]
    public int healValue;
    /// <summary>
    /// The Amount of points that will be taken from the score when the friendly is killed.
    /// </summary>
    [Tooltip("The Amount of points that will be taken from the score when the friendly is killed.")]
    public int friendlyKillPoints;
    /// <summary>
    /// The Combo System in the scene.
    /// </summary>
    public ComboSystem comSystem { get; set; }
    /// <summary>
    /// The full color the friendly will fade to.
    /// </summary>
    public Color fullColor { get; set; }
    /// <summary>
    /// The GameObject that holds the player and their data.
    /// </summary>
    public GameObject playerObject { get; set; }
    /// <summary>
    /// The Health component attached to the friendly.
    /// </summary>
    public Health ghostHealth { get; set; }
    /// <summary>
    /// The MeshRenderer attached to the friendly.
    /// </summary>
    public MeshRenderer ghostRenderer { get; set; }
    /// <summary>
    /// The GameManager within the scene.
    /// </summary>
    public GameManager gameMan { get; set; }
    /// <summary>
    /// The ObjectShake attached to this friendly.
    /// </summary>
    public ObjectShake ghostShake { get; set; }
    #endregion
    //===========================Private-Variables====================================
    #region Private Variables
    /// <summary>
    /// The audio clip that plays if the friendly heals the player.
    /// </summary>
    private AudioSource healPop;
    /// <summary>
    /// The audio clip that plays the friendly saying "Stop".
    /// </summary>
    private AudioSource stopClip;
    /// <summary>
    /// The audio clip that plays the friendly saying "Left".
    /// </summary>
    private AudioSource leftClip;
    /// <summary>
    /// The audio clip that plays the friendly saying "Right".
    /// </summary>
    private AudioSource rightClip;
    /// <summary>
    /// The Health component attached to the player.
    /// </summary>
    public Health playerHealth { get; set; }
    /// <summary>
    /// The Hostile this friendly references to when giving advice.
    /// </summary>
    private GameObject referenceGhost;
    /// <summary>
    /// The Particle System that plays when the ghost heals the player.
    /// </summary>
    public ParticleSystem ghostHealParticles { get; set; }
    /// <summary>
    /// Return true if the friendly gives health to the player. \n
    /// Return false if the friendly gives advice to the player.
    /// </summary>
    public bool giveHealth { get; set; }
    /// <summary>
    /// Returns true if the model is in the "fading in" stage.
    /// </summary>
    private bool fadingIn = true;
    /// <summary>
    /// Returns true if the model is in the "fading out stage."
    /// </summary>
    private bool fadingOut = false;
    /// <summary>
    /// Returns true if the friendly is in the "moving" stage.
    /// </summary>
    private bool move = false;
    /// <summary>
    /// Return true to have the direction the friendly travels in inverted.
    /// </summary>
    private bool invertTurnDirection = false;
    /// <summary>
    /// The speed the friendly is currently moving at.
    /// </summary>
    private float currentSpeed = 0.0f;
    /// <summary>
    /// The distance between this friendly and its referenced Hostile.
    /// </summary>
    public float distanceFromHostile { get; set; }
    #endregion
    //===========================Properties-Variables====================================
    /// <summary>
    /// Returns true if the friendly is in the "Moving" stage.
    /// </summary>
    public bool _Moving { get { return move; } }
    /// <summary>
    /// The speed that the friendly is currently moving at.
    /// </summary>
    public float _CurrentSpeed { get { return currentSpeed; } }
    #endregion

    void Start () {
        #region Getting Information
        ghostHealth = GetComponent<Health>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        comSystem = GameObject.FindGameObjectWithTag("Combo").GetComponent<ComboSystem>();
        ghostRenderer = GetComponent<MeshRenderer>();
        playerHealth = playerObject.GetComponent<Health>();
        ghostHealParticles = GetComponent<ParticleSystem>();
        gameMan = playerObject.GetComponent<GameManager>();
        ghostShake = GetComponent<ObjectShake>();
        healPop = transform.GetChild(0).GetComponent<AudioSource>();
        stopClip = transform.GetChild(1).GetComponent<AudioSource>();
        leftClip = transform.GetChild(2).GetComponent<AudioSource>();
        rightClip = transform.GetChild(3).GetComponent<AudioSource>();
        referenceGhost = GameObject.FindGameObjectWithTag("Hostile");
        #endregion
        #region Setting Information
        PickBehavior();
        ghostHealth.invincible = true;
        fullColor = new Color(0.8f, 0.8f, 0.8f, 0.8f);
        #endregion
    }

    void Update () {
        if (referenceGhost != null) {
            distanceFromHostile = transform.position.x - referenceGhost.transform.position.x;
        }
        transform.LookAt(playerObject.transform);
        if (fadingIn) {
            if ( currentFadeStatus == FadeStatus.FadedIn ) {
                ghostHealth.invincible = false;
                PickDirection();
                move = true;
                StartCoroutine(Lifespan());
                fadingIn = false;
            }
        }

		if (fadingIn) {
            FadeModelIn();
        }

        if (fadingOut) {
            FadeModelOut();
            if (currentFadeStatus == FadeStatus.FadedOut) {
                Destroy(gameObject);
            }
        }

        CheckingFadeStatus();
        RestrictingValues();
        MovingAroundThePlayer();
    }

    void PickBehavior() {
        float ranPick = Random.Range(0.0f, 100.0f);

        if (ranPick > 25.0f) {
            giveHealth = false;
        } else if (ranPick <= 25.0f) {
            giveHealth = true;
        }
    }

    public void PickDirection () {
        float ranPick = Random.Range(0.0f, 100.0f);

        if (ranPick < 50.0f) {
            invertTurnDirection = false;
        } else if (ranPick >= 50.0f) {
            invertTurnDirection = true;
        }
    }

    public void MovingAroundThePlayer() {
        if (move) {
            if (currentFadeStatus == FadeStatus.FadedIn) {
                if (currentSpeed < maximumMovementValue) {
                    currentSpeed += accerationValue * Time.deltaTime;
                }
            }
        } else {
            if (currentSpeed > 0) {
                currentSpeed -= accerationValue * Time.deltaTime;
            }
        }


        if (!invertTurnDirection) {
            transform.RotateAround(playerObject.transform.position, Vector3.up, currentSpeed * Time.deltaTime);
        } else {
            transform.RotateAround(playerObject.transform.position, Vector3.down, currentSpeed * Time.deltaTime);
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

    public void FadeModelIn () {
        Color temp = ghostRenderer.material.color;

        temp.a += fadeValue * Time.deltaTime;

        ghostRenderer.material.color = temp;
    }

    public void FadeModelOut () {
        Color temp = ghostRenderer.material.color;

        temp.a -= fadeValue * Time.deltaTime;

        ghostRenderer.material.color = temp;
    }

    public void RestrictingValues() {
        if (ghostRenderer.material.color.a > fullColor.a) {
            ghostRenderer.material.color = fullColor;
        }
        if (ghostRenderer.material.color.a < Color.clear.a) {
            ghostRenderer.material.color = Color.clear;
        }
        if (currentSpeed >= maximumMovementValue) {
            currentSpeed = maximumMovementValue;
        }
        if (currentSpeed <= 0) {
            currentSpeed = 0.0f;
        }
    }

    public void OnPlayerHit() {
        if (!gameMan._PlayerIsDead) {
            if (!ghostHealth.invincible) {
                if (ghostHealth.CurrentHealth > 0) {
                    StartCoroutine(HitBehavior());
                } else if (ghostHealth.CurrentHealth <= 0) {
                    StartCoroutine(DeathBehavior());
                }
            }
        }
    }

    #region Coroutines
    IEnumerator DeathBehavior () {
        fadingOut = true;
        comSystem.InterruptCombo();
        Instantiate(hostileToSpawn, transform.position, transform.rotation);

        ghostHealth.invincible = true;
        move = false;
        currentSpeed = 0;

        gameMan.DecreaseScore(friendlyKillPoints);
        ghostShake.shakeAmount *= 2;
        ghostShake.ShakeMe(true);
        
        yield return new WaitForSeconds(2.0f);

        ghostShake.ShakeMe(false);

        StopCoroutine(DeathBehavior());
    }

    IEnumerator HitBehavior() {
        if (ghostHealth.CurrentHealth > 0) {
            if (ghostHealth.CurrentHealth == ghostHealth.maxHealth) {
                if (giveHealth) { //Give the player health
                    ghostHealParticles.Play();
                    playerHealth.HealObject(healValue);
                    healPop.Play();
                } else { //Give the player advise
                    if (referenceGhost != null) {
                        if (Mathf.Sign(distanceFromHostile) == 1) {         //Play Left
                            leftClip.Play();
                        } else if (Mathf.Sign(distanceFromHostile) == -1) { //Play Right
                            rightClip.Play();
                        }
                    } else {
                        stopClip.Play();
                    }
                }
            } else if ((ghostHealth.CurrentHealth < ghostHealth.maxHealth) && (ghostHealth.CurrentHealth > 0)) {
                ghostShake.ShakeMe(true);
                stopClip.Play();
            }
        }
        ghostHealth.DamageObject(1);
        ghostHealth.invincible = true;
        move = false;
        currentSpeed = 0;

        if (ghostHealth.CurrentHealth > 0) {
            comSystem.IncreaseComboByOne();
        } else {
            comSystem.InterruptCombo();
        }

        yield return new WaitForSeconds(0.75f);

        ghostShake.ShakeMe(false);
        move = true;
        ghostHealth.invincible = false;

        StopCoroutine(HitBehavior());
    }

    IEnumerator Lifespan () {
        yield return new WaitForSeconds(roamTime);
        ghostHealth.invincible = true;
        fadingOut = true;
        StopCoroutine(Lifespan());
    }
    #endregion
}