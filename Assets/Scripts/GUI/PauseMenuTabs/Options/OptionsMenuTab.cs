using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenuTab : PauseMenuTab
{
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
        GameGUI.instance.PlayButtonSFX();
        if (currentSection != null) currentSection.Close();
        section.Open();
        currentSection = section;
    }

    /// <summary>
    /// Applies settings
    /// </summary>
    public void Click_Apply()
    {
        GameGUI.instance.PlayButtonSFX();
        foreach (OptionsSection section in sections)
        {
            section.Save();
        }
        GameManager.instance.SaveGlobal();
    }
}
