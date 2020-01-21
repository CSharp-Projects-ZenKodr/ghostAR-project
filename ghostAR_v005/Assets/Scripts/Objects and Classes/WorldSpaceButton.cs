using System.Collections;

using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class WorldSpaceButton : MonoBehaviour, IPlayerHitable
{
    #region Variables
    /// <summary>
    /// The Color the button will change to when hit.
    /// </summary>
    [Tooltip("The Color the button will change to when hit.")]
    public Color pressedColor = Color.grey;

    /// <summary>
    /// The Event/Method that will invoke when the player hits the object.
    /// </summary>
    [Tooltip("The Event/Method that will invoke when the player hits the object.")]
    public UnityEvent OnHit;

    /// <summary>
    /// The initial color of the World Space Button;
    /// </summary>
    public Color InitialColor { get; set; }

    /// <summary>
    /// The SpriteRenderer attached to the World Space Button.
    /// </summary>
    public SpriteRenderer ButtonSprite { get; private set; }
    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, GetComponent<BoxCollider>().size);
        Gizmos.color = Color.red;
    }

    private void Start()
    {
        ButtonSprite = GetComponent<SpriteRenderer>();
        InitialColor = ButtonSprite.color;
    }

    /// <summary>
    /// The Behavior that happens when the player hits this button.
    /// </summary>
    public void OnPlayerHit()
    {
        ButtonSprite.color = pressedColor;
        StartCoroutine("TimeAsColor");
        OnHit.Invoke();
    }

    IEnumerator TimeAsColor ()
    {
        yield return new WaitForSeconds(0.5f);
        ButtonSprite.color = InitialColor;
        StopCoroutine("TimeAsColor");
    }
}
