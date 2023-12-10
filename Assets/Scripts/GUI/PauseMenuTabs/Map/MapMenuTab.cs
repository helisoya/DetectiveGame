using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMenuTab : PauseMenuTab
{
    [Header("Map Tab")]
    [SerializeField] private Transform outsideMap;
    [SerializeField] private Transform F1Map;
    [SerializeField] private Transform F2Map;
    [SerializeField] private List<string> outsideScenes;
    [SerializeField] private List<string> F1Scenes;
    [SerializeField] private List<string> F2Scenes;

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
