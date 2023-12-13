using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A utility class
/// </summary>
public class Utils
{
    /// <summary>
    /// Destroys the children of a parent
    /// </summary>
    /// <param name="parent">The parent transform</param>
    public static void DestroyAllChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Object.Destroy(child);
        }
    }
}
