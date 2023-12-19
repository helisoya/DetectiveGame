using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A story object is an object that can only appears under certain circonstances.
/// It is registered in the StoryObjectsManager
/// </summary>
public class StoryObject : MonoBehaviour
{
    [SerializeField] private string[] enablingConditions;
    [SerializeField] private bool needAll;

    private bool computed = false;
    private bool value;

    void Start()
    {
        StoryObjectsManager.instance.RegisterStoryObject(this);

        RefreshIsVisible();
    }

    /// <summary>
    /// Forces the StoryObject value to another
    /// </summary>
    /// <param name="value">The new value</param>
    public void ForceSetValueTo(bool value)
    {
        computed = true;
        this.value = value;
    }

    /// <summary>
    /// Finds out if the object can be enabled. Only computes it once.
    /// </summary>
    /// <returns>Can the object be enabled ?</returns>
    public bool GetCanBeEnabled()
    {
        if (computed) return value;

        value = CanBeEnabled();
        computed = true;
        return value;
    }

    /// <summary>
    /// Computes if the object can be enabled
    /// </summary>
    /// <returns>Can the object be enabled ?</returns>
    private bool CanBeEnabled()
    {
        string[] args;
        foreach (string condition in enablingConditions)
        {
            args = condition.Split(";");

            string keyValue = GameManager.instance.GetSaveItemValue(args[0]);

            bool value = false;

            switch (args[1])
            {
                case "=":
                    value = args[2].Equals(keyValue);

                    break;
                case "<":
                    value = int.Parse(keyValue) < int.Parse(args[2]);
                    break;
                case ">":
                    value = int.Parse(keyValue) > int.Parse(args[2]);
                    break;
            }

            if (value && !needAll)
            {
                return true;
            }
            else if (!value && needAll)
            {
                return false;
            }
        }

        return needAll;
    }

    /// <summary>
    /// Refreshes if the object can be shown
    /// </summary>
    public void RefreshIsVisible()
    {
        gameObject.SetActive(GetCanBeEnabled());
    }
}
