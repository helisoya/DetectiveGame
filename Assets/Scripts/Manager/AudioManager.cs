using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Sources")]
    [SerializeField] private AudioSource sourceBGM;
    [SerializeField] private AudioSource sourceSFX;

    public static AudioManager instance;

    void Awake()
    {
        instance = this;
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
}
