using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryObjectsManager : MonoBehaviour
{
    public static StoryObjectsManager instance;

    private List<StoryObject> listObjs = new List<StoryObject>();
    void Awake()
    {
        instance = this;
        listObjs = new List<StoryObject>();
    }

    public void RegisterStoryObject(StoryObject obj)
    {
        listObjs.Add(obj);
    }

    public void RefreshAllStoryObjects()
    {
        foreach (StoryObject obj in listObjs)
        {
            obj.RefreshIsVisible();
        }
    }
}
