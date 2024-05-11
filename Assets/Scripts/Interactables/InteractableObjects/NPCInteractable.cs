using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interactable object that represents a NPC
/// </summary>
public class NPCInteractable : InteractableObject
{
    [Header("NPC Interaction")]
    [SerializeField] private string dialogToLoad;
    [SerializeField] private NPC npc;

    /// <summary>
    /// Changes the dialog to load when spoken to
    /// </summary>
    public void ChangeDialogToLoad(string newDialog)
    {
        dialogToLoad = newDialog;
    }

    /// <summary>
    /// Starts a dialog if free
    /// </summary>
    public override void OnInteraction()
    {
        if (!npc.isProcessingEvent)
        {
            DialogMaster.instance.StartDialog(dialogToLoad);
        }

    }

    /// <summary>
    /// Handles the OnMouseEnter event if free
    /// </summary>
    public override void OnEnter()
    {
        if (!npc.isProcessingEvent)
        {
            base.OnEnter();
        }

    }

    /// <summary>
    /// Handles the OnMouseExit event if free
    /// </summary>
    public override void OnExit()
    {
        if (!npc.isProcessingEvent)
        {
            base.OnExit();
        }

    }
}
