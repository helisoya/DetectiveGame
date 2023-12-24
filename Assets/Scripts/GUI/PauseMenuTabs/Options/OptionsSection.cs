using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsSection : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] private GameObject sectionRoot;


    /// <summary>
    /// Opens the current section
    /// </summary>
    public virtual void Open()
    {
        sectionRoot.SetActive(true);
    }

    /// <summary>
    /// Closes the current section
    /// </summary>
    public virtual void Close()
    {
        sectionRoot.SetActive(false);
    }


    /// <summary>
    /// Saves the current settings
    /// </summary>
    public virtual void Save()
    {

    }

    /// <summary>
    /// Loads the current settings
    /// </summary>
    public virtual void Load()
    {

    }
}
