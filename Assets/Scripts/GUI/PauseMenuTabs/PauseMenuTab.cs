using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuTab : MonoBehaviour
{
    [Header("Common")]
    [SerializeField] private GameObject tabRoot;
    [SerializeField] private Image tabButtonSprite;
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color normalColor;

    public virtual void Open()
    {
        tabRoot.SetActive(true);
        Refresh();
    }

    public virtual void Close()
    {
        tabRoot.SetActive(false);
        tabButtonSprite.color = normalColor;
    }

    public virtual void Refresh()
    {
        tabButtonSprite.color = selectedColor;
        tabRoot.SetActive(true);
    }
}
