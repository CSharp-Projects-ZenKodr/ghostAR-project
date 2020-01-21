using UnityEngine;
using System.Collections;

public class JustWalker : MonoBehaviour {
    /// <summary>
    /// The Jumpscare Ghost.
    /// </summary>
    [Tooltip("The Jumpscare Ghost.")]
    public GameObject ghostObject;

    /// <summary>
    /// The animator attached to this GameObject.
    /// </summary>
    Animator bansheeAnim;
    /// <summary>
    /// The sound that plays when the jumpscare happens.
    /// </summary>
    AudioSource scareNine;
    bool scarable = true;

	void Start () {
        bansheeAnim = GetComponent<Animator>();
        scareNine = GetComponent<AudioSource>();
        ghostObject.SetActive(false);
        bansheeAnim.SetBool("Just Walk", true);
	}

    /// <summary>
    /// Scares the player.
    /// </summary>
    public void ScarePlayer () {
        if (scarable) {
            scareNine.Play();
            ghostObject.SetActive(true);
            StartCoroutine(MakeInvisible());
            scarable = false;
        }
    }

    IEnumerator MakeInvisible() {
        yield return new WaitForSeconds(0.75f);
        ghostObject.SetActive(false);
        scarable = true;
        StopCoroutine(MakeInvisible());
    }
}
