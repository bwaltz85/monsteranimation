using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Reference to the AudioSource component
    public AudioSource audioSource;

    // List of music tracks to cycle through
    public List<AudioClip> musicClips = new List<AudioClip>();

    // Optional delay between tracks
    public float delayBetweenTracks = 0.5f;

    // Keeps track of the current track index
    private int currentTrackIndex = 0;

    void Start()
    {
        // Automatically add an AudioSource component if one is not attached
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        // Start playing if we have at least one music clip
        if (musicClips.Count > 0)
        {
            PlayNextTrack();
        }
    }

    // Plays the next track in the list
    void PlayNextTrack()
    {
        if (musicClips.Count == 0) return;

        // Set the current clip and play it
        audioSource.clip = musicClips[currentTrackIndex];
        audioSource.Play();

        // Update the track index for the next cycle
        currentTrackIndex = (currentTrackIndex + 1) % musicClips.Count;

        // Start coroutine to wait for the current track to finish
        StartCoroutine(WaitForTrackEnd());
    }

    // Coroutine that waits until the current track finishes playing, then plays the next track
    IEnumerator WaitForTrackEnd()
    {
        // Wait until the audio clip finishes playing
        yield return new WaitUntil(() => !audioSource.isPlaying);

        // Wait for an optional delay between tracks
        yield return new WaitForSeconds(delayBetweenTracks);

        // Play the next track in the list
        PlayNextTrack();
    }
}
