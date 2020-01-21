using UnityEngine;
using System.Collections;

public class JustWalker : MonoBehaviour {

    public GameObject ghostObject;

    Animator bansheeAnim;
    AudioSource scareNine;
    bool scarable = true;

	void Start () {
        bansheeAnim = GetComponent<Animator>();
        scareNine = GetComponent<AudioSource>();
        ghostObject.SetActive(false);
        bansheeAnim.SetBool("Just Walk", true);
	}

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
