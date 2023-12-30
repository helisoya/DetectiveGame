using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Button used to make a choice
/// </summary>
public class ChoiceButton : MonoBehaviour
{
    private int choice;

    [SerializeField] private TextMeshProUGUI buttonText;

    /// <summary>
    /// Intialize the button
    /// </summary>
    /// <param name="label">Label for the button</param>
    /// <param name="index">Choice index</param>
    public void SetChoice(string label, int index)
    {
        choice = index;

        buttonText.text = label;
    }

    /// <summary>
    /// Handles the click 
    /// </summary>
    public void Click_Choice()
    {
        GameGUI.instance.PlayButtonSFX();
        GameGUI.instance.Event_Choice(choice);
    }
}
