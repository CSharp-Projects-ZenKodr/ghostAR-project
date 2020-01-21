using UnityEngine;

public class HitGraphicLife : MonoBehaviour {

    /// <summary>
    /// Return true if the graphic needs to be destroyed.
    /// </summary>
    [Tooltip("Return true if the graphic needs to be destroyed.")]
    public bool destroyGraphic = false;

    private void Update()
    {
        if (destroyGraphic)
        {
            Destroy(gameObject);
        }
    }
}