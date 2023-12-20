using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaveObjectManager
{
    public static SaveObjectManager instance
    {
        get
        {
            if (_instance == null) _instance = new SaveObjectManager();
            return _instance;
        }
    }

    private static SaveObjectManager _instance;
    private List<SaveObject> saveObjects;

    public SaveObjectManager()
    {
        saveObjects = new List<SaveObject>();
    }

    /// <summary>
    /// Resets the save objects registered
    /// </summary>
    public void ResetSaveObjects()
    {
        saveObjects.Clear();
    }


    /// <summary>
    /// Register a SaveObject
    /// </summary>
    /// <param name="saveObject">The saveObject to register</param>
    public void RegisterSaveObject(SaveObject saveObject)
    {
        saveObjects.Add(saveObject);
    }

    /// <summary>
    /// Unregister a SaveObject
    /// </summary>
    /// <param name="saveObject"></param>
    public void UnRegister(SaveObject saveObject)
    {
        saveObjects.Remove(saveObject);
    }

    public List<SaveStoryObject> GetSaveStoryObjects()
    {
        List<SaveStoryObject> list = new List<SaveStoryObject>();

        foreach (SaveObject obj in saveObjects)
        {
            SaveStoryObject res = obj.Save();
            if (res != null)
            {
                list.Add(res);
            }
        }

        return list;
    }
}
