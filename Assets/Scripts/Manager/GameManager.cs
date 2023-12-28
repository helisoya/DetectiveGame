using System;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
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
    private bool loading = false;
    private Dictionary<string, SaveStoryObject> saveStoryObjs;
    public GlobalSave globalSave;

    public bool wasLoaded
    {
        get
        {
            return loading;
        }
    }


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

    public Vector3 save_playerPosition
    {
        get
        {
            return save.playerPosition;
        }
    }

    public float save_playerRotation
    {
        get
        {
            return save.playerRotation;
        }
    }

    public bool saveFileExists
    {
        get
        {
            return System.IO.File.Exists(FileManager.savPath + "save.sav");
        }
    }


    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            LoadGlobal();
            InitAdapters();
            DontDestroyOnLoad(gameObject);
            instance = this;
            ResetSaveFile();
        }
    }

    /// <summary>
    /// Starts a new game (from the main menu only)
    /// </summary>
    public void NewGame()
    {
        InitAdapters();
        ResetSaveFile();
        LoadMap("DOCKS", true);
    }


    /// <summary>
    /// Finds the Saved Story Object with the given name, if it exists.
    /// </summary>
    /// <param name="objectName">The name of the storyObject</param>
    /// <returns>The SaveStoryObject, if found</returns>
    public SaveStoryObject GetSaveStoryObject(string objectName)
    {
        if (saveStoryObjs.ContainsKey(objectName))
        {
            return saveStoryObjs[objectName];
        }
        return null;
    }

    /// <summary>
    /// Checks if the Follower is enabled
    /// </summary>
    /// <param name="follower">The follower's ID</param>
    /// <returns>Is the follower active ?</returns>
    public bool HasFollower(string follower)
    {
        return save.currentFollowers.Contains(follower);
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
    /// Saves the global save
    /// </summary>
    public void SaveGlobal()
    {
        FileManager.SaveJSON(FileManager.savPath + "global.sav", globalSave);
    }

    /// <summary>
    /// Loads the global and creates a new profile if it doesn't exists
    /// </summary>
    public void LoadGlobal()
    {
        if (System.IO.File.Exists(FileManager.savPath + "global.sav"))
        {
            globalSave = FileManager.LoadJSON<GlobalSave>(FileManager.savPath + "global.sav");

            AudioManager.instance.ChangeVolume("Master", globalSave.volumeMaster);
            AudioManager.instance.ChangeVolume("SFX", globalSave.volumeSFX);
            AudioManager.instance.ChangeVolume("BGM", globalSave.volumeBGM);

            Screen.SetResolution(globalSave.resolutionW, globalSave.resolutionH, globalSave.fullscreen, globalSave.refreshRate);
        }
        else
        {
            globalSave = new GlobalSave(
                Screen.currentResolution.width,
                Screen.currentResolution.height,
                Screen.currentResolution.refreshRate,
                Screen.fullScreen
            );
            SaveGlobal();
        }
    }

    /// <summary>
    /// Save the game
    /// </summary>
    public void Save()
    {
        save.playerPosition = PlayerMovements.instance.position;
        save.playerRotation = PlayerCameraManager.instance.rotation;

        save.storyObjects.Clear();
        save.storyObjects = SaveObjectManager.instance.GetSaveStoryObjects();

        FileManager.SaveJSON(FileManager.savPath + "save.sav", save);
    }

    /// <summary>
    /// Load the game
    /// </summary>
    /// <param name="fromMainMenu">Is the game loading from the Main Menu ?</param>
    public void Load(bool fromMainMenu = false)
    {
        if (saveFileExists)
        {
            save = FileManager.LoadJSON<SaveFile>(FileManager.savPath + "save.sav");
            if (save == null) return;

            _saveItems = new Dictionary<string, SaveItem>();
            foreach (SaveItem item in save.items)
            {
                _saveItems.Add(item.id, item);
            }
            RefreshAdapters();

            saveStoryObjs = new Dictionary<string, SaveStoryObject>();
            foreach (SaveStoryObject saveStoryObject in save.storyObjects)
            {
                saveStoryObjs.Add(saveStoryObject.objectName, saveStoryObject);
            }

            loading = true;
            LoadMap(save_currentMap, fromMainMenu);
        }
    }

    /// <summary>
    /// Loads a new map
    /// </summary>
    /// <param name="name">The new map's name</param>
    /// <param name="fromMainMenu">Is the game loading from the Main Menu ?</param>
    public void LoadMap(string name, bool fromMainMenu = false)
    {
        if (routine_changeMap != null) return;

        routine_changeMap = StartCoroutine(Routine_ChangeMap(name, fromMainMenu));
    }

    /// <summary>
    /// Routine for changing maps
    /// </summary>
    /// <param name="name">The new map's name</param>
    /// <param name="fromMainMenu">Is the game loading from the Main Menu ?</param>
    /// <returns></returns>
    IEnumerator Routine_ChangeMap(string name, bool fromMainMenu)
    {
        Time.timeScale = 1;

        if (fromMainMenu)
        {
            MainMenuManager.instance.FadeTo(1, 5);
            yield return new WaitForEndOfFrame();
            while (MainMenuManager.instance.isFading)
            {
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            GameGUI.instance.FadeForegroundTo(1, 5);
            yield return new WaitForEndOfFrame();
            while (GameGUI.instance.isFadingForeground)
            {
                yield return new WaitForEndOfFrame();
            }
        }

        if (SaveObjectManager.instance != null)
        {
            SaveObjectManager.instance.ResetSaveObjects();
        }

        Save_SetCurrentMap(name);
        SceneManager.LoadScene(name);

        if (loading)
        {
            for (int i = 0; i < 5; i++)
            {
                yield return new WaitForEndOfFrame();
            }

            loading = false;
        }

        routine_changeMap = null;
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            print("Saving...");
            Save();

        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            print("Loading...");
            Load();
        }
    }
}
