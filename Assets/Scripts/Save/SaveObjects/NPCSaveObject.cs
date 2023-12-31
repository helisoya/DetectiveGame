using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/// <summary>
/// Class for saving NPCs
/// </summary>
public class NPCSaveObject : SaveObject
{
    [Header("NPC")]
    [SerializeField] private NPC npc;
    [SerializeField] private StoryObject storyObject;
    [SerializeField] private bool ignoreForLoad = false;
    [SerializeField] private string followerRequiredForIgnoring;


    /// <summary>
    /// Initialize the NPCSaveObject
    /// </summary>
    protected override void Init()
    {
        base.Init();
        npc.Init();
    }

    /// <summary>
    /// Default Behaviour for the NPCSaveObject
    /// </summary>
    public override void Default()
    {
        storyObject?.RefreshIsVisible();
    }


    bool CanIgnoreFollower()
    {
        return string.IsNullOrEmpty(followerRequiredForIgnoring) || GameManager.instance.HasFollower(followerRequiredForIgnoring);
    }

    /// <summary>
    /// Loads the state of the NPC
    /// </summary>
    public override void Load()
    {
        if (ignoreForLoad && CanIgnoreFollower())
        {
            SaveObjectManager.instance.UnRegister(this);
            Destroy(gameObject);
            return;
        }

        SaveStoryObject obj = GameManager.instance.GetSaveStoryObject(saveName);
        if (obj == null && storyObject != null)
        {
            storyObject.ForceSetValueTo(false);
        }
        else
        {
            storyObject?.ForceSetValueTo(true);
            npc.RefreshFromSaveStoryObject(obj);
        }
        storyObject?.RefreshIsVisible(false);
    }

    /// <summary>
    /// Generates the SaveStoryObject for this NPC
    /// </summary>
    /// <returns>The SaveStoryObject of the NPC</returns>
    public override SaveStoryObject Save()
    {
        if (storyObject != null && !storyObject.GetCanBeEnabled() || npc == null) return null;

        if (npc.isProcessingEvent)
        {
            npc.StopEvent();
        }

        return new SaveStoryObject(
            saveName,
            npc.position,
            npc.rotation,
            npc.hidden,
            npc.currentEvent,
            npc.currentEventIndex
        );
    }
}
