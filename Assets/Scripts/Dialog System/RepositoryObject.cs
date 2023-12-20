using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Parent class for all objects that can be registered to the dialog system
/// </summary>
public class RepositoryObject : MonoBehaviour
{
    [SerializeField] protected string referenceName;
    [SerializeField] protected StoryObject storyObj;


    /// <summary>
    /// Register the object in the dialog system
    /// </summary>
    public virtual void AddToRepository()
    {
    }

    /// <summary>
    /// Register the object if possible
    /// </summary>
    public virtual void Start()
    {
        if ((storyObj == null || storyObj.GetCanBeEnabled()))
        {
            AddToRepository();
        }
    }
}