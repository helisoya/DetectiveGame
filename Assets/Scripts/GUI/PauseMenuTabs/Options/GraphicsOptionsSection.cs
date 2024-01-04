using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GraphicsOptionsSection : OptionsSection
{
    [Header("Graphics")]
    [SerializeField] private Toggle toggleFullscreen;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    private Resolution[] resolutions;




    /// <summary>
    /// Loads the settings
    /// </summary>
    public override void Load()
    {
        resolutions = Screen.resolutions;

        int currentIndex = -1;
        List<string> resLabels = new List<string>();
        GlobalSave save = GameManager.instance.globalSave;
        Resolution resolution;

        for (int i = 0; i < resolutions.Length; i++)
        {
            resolution = resolutions[i];
            resLabels.Add(resolution.width + "x" + resolution.height + " (" + resolution.refreshRate + ")");

            if (currentIndex == -1 && resolution.width == save.resolutionW &&
            resolution.height == save.resolutionH && resolution.refreshRate == save.refreshRate)
            {
                currentIndex = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resLabels);

        toggleFullscreen.SetIsOnWithoutNotify(save.fullscreen);
        resolutionDropdown.SetValueWithoutNotify(currentIndex != -1 ? currentIndex : 0);

        qualityDropdown.SetValueWithoutNotify(save.quality);
    }

    /// <summary>
    /// Saves the settings
    /// </summary>
    public override void Save()
    {
        GlobalSave save = GameManager.instance.globalSave;
        save.fullscreen = toggleFullscreen.isOn;

        Resolution res = resolutions[resolutionDropdown.value];
        save.resolutionH = res.height;
        save.resolutionW = res.width;
        save.refreshRate = res.refreshRate;

        Screen.SetResolution(res.width, res.height, save.fullscreen, res.refreshRate);

        save.quality = qualityDropdown.value;
        QualitySettings.SetQualityLevel(save.quality);
    }
}
