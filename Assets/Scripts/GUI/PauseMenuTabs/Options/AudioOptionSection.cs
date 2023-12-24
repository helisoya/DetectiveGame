using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioOptionSection : OptionsSection
{
    [Header("Audio")]
    [SerializeField] private Slider sliderMaster;
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private Slider sliderBGM;


    /// <summary>
    /// Loads the settings
    /// </summary>
    public override void Load()
    {
        GlobalSave save = GameManager.instance.globalSave;
        sliderMaster.SetValueWithoutNotify(save.volumeMaster);
        sliderSFX.SetValueWithoutNotify(save.volumeSFX);
        sliderBGM.SetValueWithoutNotify(save.volumeBGM);
    }

    /// <summary>
    /// Saves the settings
    /// </summary>
    public override void Save()
    {
        GlobalSave save = GameManager.instance.globalSave;
        save.volumeMaster = sliderMaster.value;
        save.volumeBGM = sliderBGM.value;
        save.volumeSFX = sliderSFX.value;

        AudioManager.instance.ChangeVolume("Master", sliderMaster.value);
        AudioManager.instance.ChangeVolume("SFX", sliderSFX.value);
        AudioManager.instance.ChangeVolume("BGM", sliderBGM.value);
    }
}