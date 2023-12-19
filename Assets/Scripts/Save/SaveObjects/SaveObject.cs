using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent class for all 
/// </summary>
public class SaveObject : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] protected string saveName;


    void Start()
    {
        SaveObjectManager.instance.RegisterSaveObject(this);
        if (GameManager.instance.wasLoaded)
        {
            Load();
        }
    }

    /// <summary>
    /// Generates the SaveStoryObject for this object
    /// </summary>
    /// <returns>The SaveStoryObject</returns>
    public virtual SaveStoryObject Save()
    {
        return null;
    }

    /// <summary>
    /// Loads the state of the SaveObject
    /// </summary>
    public virtual void Load()
    {
    }
}
