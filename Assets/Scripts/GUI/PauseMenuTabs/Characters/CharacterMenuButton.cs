using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for the Character button in the Characters menu tab
/// </summary>
public class CharacterMenuButton : MonoBehaviour
{

    [SerializeField] private Image buttonSprite;
    [SerializeField] private Button button;
    private string entryID;
    private CharacterMenuTab tab;
    private Sprite unlockedSprite;
    private Sprite lockedSprite;

    /// <summary>
    /// Initialize the button
    /// </summary>
    /// <param name="id">The id of the biography</param>
    /// <param name="t">The parent tab</param>
    public void Init(string id, CharacterMenuTab t)
    {
        entryID = id;
        tab = t;

        unlockedSprite = Resources.Load<Sprite>("CharacterFaces/" + entryID + "_unlocked");
        lockedSprite = Resources.Load<Sprite>("CharacterFaces/" + entryID + "_locked");

        Refresh();
    }

    /// <summary>
    /// Refreshes the button's informations
    /// </summary>
    public void Refresh()
    {
        bool hasUnlockedBio = GameManager.instance.save_unlockedBios.Contains(entryID);
        button.interactable = hasUnlockedBio;
        buttonSprite.sprite = hasUnlockedBio ? unlockedSprite : lockedSprite;
    }

    /// <summary>
    /// Handles the button click
    /// </summary>
    public void Event_Click()
    {
        GameGUI.instance.PlayButtonSFX();
        tab.SelectBio(entryID);
    }
}
