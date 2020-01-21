using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound {
    /// <summary>
    /// The Name of the sound.
    /// </summary>
    public string name;
    /// <summary>
    /// The sound itself.
    /// </summary>
    public AudioClip clip;

    /// <summary>
    /// The volume the sound should play as.
    /// </summary>
    [Range(0f, 1f)]
    public float volume;
    /// <summary>
    /// The pitch the sound will play at.
    /// </summary>
    [Range(.1f, 3f)]
    public float pitch;

    /// <summary>
    /// Return true if the sound should loop.
    /// </summary>
    public bool loop;

    /// <summary>
    /// The AudioSource that will play the sound.
    /// </summary>
    public AudioSource source { get; set; }
}
