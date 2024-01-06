using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuTab : PauseMenuTab
{
    [SerializeField] private bool onMainMenu = false;
    [SerializeField] private OptionsSection[] sections;

    private OptionsSection currentSection;



    /// <summary>
    /// Refreshes the options tab
    /// </summary>
    public override void Refresh()
    {
        base.Refresh();

        foreach (OptionsSection section in sections)
        {
            section.Load();
        }
    }

    /// <summary>
    /// Handles the click to change the section
    /// </summary>
    /// <param name="section">The new section</param>
    public void Click_ChangeSection(OptionsSection section)
    {
        PlayButtonSFX();
        if (currentSection != null) currentSection.Close();
        section.Open();
        currentSection = section;
    }

    /// <summary>
    /// Applies settings
    /// </summary>
    public void Click_Apply()
    {
        PlayButtonSFX();
        foreach (OptionsSection section in sections)
        {
            section.Save();
        }
        GameManager.instance.SaveGlobal();
    }

    /// <summary>
    /// Plays the button click SFX, depending on the parent GUI
    /// </summary>
    void PlayButtonSFX()
    {
        if (onMainMenu)
        {
            MainMenuManager.instance.PlayButtonSFX();
        }
        else
        {
            GameGUI.instance.PlayButtonSFX();
        }
    }
}
