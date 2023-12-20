using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Stores all the StoryObjects and refreshes them if needed
/// </summary>
public class StoryObjectsManager : MonoBehaviour
{
    public static StoryObjectsManager instance;

    private List<StoryObject> listObjs = new List<StoryObject>();
    void Awake()
    {
        instance = this;
        listObjs = new List<StoryObject>();
    }

    /// <summary>
    /// Registers a StoryObject
    /// </summary>
    /// <param name="obj">The storyObject</param>
    public void RegisterStoryObject(StoryObject obj)
    {
        listObjs.Add(obj);
    }

    /// <summary>
    /// Refreshes all story objects
    /// </summary>
    public void RefreshAllStoryObjects()
    {
        foreach (StoryObject obj in listObjs)
        {
            obj.RefreshIsVisible();
        }
        PlayerDetectiveVision.instance.FindRenderers();
    }
}
