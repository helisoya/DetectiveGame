using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogInteractable : InteractableObject
{
    [Header("Dialog Interaction")]
    [SerializeField] private string dialogToLoad;

    public override void OnInteraction()
    {
        DialogMaster.instance.StartDialog(dialogToLoad);
    }
}
