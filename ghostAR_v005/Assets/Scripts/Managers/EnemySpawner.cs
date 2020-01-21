using System.Collections;

using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EnemySpawner : MonoBehaviour {

    #region Variables
    [Header("Circle Variables")]
    /// <summary>
    /// The Amount of Vertices within the circle.
    /// </summary>
    [Tooltip("The Amount of Vertices within the circle.")]
    public int vertexCount = 16;
    /// <summary>
    /// The Radius of the Circle.
    /// </summary>
    [Tooltip("The Radius of the Circle.")]
    public float radius = 1;

    [Header("Objects to Spawn")]
    /// <summary>
    /// The prefab that holds the Hostile Ghost data.
    /// </summary>
    [Tooltip("The prefab that holds the Hostile Ghost data.")]
    public GameObject hostileGhost;
    /// <summary>
    /// The prefab that holds the Friendly Ghost data.
    /// </summary>
    [Tooltip("The prefab that holds the Friendly Ghost data.")]
    public GameObject friendlyGhost;

    [Space(15)]

    /// <summary>
    /// The maximum amount of ghosts allowed in the scene in the early game.
    /// </summary>
    [Tooltip("The maximum amount of ghosts allowed in the scene in the early game.")]
    public int startingGhostLimit;
    /// <summary>
    /// The time it takes to cooldown for another spawn.
    /// </summary>
    [Tooltip("The time it takes to cooldown for another spawn.")]
    public float spawnCooldownTime;

    /// <summary>
    /// The Line Renderer attached to this GameObject.
    /// </summary>
    private LineRenderer lineRen;
    /// <summary>
    /// The current limit of how many ghosts can be in the scene.
    /// </summary>
    private int currentLimit;
    /// <summary>
    /// The next number the adaptive difficulty expects to up the difficulty.
    /// </summary>
    private int numberToExpect = 2;
    /// <summary>
    /// Return true to try to spawn an enemy.
    /// </summary>
    private bool spawnEnemy = false;

    /// <summary>
    /// A count of all of the ghosts within the scene.
    /// </summary>
    public int GhostsInScene { get; set; }
    #endregion

    private void Awake() {
        lineRen = GetComponent<LineRenderer>();
        currentLimit = startingGhostLimit;
        lineRen.widthMultiplier = 0.05f;
        StartCoroutine(SpawnCooldown());
    }

    private void Update() {
        CountGhostsOnScreen();
        GhostSpawning();
        GettingVertices();
        AdaptiveDifficulty();
    }

    void AdaptiveDifficulty() {
        if (GameManager.KillCount == numberToExpect) {
            currentLimit++;
            numberToExpect *= 2;
        }
    }

    private void GettingVertices() {
        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        lineRen.positionCount = vertexCount;
        for (int i = 0; i < vertexCount; i++) {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), 0, radius * Mathf.Sin(theta));
            lineRen.SetPosition(i, pos);
            theta += deltaTheta;
        }
    }

    void GhostSpawning() {
        float ranPick = Random.Range(0.0f, 100.0f);

        if (spawnEnemy) {
            if (GhostsInScene < currentLimit) {
                if (ranPick > 20.0f) {
                    Instantiate(hostileGhost, lineRen.GetPosition(Random.Range(0, lineRen.positionCount)), transform.rotation);
                } else if (ranPick <= 20.0f) {
                    Instantiate(friendlyGhost, lineRen.GetPosition(Random.Range(0, lineRen.positionCount)), transform.rotation);
                }
            }
            StartCoroutine(SpawnCooldown());
            spawnEnemy = false;
        }

        if (Input.GetKeyUp(KeyCode.H)) {
            currentLimit++;
        }
    }

    void CountGhostsOnScreen() {
        GhostsInScene =
            (GameObject.FindGameObjectsWithTag("Hostile").Length + GameObject.FindGameObjectsWithTag("Friendly").Length);

        Debug.Log(GhostsInScene);
    }

    private void OnDrawGizmos() {
        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        Vector3 oldPos = Vector3.zero;
        for (int i = 0; i < vertexCount + 1; i++) {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), 0f, radius * Mathf.Sin(theta));
            Gizmos.DrawLine(oldPos, transform.position + pos);
            oldPos = transform.position + pos;

            theta += deltaTheta;
        }
    }

    IEnumerator SpawnCooldown () {
        yield return new WaitForSeconds(spawnCooldownTime);
        spawnEnemy = true;
        StopCoroutine(SpawnCooldown());
    }
}