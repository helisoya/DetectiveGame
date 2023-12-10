using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenuButton : MonoBehaviour
{

    [SerializeField] private Image buttonSprite;
    [SerializeField] private Button button;
    private string entryID;
    private CharacterMenuTab tab;
    private Sprite unlockedSprite;
    private Sprite lockedSprite;

    public void Init(string id, CharacterMenuTab t)
    {
        entryID = id;
        tab = t;

        unlockedSprite = Resources.Load<Sprite>("CharacterFaces/" + entryID + "_unlocked");
        lockedSprite = Resources.Load<Sprite>("CharacterFaces/" + entryID + "_locked");

        Refresh();
    }

    public void Refresh()
    {
        bool hasUnlockedBio = GameManager.instance.save_unlockedBios.Contains(entryID);
        button.interactable = hasUnlockedBio;
        buttonSprite.sprite = hasUnlockedBio ? unlockedSprite : lockedSprite;
    }

    public void Event_Click()
    {
        tab.SelectBio(entryID);
    }
}
