using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceButton : MonoBehaviour
{
    private int choice;

    [SerializeField] private TextMeshProUGUI buttonText;

    public void SetChoice(string label, int index)
    {
        choice = index;

        buttonText.text = label;
    }

    public void Click_Choice()
    {
        GameGUI.instance.Event_Choice(choice);
    }
}
