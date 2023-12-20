using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent class for all SaveObjects
/// </summary>
public class SaveObject : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] protected string saveName;


    void Start()
    {
        Init();
    }

    /// <summary>
    /// Initialize the SaveObject
    /// </summary>
    protected virtual void Init()
    {
        SaveObjectManager.instance.RegisterSaveObject(this);
        if (GameManager.instance.wasLoaded)
        {
            Load();
        }
        else
        {
            Default();
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

    /// <summary>
    /// Default behaviour when not loading
    /// </summary>
    public virtual void Default()
    {

    }
}
