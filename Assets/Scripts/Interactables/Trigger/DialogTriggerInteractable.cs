using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Trigger that launches a dialog
/// </summary>
public class DialogTriggerInteractable : TriggerInteractable
{
    [Header("Dialog Trigger")]
    [SerializeField] private string dialogToLoad;

    public override void OnInterraction()
    {
        DialogMaster.instance.StartDialog(dialogToLoad);
    }
}
