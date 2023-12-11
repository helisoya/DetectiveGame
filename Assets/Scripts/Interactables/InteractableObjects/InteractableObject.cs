using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Parent class of all interactable objects
/// </summary>
public class InteractableObject : MonoBehaviour
{
    [Header("General Infos")]
    public Texture2D hoverSprite;



    /// <summary>
    /// Handles the OnMouseEnter event
    /// </summary>
    public virtual void OnMouseEnter()
    {
        InteractionManager.instance.SetCurrentObject(this);
    }

    /// <summary>
    /// Handles the OnMouseExit event
    /// </summary>
    public virtual void OnMouseExit()
    {
        InteractionManager.instance.RemoveCurrentObject(this);
    }

    /// <summary>
    /// Handles the interraction
    /// </summary>
    public virtual void OnInteraction()
    {
        print("Interacted with " + name);
    }
}
