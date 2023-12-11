using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


/// <summary>
/// Class for the characters menu tab
/// </summary>
public class CharacterMenuTab : PauseMenuTab
{
    [Header("Character Menu")]
    [SerializeField] private Transform buttonsRoot;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private TextMeshProUGUI characterNameText;
    [SerializeField] private TextMeshProUGUI characterDescText;
    [SerializeField] private Image characterImg;
    [SerializeField] private CharacterMenuEntry[] entries;

    private Dictionary<string, CharacterMenuEntry> dicEntries;
    private Dictionary<string, CharacterMenuButton> dicButtons;


    private string currentCharacter = "ARTHUR";

    void Start()
    {
        dicEntries = new Dictionary<string, CharacterMenuEntry>();
        dicButtons = new Dictionary<string, CharacterMenuButton>();
        foreach (CharacterMenuEntry entry in entries)
        {
            dicEntries.Add(entry.ID, entry);

            CharacterMenuButton button = Instantiate(buttonPrefab, buttonsRoot).GetComponent<CharacterMenuButton>();
            button.Init(entry.ID, this);
            dicButtons.Add(entry.ID, button);
        }

    }

    /// <summary>
    /// Refreshes the tab
    /// </summary>
    public override void Refresh()
    {
        base.Refresh();

        foreach (CharacterMenuButton button in dicButtons.Values)
        {
            button.Refresh();
        }
        SelectBio(currentCharacter);
    }


    /// <summary>
    /// Selects a new biography
    /// </summary>
    /// <param name="id">The ID of the biography</param>
    public void SelectBio(string id)
    {
        currentCharacter = id;
        CharacterMenuEntry entry = dicEntries[id];

        characterImg.sprite = entry.characterImg;
        characterDescText.text = entry.characterDesc;
        characterNameText.text = entry.characterName;
    }
}


/// <summary>
/// Class of a biography entry
/// </summary>
[System.Serializable]
public class CharacterMenuEntry
{
    public string ID;
    public string characterName;
    public string characterDesc;
    public Sprite characterImg;
}
