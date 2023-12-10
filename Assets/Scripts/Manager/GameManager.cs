using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

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

    public string GetSaveItemValue(string key)
    {
        return _saveItems[key].value;
    }

    public void SetSaveItem(string key, string value)
    {
        _saveItems[key].value = value;
    }


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

    void RefreshAdapters()
    {
        Evidence[] evidences = Resources.LoadAll<Evidence>("Evidences");
        foreach (Evidence evidence in evidences)
        {
            caseAdapters[evidence.caseRef.ID].evidenceFound[evidence.ID] = save.evidencesUnlocked.Contains(evidence.ID);
        }
    }

    public void Save_UnlockNewBio(string newBio)
    {
        if (!save.biosUnlocked.Contains(newBio))
        {
            save.biosUnlocked.Add(newBio);
        }
    }

    public void Save_StartNewCase(string caseID)
    {
        if (!save.currentCases.Contains(caseID))
        {
            save.currentCases.Add(caseID);
        }
    }

    public void Save_EndCase(string caseID)
    {
        if (save.currentCases.Contains(caseID))
        {
            save.currentCases.Remove(caseID);
            save.pastCases.Add(caseID);
        }
    }

    public void Save_AddEvidence(string ID)
    {
        Evidence evidence = Resources.Load<Evidence>("Evidences/" + ID);

        if (!HasUnlockedEvidence(evidence))
        {
            save.evidencesUnlocked.Add(ID);
            caseAdapters[evidence.caseRef.ID].evidenceFound[ID] = true;
        }
    }


    public void Save_SetCurrentMap(string name)
    {
        save.lastMap = save.currentMap;
        save.currentMap = name;
    }

    public void Save_AddFollower(string name)
    {
        if (!save.currentFollowers.Contains(name))
        {
            save.currentFollowers.Add(name);
        }
    }

    public void Save_RemoveFollower(string name)
    {
        save.currentFollowers.Remove(name);
    }


    public void Save()
    {
        FileManager.SaveJSON(FileManager.savPath + "save.sav", save);
    }

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


    public void LoadMap(string name)
    {
        if (routine_changeMap != null) return;

        routine_changeMap = StartCoroutine(Routine_ChangeMap(name));
    }

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
