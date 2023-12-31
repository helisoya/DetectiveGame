using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class representing the audio source for the footstep
/// </summary>
public class FootstepSource : MonoBehaviour
{
    [SerializeField] private AudioSource source;


    /// <summary>
    /// Changes the current footstep clip
    /// </summary>
    /// <param name="newClip">The new footstep clip</param>
    public void ChangeClip(AudioClip newClip)
    {
        source.clip = newClip;
    }

    /// <summary>
    /// Plays the footstep sound effect
    /// </summary>
    public void Play()
    {
        source.Play();
    }

    /// <summary>
    /// Changes the pitch of the Audio Source
    /// </summary>
    /// <param name="pitch">The new pitch</param>
    public void SetPitch(float pitch)
    {
        source.pitch = pitch;
    }
}
