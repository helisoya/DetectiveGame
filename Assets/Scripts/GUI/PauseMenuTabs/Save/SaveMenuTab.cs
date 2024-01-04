using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class for the Save Menu tab. (Handles saving, loading and going to the main menu)
/// </summary>
public class SaveMenuTab : PauseMenuTab
{
    [SerializeField] private Button saveButton;
    [SerializeField] private Button loadButton;

    /// <summary>
    /// Refreshes the tab
    /// </summary>
    public override void Refresh()
    {
        base.Refresh();
        loadButton.interactable = GameManager.instance.saveFileExists;
        saveButton.interactable = !DialogMaster.instance.inDialog;
    }

    /// <summary>
    /// Handles the click to save the game
    /// </summary>
    public void Click_Save()
    {
        GameGUI.instance.PlayButtonSFX();
        GameManager.instance.Save();
        GameGUI.instance.ClosePauseMenu();
    }

    /// <summary>
    /// Handles the click to load the game
    /// </summary>
    public void Click_Load()
    {
        GameGUI.instance.PlayButtonSFX();
        GameManager.instance.Load();
    }

    /// <summary>
    /// Handles the click to the title screen
    /// </summary>
    public void Click_MainMenu()
    {
        GameGUI.instance.PlayButtonSFX();
        GameManager.instance.LoadMap("MainMenu");
    }

}
