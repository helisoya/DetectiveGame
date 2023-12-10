using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : InteractableObject
{
    [Header("NPC Interaction")]
    [SerializeField] private string dialogToLoad;
    [SerializeField] private NPC npc;

    public void ChangeDialogToLoad(string newDialog)
    {
        dialogToLoad = newDialog;
    }

    public override void OnInteraction()
    {
        if (!npc.isProcessingEvent)
        {
            DialogMaster.instance.StartDialog(dialogToLoad);
        }

    }

    public override void OnMouseEnter()
    {
        if (!npc.isProcessingEvent)
        {
            base.OnMouseEnter();
        }

    }

    public override void OnMouseExit()
    {
        if (!npc.isProcessingEvent)
        {
            base.OnMouseExit();
        }

    }
}
