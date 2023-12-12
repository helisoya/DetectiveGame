using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

/// <summary>
/// The GameManager handles the Savefile as well as other things. It isn't destroyed when changing scenes.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private SaveFile save;

    private Dictionary<string, SaveItem> _saveItems;
    private Coroutine routine_changeMap;

    private Dictionary<string, CaseAdapter> caseAdapters;


    public List<string> save_unlockedBios
    {
        get
        {
            return save.biosUnlocked;
        }
    }

    public List<string> save_unlockedEvidences
    {
        get
        {
            return save.evidencesUnlocked;
        }
    }

    public List<string> save_currentfollowers
    {
        get
        {
            return save.currentFollowers;
        }
    }

    public List<string> save_currentCases
    {
        get
        {
            return save.currentCases;
        }
    }

    public List<string> save_pastCases
    {
        get
        {
            return save.pastCases;
        }
    }

    public string save_currentMap
    {
        get
        {
            return save.currentMap;
        }
    }

    public string save_lastMap
    {
        get
        {
            return save.lastMap;
        }
    }

    public string save_currentWeather
    {
        get
        {
            return save.currentWeather;
        }
        set
        {
            save.currentWeather = value;
        }
    }

    /// <summary>
    /// Gets a specific Save Item's value
    /// </summary>
    /// <param name="key">The item's key</param>
    /// <returns>The item's value</returns>
    public string GetSaveItemValue(string key)
    {
        return _saveItems[key].value;
    }

    /// <summary>
    /// Changes a specfic Save Item's value
    /// </summary>
    /// <param name="key">The item's key</param>
    /// <param name="value">The new item's value</param>
    public void SetSaveItem(string key, string value)
    {
        _saveItems[key].value = value;
    }


    /// <summary>
    /// Checks if a evidence was unlocked 
    /// </summary>
    /// <param name="evidence">The evidence</param>
    /// <returns>Has the evidence been unlocked ?</returns>
    public bool HasUnlockedEvidence(Evidence evidence)
    {
        return caseAdapters[evidence.caseRef.ID].evidenceFound[evidence.ID];
    }


    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            InitAdapters();
            DontDestroyOnLoad(gameObject);
            instance = this;
            ResetSaveFile();
            Save();
        }
    }

    /// <summary>
    /// Resets the SaveFile
    /// </summary>
    void ResetSaveFile()
    {
        save = new SaveFile();
        _saveItems = new Dictionary<string, SaveItem>();
        foreach (SaveItem item in save.items)
        {
            _saveItems.Add(item.id, item);
        }

        Save_UnlockNewBio("ARTHUR");
        RefreshAdapters();
    }

    /// <summary>
    /// Initialize save adapters
    /// </summary>
    void InitAdapters()
    {
        caseAdapters = new Dictionary<string, CaseAdapter>();
        Case[] cases = Resources.LoadAll<Case>("Cases");

        foreach (Case c in cases)
        {
            CaseAdapter adapter = new CaseAdapter();
            foreach (Evidence evidence in c.evidences)
            {
                adapter.evidenceFound.Add(evidence.ID, false);
            }
            caseAdapters.Add(c.ID, adapter);
        }
    }

    /// <summary>
    /// Refresh the Save Adapters
    /// </summary>
    void RefreshAdapters()
    {
        Evidence[] evidences = Resources.LoadAll<Evidence>("Evidences");
        foreach (Evidence evidence in evidences)
        {
            caseAdapters[evidence.caseRef.ID].evidenceFound[evidence.ID] = save.evidencesUnlocked.Contains(evidence.ID);
        }
    }

    /// <summary>
    /// Unlocks a new Biography
    /// </summary>
    /// <param name="newBio">The new biography's ID</param>
    public void Save_UnlockNewBio(string newBio)
    {
        if (!save.biosUnlocked.Contains(newBio))
        {
            save.biosUnlocked.Add(newBio);
        }
    }

    /// <summary>
    /// Marks a case as started
    /// </summary>
    /// <param name="caseID">The case ID</param>
    public void Save_StartNewCase(string caseID)
    {
        if (!save.currentCases.Contains(caseID))
        {
            save.currentCases.Add(caseID);
        }
    }

    /// <summary>
    /// Marks a case as finished
    /// </summary>
    /// <param name="caseID"></param>
    public void Save_EndCase(string caseID)
    {
        if (save.currentCases.Contains(caseID))
        {
            save.currentCases.Remove(caseID);
            save.pastCases.Add(caseID);
        }
    }

    /// <summary>
    /// Unlock a new evidence
    /// </summary>
    /// <param name="ID">The evidence ID</param>
    public void Save_AddEvidence(string ID)
    {
        Evidence evidence = Resources.Load<Evidence>("Evidences/" + ID);

        if (!HasUnlockedEvidence(evidence))
        {
            save.evidencesUnlocked.Add(ID);
            caseAdapters[evidence.caseRef.ID].evidenceFound[ID] = true;
        }
    }

    /// <summary>
    /// Changes the current map (in the save file)
    /// </summary>
    /// <param name="name">The new current map</param>
    public void Save_SetCurrentMap(string name)
    {
        save.lastMap = save.currentMap;
        save.currentMap = name;
    }

    /// <summary>
    /// Adds a follower
    /// </summary>
    /// <param name="name">The follower's name</param>
    public void Save_AddFollower(string name)
    {
        if (!save.currentFollowers.Contains(name))
        {
            save.currentFollowers.Add(name);
        }
    }

    /// <summary>
    /// Removes a follower
    /// </summary>
    /// <param name="name">The follower's name</param>
    public void Save_RemoveFollower(string name)
    {
        save.currentFollowers.Remove(name);
    }

    /// <summary>
    /// Save the game
    /// </summary>
    public void Save()
    {
        FileManager.SaveJSON(FileManager.savPath + "save.sav", save);
    }

    /// <summary>
    /// Loads the game
    /// </summary>
    public void Load()
    {
        if (System.IO.File.Exists(FileManager.savPath + "save.sav"))
        {
            save = FileManager.LoadJSON<SaveFile>(FileManager.savPath + "save.sav");
            _saveItems = new Dictionary<string, SaveItem>();
            foreach (SaveItem item in save.items)
            {
                _saveItems.Add(item.id, item);
            }
            RefreshAdapters();
        }
    }

    /// <summary>
    /// Loads a new map
    /// </summary>
    /// <param name="name">The new map's name</param>
    public void LoadMap(string name)
    {
        if (routine_changeMap != null) return;

        routine_changeMap = StartCoroutine(Routine_ChangeMap(name));
    }

    /// <summary>
    /// Routine for changing maps
    /// </summary>
    /// <param name="name">The new map's name</param>
    /// <returns></returns>
    IEnumerator Routine_ChangeMap(string name)
    {
        Time.timeScale = 1;

        GameGUI.instance.FadeForegroundTo(1, 5);

        yield return new WaitForEndOfFrame();

        while (GameGUI.instance.isFadingForeground)
        {
            yield return new WaitForEndOfFrame();
        }

        Save_SetCurrentMap(name);
        SceneManager.LoadScene(name);

        routine_changeMap = null;
    }

}
