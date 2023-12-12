using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Class containing all the informations that are saved by the game
/// </summary>
[System.Serializable]
public class SaveFile
{
    public List<string> biosUnlocked;
    public List<string> evidencesUnlocked;
    public List<string> currentCases;
    public List<string> pastCases;
    public List<SaveItem> items;
    public string currentMap;
    public string lastMap;
    public string currentWeather;
    public List<string> currentFollowers;

    public SaveFile()
    {
        biosUnlocked = new List<string>();
        evidencesUnlocked = new List<string>();
        currentCases = new List<string>();
        pastCases = new List<string>();
        items = new List<SaveItem>();
        items.Add(new SaveItem("STORY", "1"));
        currentMap = "DOCKS";
        lastMap = "NONE";
        currentWeather = "DAY";
        currentFollowers = new List<string>();
    }
}


/// <summary>
/// An item that is saved (The story point for instance)
/// </summary>
[System.Serializable]
public class SaveItem
{
    public string id;
    public string value;

    public SaveItem(string id, string defaultValue)
    {
        this.id = id;
        value = defaultValue;
    }
}