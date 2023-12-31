using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [Header("Sources")]
    [SerializeField] private AudioSource sourceBGM;
    [SerializeField] private AudioSource sourceSFX;
    [SerializeField] private AudioMixer audioMixer;

    public static AudioManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    /// <summary>
    /// Plays a Background Music
    /// </summary>
    /// <param name="clip">The audio clip</param>
    /// <param name="loop">Loop the audio ? (default = true)</param>
    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (sourceBGM.isPlaying)
        {
            sourceBGM.Stop();
        }

        sourceBGM.clip = clip;
        sourceBGM.loop = loop;
        sourceBGM.Play();
    }

    /// <summary>
    /// Plays a Sound Effect
    /// </summary>
    /// <param name="clip">The audio clip</param>
    public void PlaySFX(AudioClip clip)
    {
        sourceSFX.PlayOneShot(clip);
    }


    /// <summary>
    /// Changes the volume of a group (SFX,BGM,Master)
    /// </summary>
    /// <param name="volumeName">The group's name</param>
    /// <param name="value">The new volume value</param>
    public void ChangeVolume(string volumeName, float value)
    {
        audioMixer.SetFloat(volumeName, value);
    }
}
