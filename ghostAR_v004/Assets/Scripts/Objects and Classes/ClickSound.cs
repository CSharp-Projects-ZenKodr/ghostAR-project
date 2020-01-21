using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ClickSound : MonoBehaviour {
    /// <summary>
    /// The sound that plays when clicked.
    /// </summary>
    [Tooltip("The sound that plays when clicked.")]
    public AudioClip sound;

    /// <summary>
    /// The Button component attached to this GameObject.
    /// </summary>
    private Button button { get { return GetComponent<Button>(); } }
    /// <summary>
    /// The Audio source attached to this GameObject.
    /// </summary>
    private AudioSource source { get { return GetComponent<AudioSource>(); } }

	void Start () {
        gameObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.playOnAwake = false;

        button.onClick.AddListener(() => PlaySound());
	}
    	
    void PlaySound() {
        source.PlayOneShot(sound);
    }
}