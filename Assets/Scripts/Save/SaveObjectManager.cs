using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SaveObjectManager : MonoBehaviour
{
    public static SaveObjectManager instance;
    private List<SaveObject> saveObjects;

    void Awake()
    {
        instance = this;
        saveObjects = new List<SaveObject>();
    }

    /// <summary>
    /// Register a SaveObject
    /// </summary>
    /// <param name="saveObject">The saveObject to register</param>
    public void RegisterSaveObject(SaveObject saveObject)
    {
        saveObjects.Add(saveObject);
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
