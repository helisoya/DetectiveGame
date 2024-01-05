using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Class for the map menu tab
/// </summary>
public class MapMenuTab : PauseMenuTab
{
    [Header("Map Tab")]
    [SerializeField] private Transform outsideMap;
    [SerializeField] private Transform F1Map;
    [SerializeField] private Transform F2Map;
    [SerializeField] private List<string> outsideScenes;
    [SerializeField] private List<string> F1Scenes;
    [SerializeField] private List<string> F2Scenes;

    [Header("Objective")]
    [SerializeField] private string[] objectiveLabels;
    [SerializeField] private TextMeshProUGUI objectiveText;

    void Start()
    {
        string mapName = GameManager.instance.save_currentMap;
        if (outsideScenes.Contains(mapName))
        {
            SearchForIconInTransform(outsideMap, mapName);
        }
        else if (F1Scenes.Contains(mapName))
        {
            SearchForIconInTransform(F1Map, mapName);
        }
        else
        {
            SearchForIconInTransform(F2Map, mapName);
        }
    }

    public override void Refresh()
    {
        base.Refresh();
        objectiveText.text = objectiveLabels[int.Parse(GameManager.instance.GetSaveItemValue("OBJECTIVE"))];
    }

    /// <summary>
    /// Search for a child of a transform, and enables it
    /// </summary>
    /// <param name="transform">The parent</param>
    /// <param name="name">The name of the researched child</param>
    void SearchForIconInTransform(Transform transform, string name)
    {
        transform.gameObject.SetActive(true);

        foreach (Transform child in transform)
        {
            if (child.name.Equals(name))
            {
                child.gameObject.SetActive(true);
                return;
            }
        }
    }
}
