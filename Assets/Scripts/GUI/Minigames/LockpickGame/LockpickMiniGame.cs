using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for the lockpick minigame
/// </summary>
public class LockpickMiniGame : Minigame
{
    [Header("Target")]
    [SerializeField] private RectTransform targetTransform;
    [SerializeField] private float targetRange;

    [Header("Arrow")]
    [SerializeField] private RectTransform arrowTransform;
    [SerializeField] private float arrowSpeed;
    [SerializeField] private float sideMax;
    [SerializeField] private float arrowMoveCooldown;
    [SerializeField] private float arrowActionCooldown;

    [Header("Locks")]
    [SerializeField] private Image[] imagesLock;
    [Header("Audios")]
    [SerializeField] private AudioClip sfxNormal;
    [SerializeField] private AudioClip sfxWin;

    private bool inCooldown;
    private float startedWaitingAt;
    private int side;
    private int currentLock = -1;
    private float arrowPosition;
    private float targetPosition;

    public bool isPlaying
    {
        get
        {
            return playing;
        }
    }

    /// <summary>
    /// Handles the start of the lockpick minigame
    /// </summary>
    /// <param name="endDialog">Dialog to load afterwards</param>
    public override void StartMiniGame(string endDialog)
    {
        base.StartMiniGame(endDialog);

        targetPosition = Random.Range(-225f, 255f);

        currentLock = -1;
        side = Random.Range(1, 3) == 1 ? -1 : 1;

        foreach (Image image in imagesLock)
        {
            image.color = Color.black;
        }

        arrowPosition = 0;
        arrowTransform.anchoredPosition = new Vector2(arrowPosition, arrowTransform.anchoredPosition.y);

        targetTransform.anchoredPosition = new Vector2(targetPosition, targetTransform.anchoredPosition.y);
    }

    /// <summary>
    /// Handles the lockpick minigame's logic
    /// </summary>
    public override void Update()
    {
        base.Update();


        if (!inCooldown || Time.time - startedWaitingAt >= arrowMoveCooldown)
        {
            arrowPosition = Mathf.Clamp(arrowPosition + Time.deltaTime * arrowSpeed * side, -sideMax, sideMax);

            if (arrowPosition == sideMax || arrowPosition == -sideMax)
            {
                side *= -1;
            }

            arrowTransform.anchoredPosition = new Vector2(arrowPosition, arrowTransform.anchoredPosition.y);
        }

        if (!inCooldown)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                inCooldown = true;
                startedWaitingAt = Time.time;

                if (targetPosition - targetRange <= arrowPosition && arrowPosition <= targetPosition + targetRange)
                {
                    currentLock++;
                    imagesLock[currentLock].color = Color.green;

                    if (currentLock == 2)
                    {
                        AudioManager.instance.PlaySFX(sfxWin);
                        EndMiniGame();
                    }
                    else
                    {
                        AudioManager.instance.PlaySFX(sfxNormal);
                    }
                }
                else
                {
                    AudioManager.instance.PlaySFX(sfxNormal);
                }
            }
        }
        else
        {
            if (Time.time - startedWaitingAt >= arrowActionCooldown)
            {
                inCooldown = false;
            }
        }

    }

    public override void EndMiniGame()
    {
        base.EndMiniGame();
    }

}
