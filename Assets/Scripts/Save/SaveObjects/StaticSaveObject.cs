using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class for saving static objects
/// </summary>
public class StaticSaveObject : SaveObject
{
    [Header("Static")]
    [SerializeField] private StoryObject storyObject;

    /// <summary>
    /// Loads the state of the Object
    /// </summary>
    public override void Load()
    {
        SaveStoryObject obj = GameManager.instance.GetSaveStoryObject(saveName);
        if (obj == null)
        {
            storyObject?.ForceSetValueTo(false);
        }
        else
        {
            storyObject?.ForceSetValueTo(true);
            transform.position = obj.objectPosition;
        }
        storyObject?.RefreshIsVisible(false);
    }

    /// <summary>
    /// Generates the SaveStoryObject for this Object
    /// </summary>
    /// <returns>The SaveStoryObject of the Object</returns>
    public override SaveStoryObject Save()
    {
        if (storyObject != null && !storyObject.GetCanBeEnabled()) return null;

        return new SaveStoryObject(
            saveName,
            transform.position,
            transform.eulerAngles.y
        );
    }
}
