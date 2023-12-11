using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Parent class for all pause menu tabs
/// </summary>
public class PauseMenuTab : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] private GameObject tabRoot;
    [SerializeField] private Image tabButtonSprite;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color normalColor;

    /// <summary>
    /// Opens the tab
    /// </summary>
    public virtual void Open()
    {
        tabRoot.SetActive(true);
        Refresh();
    }

    /// <summary>
    /// Closes the tab
    /// </summary>
    public virtual void Close()
    {
        tabRoot.SetActive(false);
        tabButtonSprite.color = normalColor;
    }

    /// <summary>
    /// Refreshes the tab's content
    /// </summary>
    public virtual void Refresh()
    {
        tabButtonSprite.color = selectedColor;
        tabRoot.SetActive(true);
    }
}
