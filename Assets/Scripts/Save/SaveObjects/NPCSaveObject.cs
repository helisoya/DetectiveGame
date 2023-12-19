using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class NPCSaveObject : SaveObject
{
    [Header("NPC")]
    [SerializeField] private NPC npc;
    [SerializeField] private StoryObject storyObject;

    /// <summary>
    /// Loads the state of the NPC
    /// </summary>
    public override void Load()
    {
        SaveStoryObject obj = GameManager.instance.GetSaveStoryObject(saveName);
        if (obj == null)
        {
            storyObject.ForceSetValueTo(false);
            npc.SetHidden(true);

        }
        else
        {
            storyObject.ForceSetValueTo(true);
            npc.RefreshFromSaveStoryObject(obj);
        }
    }

    /// <summary>
    /// Generates the SaveStoryObject for this NPC
    /// </summary>
    /// <returns>The SaveStoryObject of the NPC</returns>
    public override SaveStoryObject Save()
    {
        if (storyObject != null && !storyObject.GetCanBeEnabled()) return null;

        return new SaveStoryObject(
            saveName,
            npc.position,
            npc.rotation,
            npc.currentEvent,
            npc.currentEventIndex
        );
    }
}
