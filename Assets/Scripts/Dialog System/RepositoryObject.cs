using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepositoryObject : MonoBehaviour
{
    [SerializeField] protected string referenceName;
    [SerializeField] protected StoryObject storyObj;

    public virtual void AddToRepository()
    {
    }

    public virtual void Start()
    {
        if (storyObj == null || storyObj.GetCanBeEnabled())
        {
            AddToRepository();
        }
    }
}