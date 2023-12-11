using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Parent class for all minigames
/// </summary>
public class Minigame : MonoBehaviour
{
    [Header("MiniGame")]
    [SerializeField] protected GameObject root;
    protected string dialogToLoadAtTheEnd;
    protected bool playing;

    /// <summary>
    /// Starts the minigame
    /// </summary>
    /// <param name="endDialog">Dialog to launch after winning the minigame</param>
    public virtual void StartMiniGame(string endDialog)
    {
        dialogToLoadAtTheEnd = endDialog;
        root.SetActive(true);
        playing = true;
    }

    /// <summary>
    /// Base mechanics of a game
    /// </summary>
    public virtual void Update()
    {
        if (GameGUI.instance.inMenu || DialogMaster.instance.inDialog || !playing)
        {
            return;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            dialogToLoadAtTheEnd = "";
            EndMiniGame();
            return;
        }
    }

    /// <summary>
    /// End the minigame and load the dialog afterwards if it exists
    /// </summary>
    public virtual void EndMiniGame()
    {
        root.SetActive(false);
        playing = false;
        if (!string.IsNullOrEmpty(dialogToLoadAtTheEnd))
        {
            DialogMaster.instance.StartDialog(dialogToLoadAtTheEnd);
        }
    }
}
