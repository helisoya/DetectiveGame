using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interactable object that starts a dialog when interacted with
/// </summary>
public class DialogInteractable : InteractableObject
{
    [Header("Dialog Interaction")]
    [SerializeField] private string dialogToLoad;

    /// <summary>
    /// Starts the dialog
    /// </summary>
    public override void OnInteraction()
    {
        DialogMaster.instance.StartDialog(dialogToLoad);
    }
}
