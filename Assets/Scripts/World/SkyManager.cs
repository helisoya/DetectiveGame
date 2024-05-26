using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the weather and the day/night cycle.
/// </summary>
public class SkyManager : MonoBehaviour
{
    /// <summary>
    /// Represents the different types of sky possible
    /// </summary>
    public enum SkyType
    {
        DAY,
        EVENING,
        NIGHT,
        CLOUD,
        STORM
    }

    [Header("Components")]
    [SerializeField] private Light sun;
    [SerializeField] private GameObject rain;

    [Header("Data")]
    [SerializeField] private SkyData dayData;
    [SerializeField] private SkyData eveningData;
    [SerializeField] private SkyData nightData;
    [SerializeField] private SkyData cloudData;
    [SerializeField] private SkyData stormData;
    [SerializeField] private bool lanternShouldBeUsed;
    [SerializeField] private bool ignoreWeatherForMap;


    public static SkyManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        if (!ignoreWeatherForMap)
        {
            ChangeSkybox(System.Enum.Parse<SkyType>(GameManager.instance.save_currentWeather));
        }

        PlayerMovements.instance.SetUsingLantern(lanternShouldBeUsed);
    }

    /// <summary>
    /// Changes the skybox
    /// </summary>
    /// <param name="type">The type of sky</param>
    public void ChangeSkybox(SkyType type)
    {
        if (ignoreWeatherForMap) return;

        SkyData data = Resources.Load<SkyData>("SkyDatas/" + type.ToString());
        GameManager.instance.save_currentWeather = type.ToString();


        RenderSettings.skybox = data.skybox;
        sun.transform.eulerAngles = data.sunRotation;
        sun.color = data.sunColor;
        sun.intensity = data.sunIntensity;

        rain.SetActive(data.rain);
    }

}
