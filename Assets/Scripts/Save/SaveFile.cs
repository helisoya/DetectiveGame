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

    public Vector3 playerPosition;
    public float playerRotation;
    public List<SaveStoryObject> storyObjects;

    public SaveFile()
    {
        biosUnlocked = new List<string>();
        evidencesUnlocked = new List<string>();
        currentCases = new List<string>();
        pastCases = new List<string>();
        items = new List<SaveItem>
        {
            new SaveItem("STORY", "1"),
            new SaveItem("OBJECTIVE", "0")
        };
        currentMap = "DOCKS";
        lastMap = "NONE";
        currentWeather = "DAY";
        currentFollowers = new List<string>();

        playerRotation = 0f;
        playerPosition = new Vector3();
        storyObjects = new List<SaveStoryObject>();
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


/// <summary>
/// Represents a StoryObject (NPC, ...) whose informations will be saved here
/// </summary>
[System.Serializable]
public class SaveStoryObject
{
    public string objectName;
    public Vector3 objectPosition;
    public float objectRotation;
    public string npcEventName;
    public int currentNpcEventIndex;
    public bool npcHidden;

    public SaveStoryObject(string objectName, Vector3 objectPosition, float objectRotation, bool npcHidden = false, string npcEventName = "", int currentNpcEventIndex = 0)
    {
        this.objectName = objectName;
        this.objectPosition = objectPosition;
        this.objectRotation = objectRotation;
        this.npcEventName = npcEventName;
        this.npcHidden = npcHidden;
        this.currentNpcEventIndex = currentNpcEventIndex;
    }
}