using UnityEngine;

public class SimpleDestroy : MonoBehaviour {

    /// <summary>
    /// Return true to destroy the attached GameObject immediately.
    /// </summary>
    [Tooltip("Return true to destroy the attached GameObject immediately.")]
    public bool kill = false;
	
	void Update () {
		if (kill == true) {
            Destroy(gameObject);
        }
	}
}