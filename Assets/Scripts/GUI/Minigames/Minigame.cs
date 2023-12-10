using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour
{
    [Header("MiniGame")]
    [SerializeField] protected GameObject root;
    protected string dialogToLoadAtTheEnd;
    protected bool playing;


    public virtual void StartMiniGame(string endDialog)
    {
        dialogToLoadAtTheEnd = endDialog;
        root.SetActive(true);
        playing = true;
    }

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
