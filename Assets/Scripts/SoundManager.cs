using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;  // Singleton instance

    public AudioSource audioSource;  // Audio source to play sounds
    public AudioSource trailSource;  // Audio source to play sounds

    public AudioClip[] audioClips;  // Array to hold different audio clips

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of SoundManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Optional: keep this object across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to play a sound by name with optional pitch adjustment
    public void PlaySound(string soundName, float pitch)
    {
        Debug.Log(audioSource);

        // Find the audio clip by name
        AudioClip clip = System.Array.Find(audioClips, c => c.name == soundName);

        // Play the clip if found
        if (clip != null)
        {
            // Set the pitch of the audio source
            audioSource.pitch = pitch;

            // Play the sound
            audioSource.PlayOneShot(clip);

            // Reset the pitch to default after playing
            // Important to reset if the pitch is changed frequently
            //audioSource.pitch = 1.0f;
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found.");
        }
    }

    public void PlayTrailSound(string soundName)
    {
        // Find the audio clip by name
        AudioClip clip = System.Array.Find(audioClips, c => c.name == soundName);

        // Play the clip if found
        if (clip != null)
        {

            // Play the sound
            trailSource.PlayOneShot(clip);

            // Reset the pitch to default after playing
            // Important to reset if the pitch is changed frequently
            //audioSource.pitch = 1.0f;
        }
        else
        {
            Debug.LogWarning($"Sound '{soundName}' not found.");
        }
    }
}
