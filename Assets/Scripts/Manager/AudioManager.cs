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
    [SerializeField] private GameObject sfxPrefab;

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
    /// Plays a 3D sound effect
    /// </summary>
    /// <param name="clip">The audio clip</param>
    /// <param name="position">The position of the sound effect</param>
    /// <param name="parent">The parent of the sound effect</param>
    public void Play3DSFX(AudioClip clip, Vector3 position, Transform parent)
    {
        GameObject obj = Instantiate(sfxPrefab, parent);
        obj.name = "3D SFX - " + clip.name;
        obj.transform.position = position;

        AudioSource source = obj.GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();

        Destroy(obj, clip.length);
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
