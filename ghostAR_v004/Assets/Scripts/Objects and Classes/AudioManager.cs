using UnityEngine;
using UnityEngine.Audio;

using System;

public class AudioManager : MonoBehaviour {

    /// <summary>
    /// The Array of sounds that this Audio Manager can play.
    /// </summary>
    [Tooltip("The Array of sounds that this Audio Manager can play.")]
    public Sound[] sounds;

    private void Awake() {

        foreach (Sound s in sounds) {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    private void Start() {
        Play("Ambience");
    }

    /// <summary>
    /// Plays the sound whose name is given.
    /// </summary>
    /// <param name="name">
    /// (string) The Name of the sound you want to play.
    /// </param>
    public void Play (string name) {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null) {
            Debug.LogError("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }
}
