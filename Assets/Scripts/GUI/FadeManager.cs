using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class used to fade an image
/// </summary>
public class FadeManager : MonoBehaviour
{
    [SerializeField] protected Image fadeImg;
    [SerializeField] protected bool fadeAtStart;
    protected Coroutine fading;

    public bool isFading
    {
        get
        {
            return fading != null;
        }
    }

    void Start()
    {
        if (fadeAtStart)
        {
            FadeTo(0, 5);
        }
    }

    /// <summary>
    /// Starts fading the image
    /// </summary>
    /// <param name="to">Target Alpha</param>
    /// <param name="speed">Fade speed</param>
    public virtual void FadeTo(float to, float speed)
    {
        if (fading != null)
        {
            StopCoroutine(fading);
        }
        fading = StartCoroutine(Routine_Fade(to, speed));
    }

    /// <summary>
    /// Routine for the fading
    /// </summary>
    /// <param name="to">Target alpha</param>
    /// <param name="speed">Fade speed</param>
    /// <returns></returns>
    protected virtual IEnumerator Routine_Fade(float to, float speed)
    {
        Color col = fadeImg.color;
        float a = col.a;
        int side = to < a ? -1 : 1;

        while (a != to)
        {
            a = Mathf.Clamp(
                a + side * speed * Time.deltaTime,
                side == 1 ? 0 : to,
                side == 1 ? to : 1
                );
            col.a = a;
            fadeImg.color = col;
            yield return new WaitForEndOfFrame();
        }
        fading = null;
    }
}
