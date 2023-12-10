using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyManager : MonoBehaviour
{
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


    public static SkyManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ChangeSkybox(System.Enum.Parse<SkyType>(GameManager.instance.save_currentWeather));
    }


    void ChangeValues(SkyData data)
    {
        RenderSettings.skybox = data.skybox;
        sun.transform.eulerAngles = data.sunRotation;
        sun.color = data.sunColor;
        sun.intensity = data.sunIntensity;

        rain.SetActive(data.rain);
    }

    public void ChangeSkybox(SkyType type)
    {
        GameManager.instance.save_currentWeather = type.ToString();
        switch (type)
        {
            case SkyType.DAY:
                ChangeValues(dayData);
                break;
            case SkyType.EVENING:
                ChangeValues(eveningData);
                break;
            case SkyType.NIGHT:
                ChangeValues(nightData);
                break;
            case SkyType.CLOUD:
                ChangeValues(cloudData);
                break;
            case SkyType.STORM:
                ChangeValues(stormData);
                // Rain
                break;
        }
    }

}
