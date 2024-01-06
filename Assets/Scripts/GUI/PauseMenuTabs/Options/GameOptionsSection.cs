using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOptionsSection : OptionsSection
{
    [Header("Game")]
    [SerializeField] private Slider sliderTypeWritter;


    /// <summary>
    /// Loads the settings
    /// </summary>
    public override void Load()
    {
        GlobalSave save = GameManager.instance.globalSave;
        sliderTypeWritter.SetValueWithoutNotify(save.typewritterSpeed);
    }

    /// <summary>
    /// Saves the settings
    /// </summary>
    public override void Save()
    {
        GlobalSave save = GameManager.instance.globalSave;
        save.typewritterSpeed = sliderTypeWritter.value;
    }
}
