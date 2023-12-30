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
            Object.Destroy(child.gameObject);
        }
    }



    /// <summary>
    /// Remove rich text from a string
    /// </summary>
    /// <param name="txt">Reference for the text</param>
    /// <returns>The string with no rich text</returns>
    public static string RemoveRichText(string txt)
    {
        string res = (string)txt.Clone();
        int currentIndex = 0;

        while (currentIndex < res.Length)
        {
            if (res[currentIndex].Equals('<'))
            {
                while (!res[currentIndex].Equals('>'))
                {
                    res = res.Remove(currentIndex, 1);
                }
            }
            else
            {
                currentIndex++;
            }
        }
        return res;
    }
}
