using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("General Infos")]
    public Texture2D hoverSprite;




    public virtual void OnMouseEnter()
    {
        InteractionManager.instance.SetCurrentObject(this);
    }


    public virtual void OnMouseExit()
    {
        InteractionManager.instance.RemoveCurrentObject(this);
    }

    public virtual void OnInteraction()
    {
        print("Interacted with " + name);
    }
}
