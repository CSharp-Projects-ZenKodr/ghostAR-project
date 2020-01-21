using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EnemySpawner : MonoBehaviour {

    #region Variables
    [Header("LineRenderer Variables")]
    public int vertexCount = 40;
    public float lineWidth = 0.2f;
    public float radius = 1;
    #endregion

    private LineRenderer lineRen;

    private void Awake() {
        lineRen = GetComponent<LineRenderer>();
        SetupCircle();
    }

    private void SetupCircle() {
        lineRen.widthMultiplier = lineWidth;

        float deltaTheta = (2f * Mathf.PI) / vertexCount;
        float theta = 0f;

        lineRen.positionCount = vertexCount;
        for (int i = 0; i < lineRen.positionCount; i++) {
            Vector3 pos = new Vector3(radius * Mathf.Cos(theta), 0f, radius * Mathf.Sin(theta));
            lineRen.SetPosition(i, pos);
            theta += deltaTheta;
        }
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
}