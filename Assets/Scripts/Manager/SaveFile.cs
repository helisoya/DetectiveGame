using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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